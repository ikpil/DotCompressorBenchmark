using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using K4os.Compression.LZ4;

namespace DotCompressorBenchmark.Tools;

public static class Benchmarks
{
    public static List<BenchmarkResult> Benchmark(List<string> files)
    {
        try
        {
            //R.ExtractAll();

            var benchmakrs = new List<IBenchmark>();

            benchmakrs.Add(new BenchmarkMemCopy());

            benchmakrs.Add(new BenchmarkDotFastLZ(1));
            benchmakrs.Add(new BenchmarkDotFastLZ(2));

            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L00_FAST));
            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L03_HC));
            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L09_HC));
            benchmakrs.Add(new BenchmarkK4osLZ4(LZ4Level.L12_MAX));

            benchmakrs.Add(new BenchmarkSystemZip(CompressionLevel.Fastest));
            benchmakrs.Add(new BenchmarkSystemZip(CompressionLevel.Optimal));

            var results = new List<BenchmarkResult>();
            foreach (var file in files)
            {
                var srcBytes = ToBytes(file);
                var dstBytes = new byte[srcBytes.Length * 2];
            
                foreach (var benchmark in benchmakrs)
                {
                    var result = benchmark.Roundtrip(file, srcBytes.ToArray(), dstBytes);
                    results.Add(result);
            
                    Console.WriteLine(result.ToString());
                }
            }

            return results;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            //R.DeleteAll();
        }
    }

    public static byte[] ToBytes(string filepath)
    {
        using var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);

        return buffer;
    }
    
    public static BenchmarkResult Roundtrip(string name, string filename, byte[] srcBytes, byte[] dstBytes, Func<byte[], byte[], long> compress, Func<byte[], long, byte[], long> decompress)
    {
        var result = new BenchmarkResult();
        result.Name = name;
        result.FileName = filename;
        result.Times = 5;
        result.SourceByteLength = srcBytes.Length;
        result.Compression.ElapsedWatch = new Stopwatch();
        result.Compression.ElapsedWatch.Start();
        for (int i = 0; i < result.Times; ++i)
        {
            long size = compress.Invoke(srcBytes, dstBytes);
            result.Compression.InputBytes += srcBytes.Length;
            result.Compression.OutputBytes += size;
        }

        result.Compression.ElapsedWatch.Stop();

        var decompInputLength = result.Compression.OutputBytes / result.Times;

        result.Decompression.ElapsedWatch = new Stopwatch();
        result.Decompression.ElapsedWatch.Start();
        for (int i = 0; i < result.Times; ++i)
        {
            long size = decompress.Invoke(dstBytes, decompInputLength, srcBytes);
            result.Decompression.InputBytes += decompInputLength;
            result.Decompression.OutputBytes += size;
        }

        result.Decompression.ElapsedWatch.Stop();
        return result;
    }

    public static void Print(string headline, List<BenchmarkResult> results)
    {
        var rows = results
            .OrderByDescending(x => x.ComputeTotalSpeed())
            .ToList();

        int[] widths = new int[BenchmarkResult.CollSize];
        for (int i = 0; i < rows.Count; i++)
        {
            widths[0] = Math.Max(rows[i].Name.Length, widths[0]);
            widths[1] = Math.Max(rows[i].FileName.Length, widths[1]);
            widths[2] = Math.Max(rows[i].ToSourceKbString().Length, widths[2]);
            widths[3] = Math.Max(rows[i].Compression.ToSpeedString().Length, widths[3]);
            widths[4] = Math.Max(rows[i].Decompression.ToSpeedString().Length, widths[4]);
            widths[5] = Math.Max(rows[i].Compression.ToRateString().Length, widths[5]);
        }

        var headName = "Name";
        var headFilename = "Filename";
        var headFileSize = "File kB";
        var headCompMbs = "Comp. MB/s";
        var headDecompMbs = "Decomp. MB/s";
        var headRate = "Rate";

        widths[0] = Math.Max(widths[0], headName.Length) + 1;
        widths[1] = Math.Max(widths[1], headFilename.Length) + 1;
        widths[2] = Math.Max(widths[2], headFileSize.Length) + 1;
        widths[3] = Math.Max(widths[3], headCompMbs.Length) + 1;
        widths[4] = Math.Max(widths[4], headDecompMbs.Length) + 1;
        widths[5] = Math.Max(widths[5], headRate.Length) + 1;

        Console.WriteLine();
        Console.WriteLine($"### {headline} ###");
        Console.WriteLine();


        Console.WriteLine("| " +
                          headName + new string(' ', widths[0] - headName.Length) + "| " +
                          headFilename + new string(' ', widths[1] - headFilename.Length) + "| " +
                          headFileSize + new string(' ', widths[2] - headFileSize.Length) + "| " +
                          headCompMbs + new string(' ', widths[3] - headCompMbs.Length) + "| " +
                          headDecompMbs + new string(' ', widths[4] - headDecompMbs.Length) + "| " +
                          headRate + new string(' ', widths[5] - headRate.Length) + "|");
        Console.WriteLine("|-" +
                          new string('-', widths[0]) + "|-" +
                          new string('-', widths[1]) + "|-" +
                          new string('-', widths[2]) + "|-" +
                          new string('-', widths[3]) + "|-" +
                          new string('-', widths[4]) + "|-" +
                          new string('-', widths[5]) + "|");

        foreach (var row in rows)
        {
            Console.WriteLine(
                "| " +
                row.Name.PadRight(widths[0]) + "| " +
                row.FileName.PadRight(widths[1]) + "| " +
                row.ToSourceKbString().PadRight(widths[2]) + "| " +
                row.Compression.ToSpeedString().PadRight(widths[3]) + "| " +
                row.Decompression.ToSpeedString().PadRight(widths[4]) + "| " +
                row.Compression.ToRateString().PadRight(widths[5]) + "|"
            );
        }
    }
}
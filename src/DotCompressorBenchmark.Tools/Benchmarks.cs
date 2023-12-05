/*
MIT License

Copyright (c) 2023 Choi ikpil(ikpil@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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
            var benchmarks = new List<IBenchmark>();

            // MemCopy
            benchmarks.Add(new BenchmarkMemCopy());

            // FastLZ
            benchmarks.Add(new BenchmarkFastLZ(1));
            benchmarks.Add(new BenchmarkFastLZ(2));

            // LZ4
            benchmarks.Add(new BenchmarkLZ4(LZ4Level.L00_FAST));
            benchmarks.Add(new BenchmarkLZ4(LZ4Level.L03_HC));
            benchmarks.Add(new BenchmarkLZ4(LZ4Level.L06_HC));
            benchmarks.Add(new BenchmarkLZ4(LZ4Level.L09_HC));
            benchmarks.Add(new BenchmarkLZ4(LZ4Level.L12_MAX));

            // Zip
            benchmarks.Add(new BenchmarkZip(CompressionLevel.Optimal));
            benchmarks.Add(new BenchmarkZip(CompressionLevel.Fastest));
            //benchmarks.Add(new BenchmarkZip(CompressionLevel.SmallestSize));

            // GZip
            benchmarks.Add(new BenchmarkGZip(CompressionLevel.Optimal));
            benchmarks.Add(new BenchmarkGZip(CompressionLevel.Fastest));
            //benchmarks.Add(new BenchmarkGZip(CompressionLevel.SmallestSize));

            // Deflate
            benchmarks.Add(new BenchmarkDeflate(CompressionLevel.Optimal));
            benchmarks.Add(new BenchmarkDeflate(CompressionLevel.Fastest));
            //benchmarks.Add(new BenchmarkDeflate(CompressionLevel.SmallestSize));

            // Brotli
            benchmarks.Add(new BenchmarkBrotli(CompressionLevel.Optimal));
            benchmarks.Add(new BenchmarkBrotli(CompressionLevel.Fastest));
            //benchmarks.Add(new BenchmarkBrotli(CompressionLevel.SmallestSize));

            // ZLib
            benchmarks.Add(new BenchmarkZLib(CompressionLevel.Optimal));
            benchmarks.Add(new BenchmarkZLib(CompressionLevel.Fastest));
            //benchmarks.Add(new BenchmarkZLib(CompressionLevel.SmallestSize));

            benchmarks.Add(new BenchmarkSnappy());

            // LZMA
            benchmarks.Add(new BenchmarkLZMA(0));
            benchmarks.Add(new BenchmarkLZMA(2));
            benchmarks.Add(new BenchmarkLZMA(4));
            benchmarks.Add(new BenchmarkLZMA(5));
            benchmarks.Add(new BenchmarkLZMA(9));

            var results = new List<BenchmarkResult>();
            foreach (var file in files)
            {
                var srcBytes = ToBytes(file);
                var dstBytes = new byte[srcBytes.Length * 2];

                foreach (var benchmark in benchmarks)
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
        result.FileName = Path.GetFileName(filename);
        result.Times = 3;
        result.SourceSize = srcBytes.Length;
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

        int[] widths = new int[BenchmarkResult.ColumnNames.Length];
        for (int i = 0; i < rows.Count; i++)
        {
            widths[0] = Math.Max(rows[i].Name.Length, widths[0]);
            widths[1] = Math.Max(rows[i].Compression.ToSpeedString().Length, widths[1]);
            widths[2] = Math.Max(rows[i].Decompression.ToSpeedString().Length, widths[2]);
            widths[3] = Math.Max(rows[i].GetCompressedSize().ToString().Length, widths[3]);
            widths[4] = Math.Max(rows[i].Compression.ToRatioString().Length, widths[4]);
            widths[5] = Math.Max(rows[i].FileName.Length, widths[5]);
            widths[6] = Math.Max(rows[i].GetSourceSize().ToString().Length, widths[6]);
        }

        for (int i = 0; i < BenchmarkResult.ColumnNames.Length; ++i)
        {
            widths[i] = Math.Max(widths[i], BenchmarkResult.ColumnNames[i].Length) + 1;
        }

        Console.WriteLine();
        Console.WriteLine($"### {headline} ###");
        Console.WriteLine();


        var viewColumnNames = BenchmarkResult.ColumnNames.Zip(widths).Select(x => x.First + new string(' ', x.Second - x.First.Length));
        var columns = string.Join("| ", viewColumnNames);
        Console.WriteLine($"| {columns}|");

        var viewSeparators = widths.Select(x => new string('-', x));
        var separators = string.Join("|-", viewSeparators);
        Console.WriteLine($"|-{separators}|");

        foreach (var row in rows)
        {
            Console.WriteLine("| " +
                              row.Name.PadRight(widths[0]) + "| " +
                              row.Compression.ToSpeedString().PadRight(widths[1]) + "| " +
                              row.Decompression.ToSpeedString().PadRight(widths[2]) + "| " +
                              row.GetCompressedSize().ToString().PadRight(widths[3]) + "| " +
                              row.Compression.ToRatioString().PadRight(widths[4]) + "| " +
                              row.FileName.PadRight(widths[5]) + "| " +
                              row.GetSourceSize().ToString().PadRight(widths[6]) + "|"
            );
        }
    }
}
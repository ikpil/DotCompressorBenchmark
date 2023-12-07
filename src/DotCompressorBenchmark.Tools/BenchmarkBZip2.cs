using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.BZip2;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkBZip2 : IBenchmark
{
    public string Name { get; }
    private int _level;

    public BenchmarkBZip2(int level)
    {
        Name = $"bzip2 -{level}";
        _level = level;
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes,
            (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] uncompressedBytes, byte[] compressedBytes, int level)
    {
        using var compressedStream = new MemoryStream(compressedBytes);
        using (var ops = new BZip2OutputStream(compressedStream, level))
        {
            ops.IsStreamOwner = false; // leaveOpen
            ops.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return compressedStream.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using var compressedStream = new MemoryStream(compressedBytes, 0, (int)size);
        using var ips = new BZip2InputStream(compressedStream);
        return ips.Read(uncompressedBytes, 0, uncompressedBytes.Length);
    }
}
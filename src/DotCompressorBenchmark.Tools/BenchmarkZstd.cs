using System.IO;
using System.IO.Compression;
using ZstdSharp;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkZstd : IBenchmark
{
    public string Name { get; }
    private readonly int _level;

    public BenchmarkZstd(int level)
    {
        _level = level;
        Name = $"zstd -{_level.ToString()}";
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] uncompressedBytes, byte[] compressedBytes, int level)
    {
        using var compressedStream = new MemoryStream(compressedBytes);
        using (var cs = new CompressionStream(compressedStream, level))
        {
            cs.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return compressedStream.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using var ms = new MemoryStream(compressedBytes, 0, (int)size);
        using var ds = new DecompressionStream(ms);
        return ds.Read(uncompressedBytes);
    }
}
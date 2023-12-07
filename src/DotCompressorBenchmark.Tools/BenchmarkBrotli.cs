using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Lzw;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkBrotli : IBenchmark
{
    public string Name { get; }
    private readonly CompressionLevel _level;

    public BenchmarkBrotli(CompressionLevel level)
    {
        _level = level;
        Name = $"brotli -{_level.ToString()}";
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] uncompressedBytes, byte[] compressedBytes, CompressionLevel level)
    {
        using var compressedStream = new MemoryStream(compressedBytes);
        using (var ops = new BrotliStream(compressedStream, level, true))
        {
            ops.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return compressedStream.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using var compressedStream = new MemoryStream(compressedBytes, 0, (int)size);
        using var ips = new BrotliStream(compressedStream, CompressionMode.Decompress);
        return ips.Read(uncompressedBytes);
    }
}
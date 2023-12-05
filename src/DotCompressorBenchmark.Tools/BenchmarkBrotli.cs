using System.IO;
using System.IO.Compression;

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
        using MemoryStream ms = new MemoryStream(compressedBytes);
        using (BrotliStream gzipStream = new BrotliStream(ms, level, true))
        {
            gzipStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return ms.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes, 0, (int)size);
        using BrotliStream gzipStream = new BrotliStream(ms, CompressionMode.Decompress);
        return gzipStream.Read(uncompressedBytes);
    }
}
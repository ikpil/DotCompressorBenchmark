using System.IO;
using System.IO.Compression;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkDeflate : IBenchmark
{
    public string Name { get; }
    private readonly CompressionLevel _level;

    public BenchmarkDeflate(CompressionLevel level)
    {
        _level = level;
        Name = $"Deflate {_level.ToString()}";
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => CompressGZip(s, d, _level), DecompressGZip);
    }

    public static long CompressGZip(byte[] uncompressedBytes, byte[] compressedBytes, CompressionLevel level)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes);
        using (DeflateStream gzipStream = new DeflateStream(ms, level, true))
        {
            gzipStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return ms.Position;
    }

    public static long DecompressGZip(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes, 0, (int)size);
        using DeflateStream gzipStream = new DeflateStream(ms, CompressionMode.Decompress);
        return gzipStream.Read(uncompressedBytes);
    }
}
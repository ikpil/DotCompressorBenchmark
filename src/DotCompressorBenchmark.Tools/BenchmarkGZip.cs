using System.IO;
using System.IO.Compression;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkGZip : IBenchmark
{
    public string Name { get; } = "GZip";

    public BenchmarkGZip()
    {
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => CompressGZip(s, d), DecompressGZip);
    }

    private static long CompressGZip(byte[] uncompressedBytes, byte[] compressedBytes)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes);
        using (GZipStream gzipStream = new GZipStream(ms, CompressionMode.Compress, true))
        {
            gzipStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return ms.Position;
    }

    private static long DecompressGZip(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes, 0, (int)size);
        using (GZipStream gzipStream = new GZipStream(ms, CompressionMode.Decompress))
        {
            return gzipStream.Read(uncompressedBytes);
        }
    }
}
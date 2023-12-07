using System.IO;
using System.IO.Compression;
using Snappier;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkSnappy : IBenchmark
{
    public string Name { get; }

    public BenchmarkSnappy()
    {
        Name = $"snappy";
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => Compress(s, d), Decompress);
    }

    public static long Compress(byte[] uncompressedBytes, byte[] compressedBytes)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes);
        using (SnappyStream stream = new SnappyStream(ms, CompressionMode.Compress, true))
        {
            stream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return ms.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using MemoryStream ms = new MemoryStream(compressedBytes, 0, (int)size);
        using SnappyStream stream = new SnappyStream(ms, CompressionMode.Decompress);
        return stream.Read(uncompressedBytes);
    }
}
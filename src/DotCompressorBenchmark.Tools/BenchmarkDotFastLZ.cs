using DotFastLZ.Compression;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkDotFastLZ : IBenchmark
{
    public string Name { get; }

    private readonly int _level;

    public BenchmarkDotFastLZ(int level)
    {
        Name = $"DotFastLZ L{level}";
        _level = level;
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        var compress = CompressDotFastLZL1;
        if (_level == 2)
            compress = CompressDotFastLZL2;

        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, compress, DecompressDotFastLZ);
    }

    private static long CompressDotFastLZL1(byte[] srcBytes, byte[] dstBytes)
    {
        return FastLZ.CompressLevel1(srcBytes, 0, srcBytes.Length, dstBytes);
    }

    private static long CompressDotFastLZL2(byte[] srcBytes, byte[] dstBytes)
    {
        return FastLZ.CompressLevel2(srcBytes, 0, srcBytes.Length, dstBytes);
    }

    private static long DecompressDotFastLZ(byte[] srcBytes, long size, byte[] dstBytes)
    {
        return FastLZ.Decompress(srcBytes, size, dstBytes, dstBytes.Length);
    }
}
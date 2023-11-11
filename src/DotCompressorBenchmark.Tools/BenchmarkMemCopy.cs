using System;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkMemCopy : IBenchmark
{
    public string Name => "memcpy";

    //var memoryCopy = Benchmark($"memcpy", filename, srcBytes.ToArray(), dstBytes, CompressMemcpy, DecompressMemcpy);

    public BenchmarkMemCopy()
    {
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, CompressMemcpy, DecompressMemcpy);
    }


    public static long CompressMemcpy(byte[] srcBytes, byte[] dstBytes)
    {
        srcBytes.CopyTo(new Span<byte>(dstBytes));
        return srcBytes.Length;
    }

    public static long DecompressMemcpy(byte[] srcBytes, long size, byte[] dstBytes)
    {
        srcBytes.AsSpan(0, (int)size).CopyTo(new Span<byte>(dstBytes));
        return size;
    }
}
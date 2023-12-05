using System;
using System.IO;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkLZMA : IBenchmark
{
    public string Name { get; }
    private readonly int _level;

    public BenchmarkLZMA(int level)
    {
        _level = level;
        Name = $"lzma 22.1.1 -{_level}";
    }


    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes,
            (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] srcBytes, byte[] dstBytes, int level)
    {
        using MemoryStream inStream = new MemoryStream(srcBytes);
        using MemoryStream outStream = new MemoryStream(dstBytes);
        var encoder = new Encoder();
        encoder.SetCoderProperties(new CoderPropID[] { CoderPropID.Algorithm }, new object[] { level });
        encoder.WriteCoderProperties(outStream);
        outStream.Write(BitConverter.GetBytes((long)srcBytes.Length), 0, 8);
        encoder.Code(inStream, outStream, srcBytes.Length, dstBytes.Length, null);

        return outStream.Position - 5 - 8;
    }

    public static long Decompress(byte[] srcBytes, long size, byte[] dstBytes)
    {
        using var inStream = new MemoryStream(srcBytes, 0, (int)size);
        using var outStream = new MemoryStream(dstBytes);

        byte[] properties = new byte[5];
        inStream.Read(properties, 0, 5); // header

        byte[] originalSizeBytes = new byte[8];
        inStream.Read(originalSizeBytes, 0, 8); // size
        long originalSize = BitConverter.ToInt64(originalSizeBytes);

        var decoder = new Decoder();
        decoder.SetDecoderProperties(properties);
        decoder.Code(inStream, outStream, size, dstBytes.Length, null);

        return outStream.Position;
    }
}
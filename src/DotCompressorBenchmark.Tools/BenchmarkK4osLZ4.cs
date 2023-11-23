/*
MIT License

Copyright (c) 2023 Choi ikpil(ikpil@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System;
using K4os.Compression.LZ4;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkK4osLZ4 : IBenchmark
{
    public string Name { get; }

    private readonly LZ4Level _level;

    public BenchmarkK4osLZ4(LZ4Level level)
    {
        _level = level;
        Name = $"K4os.LZ L{_level.ToString()}";

        // // K4os LZ4
        // var k4osL01 = Benchmark($"K4os.Compression.LZ4 L00", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L00_FAST), DecompressK4osLZ4);
        // var k4osL12 = Benchmark($"K4os.Compression.LZ4 L12", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L12_MAX), DecompressK4osLZ4);
        // var k4osL03HC = Benchmark($"K4os.Compression.LZ4 L03_HC", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L03_HC), DecompressK4osLZ4);
        // var k4osL09HC = Benchmark($"K4os.Compression.LZ4 L09_HC", filename,
        //     srcBytes.ToArray(), dstBytes, (s, d) => CompressK4osLZ4(s, d, LZ4Level.L09_HC), DecompressK4osLZ4);
        // results.Add(k4osL01);
        // results.Add(k4osL12);
        // results.Add(k4osL03HC);
        // results.Add(k4osL09HC);
        //
    }


    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes,
            (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] srcBytes, byte[] dstBytes, LZ4Level level)
    {
        var writer = new FixedArrayBufferWriter<byte>(dstBytes);
        LZ4Pickler.Pickle(srcBytes, writer, level);
        return writer.WrittenCount;
    }

    public static long Decompress(byte[] srcBytes, long size, byte[] dstBytes)
    {
        var writer = new FixedArrayBufferWriter<byte>(dstBytes);
        LZ4Pickler.Unpickle(srcBytes.AsSpan(0, (int)size), writer);
        return writer.WrittenCount;
    }
}
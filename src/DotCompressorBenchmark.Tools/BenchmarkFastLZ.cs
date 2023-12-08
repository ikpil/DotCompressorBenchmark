﻿/*
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

using DotFastLZ.Compression;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkFastLZ : IBenchmark
{
    public string Name { get; }

    private readonly int _level;

    public BenchmarkFastLZ(int level)
    {
        Name = $"fastlz -{level}";
        _level = level;
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => Compress(s, d, _level), Decompress);
    }

    private static long Compress(byte[] uncompressedBytes, byte[] compressedBytes, int level)
    {
        if (2 == level)
        {
            return FastLZ.CompressLevel2(uncompressedBytes, 0, uncompressedBytes.Length, compressedBytes);
        }

        return FastLZ.CompressLevel1(uncompressedBytes, 0, uncompressedBytes.Length, compressedBytes);
    }


    private static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        return FastLZ.Decompress(compressedBytes, size, uncompressedBytes, uncompressedBytes.Length);
    }
}
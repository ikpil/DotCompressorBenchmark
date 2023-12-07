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

using System;
using System.IO;
using System.IO.Compression;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkZip : IBenchmark
{
    public string Name { get; }

    private readonly CompressionLevel _level;

    public BenchmarkZip(CompressionLevel level)
    {
        _level = level;
        Name = $"zip -{_level.ToString()}";
    }

    public BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes)
    {
        return Benchmarks.Roundtrip(Name, filename, srcBytes, dstBytes, (s, d) => Compress(s, d, _level), Decompress);
    }

    public static long Compress(byte[] uncompressedBytes, byte[] compressedBytes, CompressionLevel level)
    {
        using var ms = new MemoryStream(compressedBytes);
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            var entry = zip.CreateEntry("test", level);
            using var s = entry.Open();
            s.Write(uncompressedBytes, 0, uncompressedBytes.Length);
        }

        return ms.Position;
    }

    public static long Decompress(byte[] compressedBytes, long size, byte[] uncompressedBytes)
    {
        using var readStream = new MemoryStream(compressedBytes, 0, (int)size);
        using var zip = new ZipArchive(readStream, ZipArchiveMode.Read);
        var entry = zip.Entries[0];
        using var entryStream = entry.Open();
        using MemoryStream writeStream = new MemoryStream(uncompressedBytes);
        entryStream.CopyTo(writeStream);
        return writeStream.Position;
    }
}
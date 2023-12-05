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

using System.Collections.Immutable;

namespace DotCompressorBenchmark.Tools;

public class BenchmarkResult
{
    public static readonly ImmutableArray<string> ColumnNames = ImmutableArray.Create(
        "Name", "Comp. MB/s", "Decomp. MB/s", "Compr.Size", "Ratio", "Filename", "File size"
    );

    public string Name;
    public string FileName;
    public long SourceSize;

    public int Times;
    public BenchmarkSpeed Compression;
    public BenchmarkSpeed Decompression;

    public override string ToString()
    {
        var result = "";
        result += $"{Name}\n";
        result += $"  - times: {Times}\n";
        result += $"  - filename : {FileName}\n";
        result += $"  - source size : {GetSourceSize()}\n";
        result += $"  - compression size: {GetCompressedSize()}\n";
        result += $"  - compression rate: {Compression.ComputeRate():F2}%\n";
        result += $"  - compression speed: {Compression.ComputeSpeed():F2} MB/s\n";
        result += $"  - decompression speed: {Decompression.ComputeSpeed():F2} MB/s\n";

        return result;
    }

    public long GetSourceSize()
    {
        return SourceSize;
    }

    public long GetCompressedSize()
    {
        return Compression.OutputBytes / Times;
    }

    public double ComputeTotalSpeed()
    {
        return Compression.ComputeSpeed() + Decompression.ComputeSpeed();
    }
}
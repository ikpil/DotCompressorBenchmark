using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using K4os.Compression.LZ4;

namespace DotCompressorBenchmark.Tools;

public static class Program
{
    public static void Main(string[] args)
    {
        Benchmarks.Benchmark();
    }

}
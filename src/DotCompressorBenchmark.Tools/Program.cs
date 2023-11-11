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
using System.Collections.Generic;
using System.IO;

namespace DotCompressorBenchmark.Tools;

public static class Program
{
    public const string Version = "2023.11.11";

    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Usage();
            return 0;
        }

        var filenames = new List<string>();

        for (int i = 0; i < args.Length; i++)
        {
            var argument = args[i].Trim();
            if (string.IsNullOrEmpty(argument))
                continue;

            if (argument == "-h" || argument == "--help")
            {
                Usage();
                return 0;
            }

            if (argument == "-v" || argument == "--version")
            {
                Console.WriteLine($"dcbench: .net compressor benchmark tool {Version}");
                Console.WriteLine($" - DotFastLZ.Compression 2023.10.5");
                Console.WriteLine($" - K4os.Compression.LZ4 1.3.6");
                Console.WriteLine($" - EasyCompressor.LZ4 1.4.0");
                Console.WriteLine("");
                return 0;
            }

            var filepath = Path.GetFullPath(argument);
            if (!File.Exists(filepath))
            {
                Console.WriteLine($"not found file - {filepath}");
                continue;
            }

            filenames.Add(filepath);
        }

        if (0 >= filenames.Count)
        {
            Console.WriteLine($"Error: input file is not specified.");
            return 0;
        }

        var benchmarks = Benchmarks.Benchmark(filenames);
        Benchmarks.Print("Benchmark", benchmarks);

        return 0;
    }

    private static void Usage()
    {
        Console.WriteLine($"dcbench: .net compressor benchmark tool {Version}");
        Console.WriteLine($"Copyright (c) Choi Ikpil(ikpil@naver.com)");
        Console.WriteLine($" - https://github.com/ikpil/DotCompressorBenchmark");
        Console.WriteLine($"");
        Console.WriteLine($"Usage: dcbench [options] input-file");
        Console.WriteLine($"");
        // Console.WriteLine($"Options:");
        // Console.WriteLine($"  -mem  check in-memory compression speed");
        // Console.WriteLine($"");
    }
}
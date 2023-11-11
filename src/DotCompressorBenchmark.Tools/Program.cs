using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

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

            if (!File.Exists(argument))
            {
                Console.WriteLine($"not found file - {argument}");
                continue;
            }
            
            var filepath = Path.GetFullPath(argument);
            filenames.Add(filepath);
        }

        if (0 >= filenames.Count)
        {
            Console.WriteLine($"Error: input file is not specified.");
            return 0;
        }

        var benchmarks = Benchmarks.Benchmark();
        Benchmarks.Print("Benchmark", benchmarks);

        return 0;
    }

    private static void Usage()
    {
        Console.WriteLine($"dcbench: .net compressor benchmark tool {Version}");
        Console.WriteLine($"Copyright (C) Choi Ikpil(ikpil@naver.com)");
        Console.WriteLine($" - https://github.com/ikpil/DotCompressorBenchmark");
        Console.WriteLine($"");
        Console.WriteLine($"Usage: dcbench [options] input-file");
        Console.WriteLine($"");
        // Console.WriteLine($"Options:");
        // Console.WriteLine($"  -mem  check in-memory compression speed");
        // Console.WriteLine($"");
    }
}
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml)
![GitHub repo size](https://img.shields.io/github/repo-size/ikpil/DotCompressorBenchmark)
![Languages](https://img.shields.io/github/languages/top/ikpil/DotCompressorBenchmark)
[![Visitors](https://api.visitorbadge.io/api/daily?path=https%3A%2F%2Fgithub.com%2Fikpil%2FDotCompressorBenchmark&countColor=%23263759&style=flat-square)](https://visitorbadge.io/status?path=https%3A%2F%2Fgithub.com%2Fikpil%2FDotCompressorBenchmark)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/ikpil?style=flat-square&logo=GitHub-Sponsors&link=https%3A%2F%2Fgithub.com%2Fsponsors%2Fikpil)](https://github.com/sponsors/ikpil)

## Introduction 
dcbench is an in-memory benchmark of .net compressors

## Usage: DotCompressorBenchmark.Tools
```shell
$ ./dcbench --help
dcbench: .net compressor benchmark tool 2023.11.11
Copyright (c) Choi Ikpil(ikpil@naver.com)
 - https://github.com/ikpil/DotCompressorBenchmark

Usage: dcbench [options] input-file

$ ./dcbench compression-corpus/silesia/silesia.tar
```

## Supported compressors
If you have any compression libraries you'd like to add, please let me know.

- fastlz - [DotFastLZ](https://github.com/ikpil/DotFastLZ)
- lz4/lz4hc - [K4os.Compression.LZ4](https://github.com/MiloszKrajewski/K4os.Compression.LZ4)
- lzma - [LZMA-SDK](https://github.com/monemihir/LZMA-SDK)
- brotli - [system.io.compression.brotlistream](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.brotlistream)
- deflate - [system.io.compression.deflatestream](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.deflatestream)
- gzip - [system.io.compression.gzipstream](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.gzipstream)
- zlib - [system.io.compression.zlibstream](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.zlibstream)
- zip - [system.io.compression.ziparchive](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive)
- snappy - [Snappier](https://github.com/brantburnett/Snappier)
- bzip2 - [SharpZipLib](https://github.com/icsharpcode/SharpZipLib)
- zstd - [ZstdSharp](https://github.com/oleg-st/ZstdSharp)

### Benchmark
- CPU : Ryzen 3600 single core
- RAM : 64GB
- File : silesia.tar
- Size : 211948544

| Name             | Comp. MB/s | Decomp. MB/s | Compr.Size | Ratio    | Filename    | File size |
|------------------|------------|--------------|------------|----------|-------------|-----------|
| memcpy           | 7268.78    | 15795.43     | 211948544  | 100.00   | silesia.tar | 211948544 |
| lz4fast -0       | 165.21     | 2423.67      | 100881461  | 47.60    | silesia.tar | 211948544 |
| lz4hc -6         | 15.71      | 2406.49      | 78386370   | 36.98    | silesia.tar | 211948544 |
| lz4hc -12        | 3.45       | 2366.64      | 77263302   | 36.45    | silesia.tar | 211948544 |
| lz4hc -9         | 10.00      | 2356.09      | 77885122   | 36.75    | silesia.tar | 211948544 |
| lz4hc -3         | 25.75      | 2222.53      | 81343053   | 38.38    | silesia.tar | 211948544 |
| snappy           | 218.09     | 843.74       | 102380218  | 48.30    | silesia.tar | 211948544 |
| fastlz -2        | 67.31      | 365.93       | 100147467  | 47.25    | silesia.tar | 211948544 |
| fastlz -1        | 66.13      | 346.14       | 103856237  | 49.00    | silesia.tar | 211948544 |
| zip -Fastest     | 34.97      | 378.53       | 75887013   | 35.80    | silesia.tar | 211948544 |
| zip -Optimal     | 13.39      | 398.10       | 68352124   | 32.25    | silesia.tar | 211948544 |
| brotli -Fastest  | 87.12      | 261.09       | 73444548   | 34.65    | silesia.tar | 211948544 |
| deflate -Fastest | 35.66      | 210.66       | 75886907   | 35.80    | silesia.tar | 211948544 |
| gzip -Fastest    | 35.48      | 206.51       | 75886925   | 35.80    | silesia.tar | 211948544 |
| zlib -Fastest    | 34.31      | 163.11       | 75886913   | 35.80    | silesia.tar | 211948544 |
| brotli -Optimal  | 20.37      | 176.39       | 64211140   | 30.30    | silesia.tar | 211948544 |
| deflate -Optimal | 13.63      | 86.24        | 68352018   | 32.25    | silesia.tar | 211948544 |
| gzip -Optimal    | 13.70      | 81.59        | 68352036   | 32.25    | silesia.tar | 211948544 |
| zlib -Optimal    | 13.52      | 74.30        | 68352024   | 32.25    | silesia.tar | 211948544 |
| lzma 22.1.1 -5   | 0.24       | 24.07        | 49743984   | 23.47    | silesia.tar | 211948544 |
| lzma 22.1.1 -9   | 0.23       | 24.06        | 49564567   | 23.39    | silesia.tar | 211948544 |
| lzma 22.1.1 -4   | 0.23       | 23.94        | 50444814   | 23.80    | silesia.tar | 211948544 |
| lzma 22.1.1 -2   | 0.31       | 21.38        | 53730001   | 25.35    | silesia.tar | 211948544 |
| lzma 22.1.1 -0   | 0.40       | 18.04        | 59953750   | 28.29    | silesia.tar | 211948544 |
| bzip2 -1         | 1.99       | 31.60        | 60533303   | 28.56    | silesia.tar | 211948544 |
| bzip2 -5         | 1.78       | 30.28        | 55723253   | 26.29    | silesia.tar | 211948544 |
| bzip2 -9         | 1.63       | 29.83        | 54535438   | 25.73    | silesia.tar | 211948544 |
| zstd -11         | 5.22       | 801.16       | 58266350   | 27.49    | silesia.tar | 211948544 |
| zstd -15         | 1.59       | 802.37       | 57174130   | 26.98    | silesia.tar | 211948544 |
| zstd -2          | 77.17      | 716.97       | 69484490   | 32.78    | silesia.tar | 211948544 |
| zstd -8          | 11.09      | 777.71       | 60020696   | 28.32    | silesia.tar | 211948544 |
| zstd -5          | 22.69      | 704.05       | 63041984   | 29.74    | silesia.tar | 211948544 |
| zstd -1          | 87.04      | 633.52       | 73418039   | 34.64    | silesia.tar | 211948544 |
| zstd -18         | 0.69       | 694.44       | 53423198   | 25.21    | silesia.tar | 211948544 |
| zstd -22         | 0.42       | 603.32       | 52441255   | 24.74    | silesia.tar | 211948544 |



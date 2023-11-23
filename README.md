[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml)
![Repo Size](https://img.shields.io/github/repo-size/ikpil/DotCompressorBenchmark.svg?colorB=lightgray)
![Languages](https://img.shields.io/github/languages/top/ikpil/DotCompressorBenchmark)
[![Visitors](https://api.visitorbadge.io/api/daily?path=https%3A%2F%2Fgithub.com%2Fikpil%2FDotCompressorBenchmark&countColor=%23263759&style=flat-square)](https://visitorbadge.io/status?path=https%3A%2F%2Fgithub.com%2Fikpil%2FDotCompressorBenchmark)

## Introduction 
dcbench is an in-memory benchmark of .net compressors

## Usage: DotCompressorBenchmark.Tools ##
```shell
$ ./dcbench --help
dcbench: .net compressor benchmark tool 2023.11.11
Copyright (c) Choi Ikpil(ikpil@naver.com)
 - https://github.com/ikpil/DotCompressorBenchmark

Usage: dcbench [options] input-file

$ ./dcbench compression-corpus/silesia/silesia.tar
```

### Benchmark ###

| Name              | Filename    | File kB   | Comp. MB/s | Decomp. MB/s | Rate   |
|-------------------|-------------|-----------|------------|--------------|--------|
| memcpy            | silesia.tar | 206981.00 | 8089.94    | 12647.54     | 100.00 |
| K4os.LZ LL00_FAST | silesia.tar | 206981.00 | 166.41     | 2447.12      | 47.60  |
| K4os.LZ LL05_HC   | silesia.tar | 206981.00 | 18.94      | 2367.06      | 37.22  |
| K4os.LZ LL07_HC   | silesia.tar | 206981.00 | 13.51      | 2372.34      | 36.85  |
| K4os.LZ LL06_HC   | silesia.tar | 206981.00 | 15.96      | 2315.96      | 36.98  |
| K4os.LZ LL08_HC   | silesia.tar | 206981.00 | 11.62      | 2298.78      | 36.78  |
| K4os.LZ LL04_HC   | silesia.tar | 206981.00 | 21.90      | 2250.96      | 37.65  |
| K4os.LZ LL12_MAX  | silesia.tar | 206981.00 | 3.49       | 2257.13      | 36.45  |
| K4os.LZ LL11_OPT  | silesia.tar | 206981.00 | 4.42       | 2254.14      | 36.48  |
| K4os.LZ LL10_OPT  | silesia.tar | 206981.00 | 7.31       | 2232.03      | 36.61  |
| K4os.LZ LL03_HC   | silesia.tar | 206981.00 | 26.27      | 2179.22      | 38.38  |
| K4os.LZ LL09_HC   | silesia.tar | 206981.00 | 10.09      | 2195.35      | 36.75  |
| DotFastLZ L1      | silesia.tar | 206981.00 | 68.49      | 377.43       | 49.00  |
| DotFastLZ L2      | silesia.tar | 206981.00 | 70.36      | 367.11       | 47.25  |
| Zip Optimal       | silesia.tar | 206981.00 | 13.66      | 414.94       | 32.25  |
| Zip Fastest       | silesia.tar | 206981.00 | 34.39      | 381.53       | 35.80  |
| Brotli Fastest    | silesia.tar | 206981.00 | 88.58      | 230.17       | 34.65  |
| Deflate Fastest   | silesia.tar | 206981.00 | 36.06      | 239.78       | 35.80  |
| GZip Fastest      | silesia.tar | 206981.00 | 35.94      | 237.97       | 35.80  |
| ZLib Fastest      | silesia.tar | 206981.00 | 35.03      | 232.71       | 35.80  |
| Brotli Optimal    | silesia.tar | 206981.00 | 22.67      | 237.83       | 30.30  |
| GZip Optimal      | silesia.tar | 206981.00 | 14.01      | 117.18       | 32.25  |
| Deflate Optimal   | silesia.tar | 206981.00 | 14.03      | 115.28       | 32.25  |
| ZLib Optimal      | silesia.tar | 206981.00 | 13.76      | 107.26       | 32.25  |


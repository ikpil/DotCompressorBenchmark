[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml)
![Repo Size](https://img.shields.io/github/repo-size/ikpil/DotCompressorBenchmark.svg?colorB=lightgray)
![Languages](https://img.shields.io/github/languages/top/ikpil/DotCompressorBenchmark)

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

| Name                        | Filename    | File kB   | Comp. MB/s | Decomp. MB/s | Rate   |
|-----------------------------|-------------|-----------|------------|--------------|--------|
| memcpy                      | silesia.tar | 206981.00 | 9013.10    | 15519.73     | 100.00 |
| System.Io.Zip NoCompression | silesia.tar | 206981.00 | 1211.10    | 7685.98      | 100.00 |
| K4os.LZ LL00_FAST           | silesia.tar | 206981.00 | 159.84     | 2428.60      | 47.60  |
| K4os.LZ LL07_HC             | silesia.tar | 206981.00 | 13.37      | 2406.43      | 36.85  |
| K4os.LZ LL06_HC             | silesia.tar | 206981.00 | 15.71      | 2345.71      | 36.98  |
| K4os.LZ LL08_HC             | silesia.tar | 206981.00 | 11.52      | 2313.41      | 36.78  |
| K4os.LZ LL05_HC             | silesia.tar | 206981.00 | 18.54      | 2298.53      | 37.22  |
| K4os.LZ LL04_HC             | silesia.tar | 206981.00 | 22.03      | 2269.07      | 37.65  |
| K4os.LZ LL03_HC             | silesia.tar | 206981.00 | 26.10      | 2228.36      | 38.38  |
| K4os.LZ LL11_OPT            | silesia.tar | 206981.00 | 4.23       | 2202.40      | 36.48  |
| K4os.LZ LL09_HC             | silesia.tar | 206981.00 | 9.77       | 2187.10      | 36.75  |
| K4os.LZ LL12_MAX            | silesia.tar | 206981.00 | 3.30       | 2184.73      | 36.45  |
| K4os.LZ LL10_OPT            | silesia.tar | 206981.00 | 6.60       | 2156.66      | 36.61  |
| DotFastLZ L1                | silesia.tar | 206981.00 | 71.47      | 375.59       | 49.00  |
| DotFastLZ L2                | silesia.tar | 206981.00 | 71.96      | 369.80       | 47.25  |
| System.Io.Zip Optimal       | silesia.tar | 206981.00 | 13.36      | 409.62       | 32.25  |
| System.Io.Zip Fastest       | silesia.tar | 206981.00 | 33.92      | 376.51       | 35.80  |
| System.Io.Zip SmallestSize  | silesia.tar | 206981.00 | 4.44       | 405.82       | 31.92  |

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml/badge.svg)](https://github.com/ikpil/DotCompressorBenchmark/actions/workflows/codeql.yml)
![Repo Size](https://img.shields.io/github/repo-size/ikpil/DotCompressorBenchmark.svg?colorB=lightgray)
![Languages](https://img.shields.io/github/languages/top/ikpil/DotCompressorBenchmark)

## Introduction 
dcbench is an in-memory benchmark of .net compressors

## Benchmark
| Name                  | Filename    | File kB   | Comp. MB/s | Decomp. MB/s | Rate   |
|-----------------------|-------------|-----------|------------|--------------|--------|
| memcpy                | silesia.tar | 206981.00 | 9404.70    | 16149.18     | 100.00 |
| K4os.LZ LL00_FAST     | silesia.tar | 206981.00 | 165.00     | 2445.48      | 47.60  |
| K4os.LZ LL12_MAX      | silesia.tar | 206981.00 | 3.41       | 2405.24      | 36.45  |
| K4os.LZ LL09_HC       | silesia.tar | 206981.00 | 9.90       | 2362.17      | 36.75  |
| K4os.LZ LL03_HC       | silesia.tar | 206981.00 | 26.25      | 2272.51      | 38.38  |
| DotFastLZ L2          | silesia.tar | 206981.00 | 73.32      | 382.16       | 47.25  |
| DotFastLZ L1          | silesia.tar | 206981.00 | 71.67      | 380.81       | 49.00  |
| System.Io.Zip Fastest | silesia.tar | 206981.00 | 33.11      | 375.40       | 35.80  |
| System.Io.Zip Optimal | silesia.tar | 206981.00 | 13.51      | 390.04       | 32.25  |
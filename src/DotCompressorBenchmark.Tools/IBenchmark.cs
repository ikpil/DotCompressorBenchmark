namespace DotCompressorBenchmark.Tools;

public interface IBenchmark
{
    string Name => "unknown";
    BenchmarkResult Start(string filename, byte[] srcBytes, byte[] dstBytes);
}
namespace DotCompressorBenchmark.Tools;

public interface IBenchmark
{
    string Name => "unknown";
    BenchmarkResult Roundtrip(string filename, byte[] srcBytes, byte[] dstBytes);
}
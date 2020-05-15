using BenchmarkDotNet.Running;

namespace PocValidateRequestContentHash.MicroBenchmark
{
    public class Program
    {
        public static void Main()
        {
            _ = BenchmarkRunner.Run<ValidDataBenchmarks>();
            _ = BenchmarkRunner.Run<InvalidHeaderBenchmarks>();
            _ = BenchmarkRunner.Run<InvalidHashBenchmarks>();
            _ = BenchmarkRunner.Run<PoolBenchmarks>();
        }
    }
}
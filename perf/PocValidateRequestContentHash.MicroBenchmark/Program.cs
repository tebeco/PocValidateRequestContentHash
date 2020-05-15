using BenchmarkDotNet.Running;

namespace PocValidateRequestContentHash.MicroBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<ValidDataBenchmarks>();
            _ = BenchmarkRunner.Run<InvalidHashBenchmarks>();
            _ = BenchmarkRunner.Run<InvalidHeaderBenchmarks>();
            
        }
    }
}
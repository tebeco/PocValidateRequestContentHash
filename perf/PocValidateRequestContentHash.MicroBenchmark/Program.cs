using BenchmarkDotNet.Running;

namespace PocValidateRequestContentHash.MicroBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
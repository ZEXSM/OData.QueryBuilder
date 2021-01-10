using BenchmarkDotNet.Running;

namespace OData.QueryBuilder.Benchmark
{
    class Program
    {
        static void Main(string[] _) =>
            BenchmarkRunner.Run<ODataQueryBenchmark>();
    }
}

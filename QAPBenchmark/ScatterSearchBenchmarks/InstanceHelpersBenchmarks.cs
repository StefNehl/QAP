using BenchmarkDotNet.Attributes;
using Domain;
using Domain.Models;

/*
|                 Method |     Mean |    Error |  StdDev | Ratio | Allocated | Alloc Ratio |
|----------------------- |---------:|---------:|--------:|------:|----------:|------------:|
|       GetSolutionValue | 217.3 ns |  9.68 ns | 0.53 ns |  1.00 |         - |          NA |
| GetSolutionValueAsSpan | 214.6 ns | 16.60 ns | 0.91 ns |  0.99 |         - |          NA |

 */

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    //[SimpleJob(launchCount: 1, warmupCount: 3, iterationCount: 5, invocationCount:100, id: "QuickJob")]
    [ShortRunJob]
    public class InstanceHelpersBenchmarks
    {
        private QAPInstance _testInstance;
        private int[] _permutation;
        
        [GlobalSetup] 
        public async Task Setup()
        {
            var instanceReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            _testInstance = await instanceReader.ReadFileAsync("QAPLIB", "chr12a.dat");
            _permutation = new [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        }

        [Benchmark (Baseline = true)]
        public void GetSolutionValue()
        {
            InstanceHelpers.GetSolutionValue(_testInstance, _permutation);
        }
        
        [Benchmark]
        public void GetSolutionValueAsSpan()
        {
            InstanceHelpers.GetSolutionValueAsSpan(_testInstance, _permutation);
        }
    }
}


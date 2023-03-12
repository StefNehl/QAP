using BenchmarkDotNet.Attributes;
using Domain;
using Domain.Models;

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    //[SimpleJob(launchCount: 1, warmupCount: 3, iterationCount: 5, invocationCount:100, id: "QuickJob")]
    [ShortRunJob]
    public class InstanceHelpersBenchmarks
    {

        private QAPInstance testInstance;
        private int[] permutation;
        
        [GlobalSetup] 
        public async Task Setup()
        {
            var instanceReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            testInstance = await instanceReader.ReadFileAsync("QAPLIB", "chr12a.dat");
            permutation = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        }

        [Benchmark (Baseline = true)]
        public void GetSolutionValue()
        {
            InstanceHelpers.GetSolutionValue(testInstance, permutation);
        }
        
        [Benchmark]
        public void GetSolutionValueOp()
        {
            InstanceHelpers.GetSolutionValueOp(testInstance, permutation);
        }
    }
}


using BenchmarkDotNet.Attributes;
using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

[MemoryDiagnoser]
public class PathRelinkingBenchmarks
{
    private PathRelinking _pathRelinking;
    private ParallelPathRelinking _parallelPathRelinking;
    private List<InstanceSolution> _referenceSet;
    
    [Params(100, 200)] 
    public int NrOfCalls { get; set; } = 10;
    
        
    [GlobalSetup] 
    public async Task Setup()
    {
        var instanceReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var testInstance = await instanceReader.ReadFileAsync("QAPLIB", "chr12a.dat");

        _pathRelinking = new PathRelinking();
        _pathRelinking.InitMethod(testInstance);
        _parallelPathRelinking = new ParallelPathRelinking();
        _parallelPathRelinking.InitMethod(testInstance);
        
        var firstPermutation = new [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        var firstSolution = new InstanceSolution(testInstance, firstPermutation);
        
        var secondPermutation = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
        var secondSolution = new InstanceSolution(testInstance, secondPermutation);

        _referenceSet = new List<InstanceSolution>() { firstSolution, secondSolution };
    }

    [Benchmark (Baseline = true)]
    public void PathRelinking()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _pathRelinking.GetSolutions(_referenceSet);
    }
        
    [Benchmark]
    public void PathRelinking_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _parallelPathRelinking.GetSolutions(_referenceSet);
    }
}
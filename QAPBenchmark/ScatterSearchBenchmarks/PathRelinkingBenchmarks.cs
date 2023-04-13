using BenchmarkDotNet.Attributes;
using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

/*
|                 Method | NrOfCalls |       Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |  Allocated | Alloc Ratio |
|----------------------- |---------- |-----------:|---------:|---------:|------:|--------:|--------:|-----------:|------------:|
|          PathRelinking |       100 |   653.1 us | 12.61 us | 13.49 us |  1.00 |    0.00 |  7.8125 |  423.44 KB |        1.00 |
| PathRelinking_Parallel |       100 |   944.1 us |  8.27 us |  7.73 us |  1.45 |    0.03 | 10.7422 |  561.28 KB |        1.33 |
|                        |           |            |          |          |       |         |         |            |             |
|          PathRelinking |       200 | 1,305.4 us | 25.34 us | 34.68 us |  1.00 |    0.00 | 15.6250 |  846.88 KB |        1.00 |
| PathRelinking_Parallel |       200 | 2,056.7 us | 17.84 us | 13.93 us |  1.57 |    0.05 | 19.5313 | 1126.22 KB |        1.33 |
 */

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

        var improvementMethod = new ImprovedLocalSearchBestImprovement();
        improvementMethod.InitMethod(testInstance);
        
        _pathRelinking = new PathRelinking(improvementMethod, 100);
        _pathRelinking.InitMethod(testInstance);
        _parallelPathRelinking = new ParallelPathRelinking(improvementMethod, 100);
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
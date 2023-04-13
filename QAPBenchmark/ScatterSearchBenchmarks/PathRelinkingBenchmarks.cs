using BenchmarkDotNet.Attributes;
using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

/*
|                 Method | NrOfCalls |       Mean |    Error |   StdDev |     Median | Ratio | RatioSD |    Gen0 |  Allocated | Alloc Ratio |
|----------------------- |---------- |-----------:|---------:|---------:|-----------:|------:|--------:|--------:|-----------:|------------:|
|          PathRelinking |       100 |   652.4 us | 12.90 us | 13.80 us |   656.2 us |  1.00 |    0.00 |  7.8125 |  423.44 KB |        1.00 |
| PathRelinking_Parallel |       100 |   936.1 us | 17.88 us | 37.72 us |   921.7 us |  1.50 |    0.09 |  9.7656 |  561.15 KB |        1.33 |
|                        |           |            |          |          |            |       |         |         |            |             |
|          PathRelinking |       200 | 1,302.0 us | 25.96 us | 33.75 us | 1,292.1 us |  1.00 |    0.00 | 15.6250 |  846.88 KB |        1.00 |
| PathRelinking_Parallel |       200 | 1,853.6 us |  7.29 us |  6.09 us | 1,852.1 us |  1.41 |    0.05 | 19.5313 | 1122.33 KB |        1.33 |
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
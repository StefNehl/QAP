using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

/*
|                 Method | NrOfCalls |       Mean |    Error |   StdDev | Ratio | RatioSD |    Gen0 |    Gen1 |  Allocated | Alloc Ratio |
|----------------------- |---------- |-----------:|---------:|---------:|------:|--------:|--------:|--------:|-----------:|------------:|
|          PathRelinking |       100 |   670.5 us | 13.14 us | 17.09 us |  1.00 |    0.00 |  9.7656 |       - |   487.5 KB |        1.00 |
| PathRelinking_Parallel |       100 | 1,001.8 us | 17.04 us | 16.73 us |  1.48 |    0.06 | 13.6719 | 11.7188 |  673.92 KB |        1.38 |
|                        |           |            |          |          |       |         |         |         |            |             |
|          PathRelinking |       200 | 1,311.2 us | 14.86 us | 13.90 us |  1.00 |    0.00 | 19.5313 |       - |     975 KB |        1.00 |
| PathRelinking_Parallel |       200 | 1,960.3 us | 18.11 us | 16.94 us |  1.50 |    0.02 | 27.3438 | 23.4375 | 1347.64 KB |        1.38 |
 */

[MemoryDiagnoser]
public class PathRelinkingSubSetGenerationCombinedBenchmarks
{
    private PathRelinkingSubSetGenerationCombined _pathRelinkingSubSetGenerationCombined;
    private ParallelPathRelinkingSubSetGenerationCombined _parallelPathRelinkingSubSetGenerationCombined;
    private List<InstanceSolution> _referenceSet;
    
    [Params(100, 200)] 
    public int NrOfCalls { get; set; } = 10;
    
        
    [GlobalSetup] 
    public async Task Setup()
    {
        var instanceReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var testInstance = await instanceReader.ReadFileAsync("QAPLIB", "chr12a.dat");

        var improvementMethod = new ImprovedLocalSearchFirstImprovement();
        improvementMethod.InitMethod(testInstance);
        var combinationMethod = new ExhaustingPairwiseCombination();
        combinationMethod.InitMethod(testInstance);
        
        _pathRelinkingSubSetGenerationCombined = new PathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _pathRelinkingSubSetGenerationCombined.InitMethod(testInstance);
        _parallelPathRelinkingSubSetGenerationCombined = new ParallelPathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _parallelPathRelinkingSubSetGenerationCombined.InitMethod(testInstance);
        
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
            _pathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet);
    }
        
    [Benchmark]
    public void PathRelinking_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet);
    }
}
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
|          PathRelinking |       100 |   679.5 us | 13.49 us | 13.25 us |  1.00 |    0.00 |  9.7656 |       - |   487.5 KB |        1.00 |
| PathRelinking_Parallel |       100 | 1,027.6 us |  9.80 us |  8.18 us |  1.52 |    0.03 | 11.7188 |  9.7656 |  653.54 KB |        1.34 |
|                        |           |            |          |          |       |         |         |         |            |             |
|          PathRelinking |       200 | 1,366.5 us | 27.14 us | 42.26 us |  1.00 |    0.00 | 19.5313 |       - |     975 KB |        1.00 |
| PathRelinking_Parallel |       200 | 2,428.4 us | 26.30 us | 24.60 us |  1.74 |    0.05 | 23.4375 | 19.5313 | 1314.13 KB |        1.35 |
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


}
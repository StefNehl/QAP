using BenchmarkDotNet.Attributes;
using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;
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
public class SolutionGenerationBenchmarks
{
    private PathRelinking _pathRelinking;
    private ParallelPathRelinking _parallelPathRelinking;
    
    private ParallelSubSetGeneration _parallelSubSetGeneration;
    private SubSetGeneration _subSetGeneration;
    
    private PathRelinkingSubSetGenerationCombined _pathRelinkingSubSetGenerationCombined;
    private ParallelPathRelinkingSubSetGenerationCombined _parallelPathRelinkingSubSetGenerationCombined;
    
    
    private List<InstanceSolution> _referenceSet;
    
    [Params(10, 100, 200)]
    public int NrOfCalls { get; set; } = 1;

    [Params("chr12a.dat", "chr25a.dat", "tai256c.dat")]
    public string SolutionName = "tai256c.dat";


    [GlobalSetup] 
    public async Task Setup()
    {
        await Init(SolutionName);
    }

    private async Task Init(string instanceName)
    {
        var instanceReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var instance = await instanceReader.ReadFileAsync("QAPLIB", instanceName);
        
        var generationMethod = new RandomGeneratedPopulation();
        generationMethod.InitMethod(instance);

        var solutions = generationMethod.GeneratePopulation(10);
        _referenceSet = new List<InstanceSolution>();
        _referenceSet.AddRange(solutions);
        
        var improvementMethod = new ImprovedLocalSearchBestImprovement();
        improvementMethod.InitMethod(instance);
        var combinationMethod = new ExhaustingPairwiseCombination();
        combinationMethod.InitMethod(instance);
        
        _subSetGeneration = new SubSetGeneration( 1, SubSetGenerationMethodType.Cycle, combinationMethod, improvementMethod);
        _subSetGeneration.InitMethod(instance);
        _parallelSubSetGeneration = new ParallelSubSetGeneration( 1, SubSetGenerationMethodType.Cycle, combinationMethod, improvementMethod);
        _parallelSubSetGeneration.InitMethod(instance);
        
        _pathRelinking = new PathRelinking(improvementMethod);
        _pathRelinking.InitMethod(instance);
        _parallelPathRelinking = new ParallelPathRelinking(improvementMethod);
        _parallelPathRelinking.InitMethod(instance);
        
        _pathRelinkingSubSetGenerationCombined = new PathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _pathRelinkingSubSetGenerationCombined.InitMethod(instance);
        _parallelPathRelinkingSubSetGenerationCombined = new ParallelPathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _parallelPathRelinkingSubSetGenerationCombined.InitMethod(instance);
    }
    
    [Benchmark]
    public void SubSetGen()
    {
        for (int i = 0; i < NrOfCalls; i++)
        {
            _subSetGeneration.GetSolutions(_referenceSet);
        }
    }
    
    [Benchmark]
    public void SubSetGen_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
        {
            _parallelSubSetGeneration.GetSolutions(_referenceSet);
        }
    }
    
    [Benchmark]
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
    
    [Benchmark]
    public void Combination()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _pathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet);
    }
        
    [Benchmark]
    public void Combination_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet);
    }
}
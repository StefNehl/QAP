using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

[MemoryDiagnoser]
public class TestBenchmark
{
    private ImprovedLocalSearchFirstImprovement _improvedLocalSearchFirstImprovement;
    private InstanceSolution _instanceSolution;

    [GlobalSetup]
    public void SetUp()
    {
        _improvedLocalSearchFirstImprovement = new ImprovedLocalSearchFirstImprovement();
        _instanceSolution = new InstanceSolution();
    }

    [Benchmark]
    public void Benchmark()
    {
        _improvedLocalSearchFirstImprovement.ImproveSolution(_instanceSolution);
    }
}
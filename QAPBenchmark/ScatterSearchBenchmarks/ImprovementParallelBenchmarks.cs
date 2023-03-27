using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

/*
|                                                                       Method |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|----------------------------------------------------------------------------- |----------:|---------:|---------:|-------:|----------:|
|          ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions | 110.37 us | 0.415 us | 0.388 us |      - |   5.21 KB |
|  ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_WhenAll |  21.42 us | 0.276 us | 0.245 us | 0.3052 |  14.92 KB |
|  ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_WaitAll |  22.66 us | 0.208 us | 0.184 us | 0.2747 |  14.25 KB |
| ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel |  26.89 us | 0.110 us | 0.097 us | 0.2441 |  12.37 KB |
 */

[MemoryDiagnoser]
[RPlotExporter]
public class ImprovementParallelBenchmarks
{
    private LocalSearchBestImprovement bestImprovementMethod;
    
    private List<IInstanceSolution> _50solutions;
    
    [GlobalSetup]
    public void Setup()
    {
        var folderName = "QAPLIB";
        var fileName = "chr12a.dat";

        var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var instance = qapReader.ReadFileAsync(folderName, fileName).Result;

        bestImprovementMethod = new LocalSearchBestImprovement(instance);

        var permutation = new int[] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        _50solutions = new List<IInstanceSolution>();
        for (int i = 0; i < 50; i++)
        {
            var qapSolution = new InstanceSolution(instance, permutation);
            _50solutions.Add(qapSolution);
        }
    }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions()
    {
        bestImprovementMethod.ImproveSolutions(_50solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_50solutions, default);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_WaitAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync_WaitAll(_50solutions, default);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync_Parallel(_50solutions, default);
    }
}
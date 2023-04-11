using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

/*
|                                                                                       Method | Nr of Solutions |       Mean |     Error |    StdDev |   Gen0 | Allocated |
|--------------------------------------------------------------------------------------------- | ---------------:|-----------:|----------:|----------:|-------:|----------:|
|                          ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions |              50 | 111.961 us | 0.5816 us | 0.4857 us |      - |    5336 B |
|         ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_WhenAll |              50 |  23.397 us | 0.3178 us | 0.2654 us | 0.3967 |   20242 B |
|         ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_WaitAll |              50 |  22.891 us | 0.4356 us | 0.3862 us | 0.2747 |   14599 B |
| ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_ParallelForEach |              50 |  27.468 us | 0.4021 us | 0.3564 us | 0.1831 |   10193 B |
|                          ImprovedLocalSearchBestImprovement_ImproveSolutions_With20Solutions |              20 |  44.839 us | 0.1699 us | 0.1589 us |      - |    2216 B |
|         ImprovedLocalSearchBestImprovement_ImproveSolutions_With20Solutions_Parallel_WhenAll |              20 |  12.257 us | 0.0237 us | 0.0222 us | 0.1373 |    7298 B |
|                          ImprovedLocalSearchBestImprovement_ImproveSolutions_With10Solutions |              10 |  22.489 us | 0.0899 us | 0.0841 us |      - |    1176 B |
|         ImprovedLocalSearchBestImprovement_ImproveSolutions_With10Solutions_Parallel_WhenAll |              10 |   8.056 us | 0.0143 us | 0.0127 us | 0.0763 |    4457 B |
|                           ImprovedLocalSearchBestImprovement_ImproveSolutions_With5Solutions |               5 |  11.487 us | 0.0735 us | 0.0651 us | 0.0153 |     803 B |
|          ImprovedLocalSearchBestImprovement_ImproveSolutions_With5Solutions_Parallel_WhenAll |               5 |   4.922 us | 0.0928 us | 0.1239 us | 0.0458 |    2416 B |
|                            ImprovedLocalSearchBestImprovement_ImproveSolutions_With2Solution |               2 |   5.014 us | 0.0172 us | 0.0144 us | 0.0153 |     880 B |
|           ImprovedLocalSearchBestImprovement_ImproveSolutions_With2Solution_Parallel_WhenAll |               2 |   3.754 us | 0.0387 us | 0.0343 us | 0.0229 |    1528 B |
|                            ImprovedLocalSearchBestImprovement_ImproveSolutions_With1Solution |               1 |   2.513 us | 0.0195 us | 0.0163 us | 0.0076 |     472 B |
|           ImprovedLocalSearchBestImprovement_ImproveSolutions_With1Solution_Parallel_WhenAll |               1 |   3.442 us | 0.0241 us | 0.0226 us | 0.0229 |    1256 B |


 ===>> Decided to stop the parallel work at 2 Elements because the Improvement is small and the memory allocation for the parallel work is quite heavy. 
 */

[MemoryDiagnoser]
[RPlotExporter]
public class ImprovementBestSolutionParallelBenchmarks
{
    private LocalSearchBestImprovement bestImprovementMethod;
    
    private List<InstanceSolution> _50Solutions;
    private List<InstanceSolution> _20Solutions;
    private List<InstanceSolution> _10Solutions;
    private List<InstanceSolution> _5Solutions;
    private List<InstanceSolution> _2Solutions;
    private List<InstanceSolution> _1Solution;
    
    [GlobalSetup]
    public void Setup()
    {
        var folderName = "QAPLIB";
        var fileName = "chr12a.dat";

        var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var instance = qapReader.ReadFileAsync(folderName, fileName).Result;

        bestImprovementMethod = new LocalSearchBestImprovement(instance);

        var permutation = new int[] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        PopulateSolutionList(50, ref _50Solutions, instance, permutation);
        PopulateSolutionList(20, ref _20Solutions, instance, permutation);
        PopulateSolutionList(10, ref _10Solutions, instance, permutation);
        PopulateSolutionList(5, ref _5Solutions, instance, permutation);
        PopulateSolutionList(2, ref _2Solutions, instance, permutation);
        PopulateSolutionList(1, ref _1Solution, instance, permutation);


    }

    private static void PopulateSolutionList(
        int nrOfSolutions, 
        ref List<InstanceSolution> list,
        QAPInstance instance, 
        int[] permutation)
    {
        list = new List<InstanceSolution>();
        for (int i = 0; i < nrOfSolutions; i++)
        {
            var qapSolution = new InstanceSolution(instance, permutation);
            list.Add(qapSolution);
        }
    }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions()
    {
        bestImprovementMethod.ImproveSolutions(_50Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_50Solutions, default);
    }
    
    // [Benchmark]
    // public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_WaitAll()
    // {
    //     await bestImprovementMethod.ImproveSolutionsInParallelAsync_WaitAll(_50Solutions, default);
    // }
    //
    // [Benchmark]
    // public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_ParallelForEach()
    // {
    //     await bestImprovementMethod.ImproveSolutionsInParallelAsync_Parallel(_50Solutions, default);
    // }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With20Solutions()
    {
        bestImprovementMethod.ImproveSolutions(_20Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With20Solutions_Parallel_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_20Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With10Solutions()
    {
        bestImprovementMethod.ImproveSolutions(_10Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With10Solutions_Parallel_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_10Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With5Solutions()
    {
        bestImprovementMethod.ImproveSolutions(_5Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With5Solutions_Parallel_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_5Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With2Solution()
    {
        bestImprovementMethod.ImproveSolutions(_2Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With2Solution_Parallel_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_2Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchBestImprovement_ImproveSolutions_With1Solution()
    {
        bestImprovementMethod.ImproveSolutions(_1Solution);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchBestImprovement_ImproveSolutions_With1Solution_Parallel_WhenAll()
    {
        await bestImprovementMethod.ImproveSolutionsInParallelAsync(_1Solution, default);
    }
}
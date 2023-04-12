using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

/*
|                                                                                Method | Nr of Solutions |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|-------------------------------------------------------------------------------------- | ---------------:|----------:|----------:|----------:|-------:|----------:|
|                  ImprovedLocalSearchFirstImprovement_ImproveSolutions_With50Solutions |              50 | 74.486 us | 0.3129 us | 0.2443 us |      - |         - |
| ImprovedLocalSearchFirstImprovement_ImproveSolutions_With50Solutions_Parallel_WhenAll |              50 | 64.711 us | 0.0784 us | 0.0612 us | 0.1221 |   10075 B |
|                  ImprovedLocalSearchFirstImprovement_ImproveSolutions_With20Solutions |              20 | 28.817 us | 0.1310 us | 0.1161 us |      - |         - |
| ImprovedLocalSearchFirstImprovement_ImproveSolutions_With20Solutions_Parallel_WhenAll |              20 | 27.881 us | 0.0445 us | 0.0348 us | 0.0610 |    4498 B |
|                  ImprovedLocalSearchFirstImprovement_ImproveSolutions_With10Solutions |              10 | 14.353 us | 0.0534 us | 0.0417 us |      - |         - |
| ImprovedLocalSearchFirstImprovement_ImproveSolutions_With10Solutions_Parallel_WhenAll |              10 | 17.142 us | 0.0428 us | 0.0334 us | 0.0305 |    2600 B |
|                   ImprovedLocalSearchFirstImprovement_ImproveSolutions_With5Solutions |               5 |  7.155 us | 0.1019 us | 0.0904 us |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions_With5Solutions_Parallel_WhenAll |               5 |  8.391 us | 0.1380 us | 0.1223 us | 0.0305 |    1551 B |
|                    ImprovedLocalSearchFirstImprovement_ImproveSolutions_With2Solution |               2 |  3.021 us | 0.0129 us | 0.0114 us |      - |         - |
|   ImprovedLocalSearchFirstImprovement_ImproveSolutions_With2Solution_Parallel_WhenAll |               2 |  4.219 us | 0.0505 us | 0.0447 us | 0.0153 |     952 B |
|                    ImprovedLocalSearchFirstImprovement_ImproveSolutions_With1Solution |               1 |  1.273 us | 0.0088 us | 0.0083 us |      - |         - |
|   ImprovedLocalSearchFirstImprovement_ImproveSolutions_With1Solution_Parallel_WhenAll |               1 |  2.079 us | 0.0182 us | 0.0170 us | 0.0153 |     784 B |

 ===>>> Don't use parallel for the first improvement method. 

 */

[MemoryDiagnoser]
public class ImprovementFirstImprovementParallelBenchmarks
{
    private LocalSearchFirstImprovement firstImprovementMethod;
    
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

        firstImprovementMethod = new LocalSearchFirstImprovement();
        firstImprovementMethod.InitMethod(instance);

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
    public void ImprovedLocalSearchFirstImprovement_ImproveSolutions_With50Solutions()
    {
        firstImprovementMethod.ImproveSolutions(_50Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchFirstImprovement_ImproveSolutions_With50Solutions_Parallel_WhenAll()
    {
        await firstImprovementMethod.ImproveSolutionsInParallelAsync(_50Solutions, default);
    }

    [Benchmark]
    public void ImprovedLocalSearchFirstImprovement_ImproveSolutions_With20Solutions()
    {
        firstImprovementMethod.ImproveSolutions(_20Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchFirstImprovement_ImproveSolutions_With20Solutions_Parallel_WhenAll()
    {
        await firstImprovementMethod.ImproveSolutionsInParallelAsync(_20Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchFirstImprovement_ImproveSolutions_With10Solutions()
    {
        firstImprovementMethod.ImproveSolutions(_10Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchFirstImprovement_ImproveSolutions_With10Solutions_Parallel_WhenAll()
    {
        await firstImprovementMethod.ImproveSolutionsInParallelAsync(_10Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchFirstImprovement_ImproveSolutions_With5Solutions()
    {
        firstImprovementMethod.ImproveSolutions(_5Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchFirstImprovement_ImproveSolutions_With5Solutions_Parallel_WhenAll()
    {
        await firstImprovementMethod.ImproveSolutionsInParallelAsync(_5Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchFirstImprovement_ImproveSolutions_With2Solution()
    {
        firstImprovementMethod.ImproveSolutions(_2Solutions);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchFirstImprovement_ImproveSolutions_With2Solution_Parallel_WhenAll()
    {
        await firstImprovementMethod.ImproveSolutionsInParallelAsync(_2Solutions, default);
    }
    
    [Benchmark]
    public void ImprovedLocalSearchFirstImprovement_ImproveSolutions_With1Solution()
    {
        firstImprovementMethod.ImproveSolutions(_1Solution);
    }
    
    [Benchmark]
    public async Task ImprovedLocalSearchFirstImprovement_ImproveSolutions_With1Solution_Parallel_WhenAll()
    {
        await firstImprovementMethod.ImproveSolutionsInParallelAsync(_1Solution, default);
    }
}
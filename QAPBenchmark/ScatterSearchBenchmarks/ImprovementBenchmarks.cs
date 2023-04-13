using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

    //|                                                               Method |       Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
    //|--------------------------------------------------------------------- |-----------:|----------:|----------:|-------:|-------:|----------:|
    //|                           LocalSearchBestImprovement_ImproveSolution |   3.544 us | 0.0307 us | 0.0287 us | 0.1106 |      - |     472 B |
    //|          LocalSearchBestImprovement_ImproveSolutions_With10Solutions |         NA |        NA |        NA |      - |      - |         - |
    //|          LocalSearchBestImprovement_ImproveSolutions_With50Solutions |         NA |        NA |        NA |      - |      - |         - |
    //|  LocalSearchBestImprovement_ImproveSolutionsParallel_With10Solutions |   8.549 us | 0.3314 us | 0.9563 us | 0.3052 | 0.1373 |    1814 B |
    //|  LocalSearchBestImprovement_ImproveSolutionsParallel_With50Solutions |   8.886 us | 0.3465 us | 1.0052 us | 0.3510 | 0.1678 |    2066 B |
    //|                          LocalSearchFirstImprovement_ImproveSolution |   1.752 us | 0.0326 us | 0.0289 us |      - |      - |         - |
    //|         LocalSearchFirstImprovement_ImproveSolutions_With10Solutions |  19.823 us | 0.0581 us | 0.0454 us |      - |      - |         - |
    //|         LocalSearchFirstImprovement_ImproveSolutions_With50Solutions | 102.986 us | 1.1435 us | 0.9549 us |      - |      - |         - |
    //| LocalSearchFirstImprovement_ImproveSolutionsParallel_With10Solutions |   8.345 us | 0.4718 us | 1.3763 us | 0.1068 | 0.0458 |     736 B |
    //| LocalSearchFirstImprovement_ImproveSolutionsParallel_With50Solutions |   9.490 us | 0.8193 us | 2.2839 us | 0.1068 | 0.0458 |     736 B |

/*
|                                                Method | NrOfSolutions |         Mean |       Error |      StdDev |   Gen0 | Allocated |
|------------------------------------------------------ |-------------- |-------------:|------------:|------------:|-------:|----------:|
|            LocalSearchBestImprovement_ImproveSolution |            10 |  24,510.6 ns |   473.81 ns |   465.35 ns |      - |     320 B |
|    ImprovedLocalSearchBestImprovement_ImproveSolution |            10 |   6,453.3 ns |   125.00 ns |   138.93 ns | 0.0153 |    1040 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |            10 |   6,529.8 ns |    94.83 ns |    84.07 ns | 0.0153 |    1040 B |
|          LocalSearchFirstImprovement_ImproveSolutions |            10 |  15,919.6 ns |   274.84 ns |   257.09 ns |      - |         - |
|   ImprovedLocalSearchFirstImprovement_ImproveSolution |            10 |     606.1 ns |    11.69 ns |    12.51 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |            10 |   3,697.4 ns |    47.39 ns |    44.32 ns | 0.0687 |    3464 B |
|            LocalSearchBestImprovement_ImproveSolution |            50 | 123,714.0 ns | 2,449.57 ns | 2,621.01 ns |      - |    1600 B |
|    ImprovedLocalSearchBestImprovement_ImproveSolution |            50 |  32,216.1 ns |   616.65 ns |   710.13 ns | 0.0610 |    5200 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |            50 |  31,684.9 ns |   404.86 ns |   378.71 ns | 0.0610 |    5200 B |
|          LocalSearchFirstImprovement_ImproveSolutions |            50 |  82,942.9 ns | 1,634.72 ns | 1,882.54 ns |      - |         - |
|   ImprovedLocalSearchFirstImprovement_ImproveSolution |            50 |   3,014.5 ns |    58.14 ns |    62.21 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |            50 |  14,467.7 ns |    78.52 ns |    73.44 ns | 0.3052 |   16120 B |
|            LocalSearchBestImprovement_ImproveSolution |           100 | 246,779.0 ns | 4,722.14 ns | 4,417.09 ns |      - |    3200 B |
|    ImprovedLocalSearchBestImprovement_ImproveSolution |           100 |  64,000.3 ns | 1,238.17 ns | 1,425.87 ns | 0.1221 |   10400 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |           100 |  64,383.4 ns | 1,258.91 ns | 1,498.64 ns | 0.1221 |   10400 B |
|          LocalSearchFirstImprovement_ImproveSolutions |           100 | 165,796.1 ns | 2,903.83 ns | 2,716.25 ns |      - |         - |
|   ImprovedLocalSearchFirstImprovement_ImproveSolution |           100 |   6,018.9 ns |   112.05 ns |   115.06 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |           100 |  27,125.7 ns |   236.49 ns |   221.22 ns | 0.6409 |   31969 B |
*/

namespace QAPBenchmark.ScatterSearchBenchmarks
{


    [MemoryDiagnoser]
    public class ImprovementBenchmarks
    {
        private IImprovementMethod _bestImprovementMethod;
        private IImprovementMethod _improvedBestImprovementMethod;
        private IImprovementMethod _improvedBestImprovementParallelMethod;
        
        private IImprovementMethod _firstImprovementMethod;
        private IImprovementMethod _improvedFirstImprovementMethod;
        private IImprovementMethod _improvedFirstImprovementParallelMethod;

        private List<InstanceSolution> _solutions;

        [Params(10, 50, 100)]
        public int NrOfSolutions { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            var instance = qapReader.ReadFileAsync(folderName, fileName).Result;

            _bestImprovementMethod = new LocalSearchBestImprovement();
            _bestImprovementMethod.InitMethod(instance);
            _improvedBestImprovementMethod = new ImprovedLocalSearchBestImprovement();
            _improvedBestImprovementMethod.InitMethod(instance);
            _improvedBestImprovementParallelMethod = new ImprovedLocalSearchBestImprovement();
            _improvedBestImprovementParallelMethod.InitMethod(instance);

            _firstImprovementMethod = new LocalSearchFirstImprovement();
            _firstImprovementMethod.InitMethod(instance);
            _improvedFirstImprovementMethod = new ImprovedLocalSearchFirstImprovement();
            _improvedFirstImprovementMethod.InitMethod(instance);
            _improvedFirstImprovementParallelMethod = new ImprovedLocalSearchBestImprovementParallel();
            _improvedFirstImprovementParallelMethod.InitMethod(instance);
            
            var permutation = new [] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            _solutions = new List<InstanceSolution>();
            for (int i = 0; i < NrOfSolutions; i++)
            {
                var qapSolution = new InstanceSolution(instance, permutation);
                _solutions.Add(qapSolution);
            }
        }

        [Benchmark]
        public void LocalSearchBestImprovement_ImproveSolution()
        {
            _bestImprovementMethod.ImproveSolutions(_solutions);
        }
        
        [Benchmark]
        public void ImprovedLocalSearchBestImprovement_ImproveSolution()
        {
            _improvedBestImprovementMethod.ImproveSolutions(_solutions);
        }

        [Benchmark]
        public void LocalSearchBestImprovement_ImproveSolutions_Parallel()
        {
            _improvedBestImprovementParallelMethod.ImproveSolutions(_solutions);
        }

        [Benchmark]
        public void LocalSearchFirstImprovement_ImproveSolutions()
        {
            _firstImprovementMethod.ImproveSolutions(_solutions);
        }
        
        [Benchmark]
        public void ImprovedLocalSearchFirstImprovement_ImproveSolution()
        {
            _improvedFirstImprovementMethod.ImproveSolutions(_solutions);
        }

        [Benchmark]
        public void LocalSearchFirstImprovement_ImproveSolutions_Parallel()
        {
            _improvedFirstImprovementParallelMethod.ImproveSolutions(_solutions);
        }
        
    }
}

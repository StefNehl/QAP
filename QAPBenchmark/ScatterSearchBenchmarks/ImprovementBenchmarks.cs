using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks
{

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
            _improvedBestImprovementMethod = new ImprovedLocalSearchBestImprovement();
            _improvedBestImprovementParallelMethod = new ImprovedLocalSearchBestImprovement();

            _firstImprovementMethod = new LocalSearchFirstImprovement();
            _improvedFirstImprovementMethod = new ImprovedLocalSearchFirstImprovement();
            _improvedFirstImprovementParallelMethod = new ImprovedLocalSearchBestImprovementParallel();
            
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

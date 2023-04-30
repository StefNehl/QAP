using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;

/*
|                                                Method | SolutionName | NrOfSolutions |               Mean |            Error |           StdDev |   Gen0 | Allocated |
|------------------------------------------------------ |------------- |-------------- |-------------------:|-----------------:|-----------------:|-------:|----------:|
|           LocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |            10 |        23,851.8 ns |         96.51 ns |         90.27 ns |      - |     320 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |            10 |         6,192.5 ns |         19.83 ns |         16.56 ns | 0.0153 |    1040 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr12a.dat |            10 |         6,237.2 ns |         16.25 ns |         15.20 ns | 0.0153 |    1040 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |            10 |        15,194.5 ns |         50.96 ns |         45.17 ns |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |            10 |           589.9 ns |          3.34 ns |          3.13 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr12a.dat |            10 |         3,181.5 ns |         50.51 ns |         47.24 ns | 0.0458 |    2424 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           100 |       240,324.3 ns |        886.19 ns |        785.59 ns |      - |    3200 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           100 |        62,069.7 ns |        186.44 ns |        174.39 ns | 0.1221 |   10400 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr12a.dat |           100 |        62,714.1 ns |        468.95 ns |        438.66 ns | 0.1221 |   10400 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           100 |       162,872.3 ns |        609.56 ns |        570.18 ns |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           100 |         5,857.5 ns |         32.46 ns |         28.77 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr12a.dat |           100 |        33,204.0 ns |        117.76 ns |        110.15 ns | 0.4272 |   21568 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           200 |       479,082.2 ns |      3,398.24 ns |      2,837.69 ns |      - |    6400 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           200 |       124,489.5 ns |        288.86 ns |        256.07 ns | 0.2441 |   20800 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr12a.dat |           200 |       123,897.7 ns |        518.49 ns |        484.99 ns | 0.2441 |   20800 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           200 |       326,895.1 ns |        768.98 ns |        719.30 ns |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           200 |        11,647.9 ns |         58.98 ns |         55.17 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr12a.dat |           200 |        66,568.1 ns |        497.64 ns |        465.49 ns | 0.7324 |   42840 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |            10 |       216,315.3 ns |        637.63 ns |        596.44 ns |      - |     320 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |            10 |        25,782.9 ns |         90.26 ns |         80.02 ns | 0.0305 |    1600 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr25a.dat |            10 |        25,876.9 ns |        116.10 ns |        108.60 ns | 0.0305 |    1600 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |            10 |       100,693.5 ns |        319.44 ns |        298.80 ns |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |            10 |         1,213.5 ns |          1.77 ns |          1.48 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr25a.dat |            10 |         3,051.6 ns |         38.02 ns |         35.57 ns | 0.0458 |    2424 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           100 |     2,175,527.3 ns |      6,009.99 ns |      5,327.70 ns |      - |    3203 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           100 |       258,262.8 ns |        960.46 ns |        898.41 ns |      - |   16000 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr25a.dat |           100 |       257,997.5 ns |        553.16 ns |        517.42 ns |      - |   16000 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           100 |       860,889.3 ns |      2,713.48 ns |      2,538.19 ns |      - |       1 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           100 |        11,213.0 ns |         32.72 ns |         29.01 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr25a.dat |           100 |        32,420.8 ns |         44.54 ns |         41.67 ns | 0.4272 |   21568 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           200 |     4,375,272.3 ns |     30,791.32 ns |     27,295.70 ns |      - |    6406 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           200 |       515,707.0 ns |      1,008.79 ns |        943.62 ns |      - |   32001 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr25a.dat |           200 |       517,141.6 ns |      1,239.24 ns |      1,034.82 ns |      - |   32001 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           200 |     1,692,041.8 ns |      7,904.33 ns |      7,393.72 ns |      - |       2 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           200 |        22,330.8 ns |        137.18 ns |        128.32 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr25a.dat |           200 |        65,326.4 ns |        247.87 ns |        231.86 ns | 0.7324 |   42840 B |
|           LocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |            10 |   247,470,404.4 ns |    948,630.71 ns |    887,349.76 ns |      - |     696 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |            10 |     2,719,829.5 ns |      5,684.57 ns |      5,039.22 ns |      - |   10804 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |  tai256c.dat |            10 |     2,719,768.8 ns |     13,137.14 ns |     12,288.49 ns |      - |   10804 B |
|          LocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |            10 |   248,366,414.3 ns |    362,053.17 ns |    320,950.71 ns |      - |     376 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |            10 |        10,529.8 ns |         27.82 ns |         26.02 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |  tai256c.dat |            10 |         4,513.7 ns |         17.04 ns |         15.94 ns | 0.0458 |    2424 B |
|           LocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           100 | 2,446,369,378.6 ns |  7,712,243.92 ns |  6,836,703.35 ns |      - |    4328 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           100 |    27,175,281.5 ns |     85,027.75 ns |     75,374.89 ns |      - |  108035 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |  tai256c.dat |           100 |    27,092,525.2 ns |     52,758.86 ns |     44,056.04 ns |      - |  108035 B |
|          LocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           100 | 2,484,791,492.9 ns | 13,133,815.39 ns | 11,642,785.25 ns |      - |    1128 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           100 |       104,759.5 ns |        641.47 ns |        600.03 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |  tai256c.dat |           100 |        20,826.5 ns |        227.74 ns |        213.03 ns | 0.4272 |   21568 B |
|           LocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           200 | 4,921,752,353.3 ns | 18,245,687.68 ns | 17,067,027.57 ns |      - |    7528 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           200 |    53,882,977.3 ns |    344,677.18 ns |    322,411.24 ns |      - |  216113 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |  tai256c.dat |           200 |    53,875,483.3 ns |    213,647.91 ns |    199,846.39 ns |      - |  216113 B |
|          LocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           200 | 4,971,886,535.7 ns | 47,302,500.17 ns | 41,932,434.31 ns |      - |    1128 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           200 |       210,583.3 ns |        929.91 ns |        869.84 ns |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |  tai256c.dat |           200 |        37,925.9 ns |        506.27 ns |        473.56 ns | 0.7935 |   42842 B |
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

        [Params(10, 100, 200)] 
        public int NrOfSolutions { get; set; } = 10;
        
        [Params("chr12a.dat", "chr25a.dat", "tai256c.dat")]
        public string SolutionName = "tai256c.dat";

        [GlobalSetup]
        public void Setup()
        {
            var folderName = "QAPLIB";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            var instance = qapReader.ReadFileAsync(folderName, SolutionName).Result;

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
            _improvedFirstImprovementParallelMethod = new ParallelImprovedLocalSearchFirstImprovement();
            _improvedFirstImprovementParallelMethod.InitMethod(instance);
            
            var generationMethod = new RandomGeneratedPopulation(42);
            generationMethod.InitMethod(instance);
            var permutation = generationMethod.GeneratePopulation(1).First().SolutionPermutation;

            _solutions = new List<InstanceSolution>();
            for (int i = 0; i < NrOfSolutions; i++)
            {
                var qapSolution = new InstanceSolution(instance, permutation);
                _solutions.Add(qapSolution);
            }
        }

        [Benchmark]
        public void LocalSearchBestImprovement_ImproveSolutions()
        {
            _bestImprovementMethod.ImproveSolutions(_solutions);
        }
        
        [Benchmark]
        public void ImprovedLocalSearchBestImprovement_ImproveSolutions()
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
        public void ImprovedLocalSearchFirstImprovement_ImproveSolutions()
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

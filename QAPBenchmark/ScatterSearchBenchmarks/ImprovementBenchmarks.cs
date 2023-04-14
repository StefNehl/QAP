using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;

/*
|                                                Method | SolutionName | NrOfSolutions |               Mean |            Error |            StdDev |   Gen0 |   Gen1 | Allocated |
|------------------------------------------------------ |------------- |-------------- |-------------------:|-----------------:|------------------:|-------:|-------:|----------:|
|           LocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |            10 |        24,110.9 ns |        150.17 ns |         133.12 ns |      - |      - |     320 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |            10 |         6,243.9 ns |         36.39 ns |          34.04 ns | 0.0153 |      - |    1040 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr12a.dat |            10 |         6,268.9 ns |         30.81 ns |          28.82 ns | 0.0153 |      - |    1040 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |            10 |         8,480.8 ns |         53.98 ns |          50.49 ns |      - |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |            10 |           594.8 ns |          3.24 ns |           2.71 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr12a.dat |            10 |         3,608.6 ns |          6.60 ns |           5.51 ns | 0.0687 |      - |    3464 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           100 |       240,373.8 ns |      1,227.23 ns |       1,147.95 ns |      - |      - |    3200 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           100 |        62,363.8 ns |        197.70 ns |         175.26 ns | 0.1221 |      - |   10400 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr12a.dat |           100 |        62,467.1 ns |        269.44 ns |         252.03 ns | 0.1221 |      - |   10400 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           100 |       131,705.2 ns |        497.89 ns |         465.73 ns |      - |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           100 |         5,933.5 ns |         26.97 ns |          25.22 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr12a.dat |           100 |        26,806.7 ns |        510.09 ns |         477.14 ns | 0.6409 |      - |   31968 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           200 |       493,693.0 ns |      9,658.76 ns |       9,486.19 ns |      - |      - |    6401 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr12a.dat |           200 |       124,711.2 ns |        260.60 ns |         217.61 ns | 0.2441 |      - |   20800 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr12a.dat |           200 |       124,909.0 ns |        427.27 ns |         399.67 ns | 0.2441 |      - |   20800 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           200 |       220,562.3 ns |      1,012.75 ns |         947.32 ns |      - |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr12a.dat |           200 |        11,869.3 ns |         67.58 ns |          56.44 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr12a.dat |           200 |        60,681.5 ns |        688.83 ns |         644.33 ns | 1.2207 | 0.0610 |   63641 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |            10 |       219,671.7 ns |      1,259.11 ns |       1,177.78 ns |      - |      - |     320 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |            10 |        29,866.7 ns |        556.59 ns |         520.63 ns | 0.0305 |      - |    1600 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr25a.dat |            10 |        25,970.7 ns |        137.91 ns |         129.00 ns | 0.0305 |      - |    1600 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |            10 |       149,545.6 ns |        475.82 ns |         421.80 ns |      - |      - |         - |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |            10 |         1,131.5 ns |          3.27 ns |           2.90 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr25a.dat |            10 |         7,934.3 ns |         50.33 ns |          42.03 ns | 0.0763 |      - |    4024 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           100 |     2,213,306.8 ns |     10,200.42 ns |       9,541.48 ns |      - |      - |    3203 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           100 |       260,702.8 ns |      1,034.80 ns |         917.32 ns |      - |      - |   16000 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr25a.dat |           100 |       260,383.3 ns |        894.93 ns |         837.12 ns |      - |      - |   16000 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           100 |     1,684,755.4 ns |      9,759.69 ns |       9,129.22 ns |      - |      - |       2 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           100 |        11,233.5 ns |         55.81 ns |          52.20 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr25a.dat |           100 |        42,221.4 ns |        488.35 ns |         456.80 ns | 0.7324 |      - |   37570 B |
|           LocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           200 |     4,406,187.7 ns |     17,307.73 ns |      16,189.66 ns |      - |      - |    6406 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |   chr25a.dat |           200 |       522,128.3 ns |      2,339.48 ns |       2,188.36 ns |      - |      - |   32001 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |   chr25a.dat |           200 |       519,100.0 ns |      2,240.91 ns |       2,096.15 ns |      - |      - |   32001 B |
|          LocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           200 |     2,180,084.2 ns |      8,317.87 ns |       7,373.58 ns |      - |      - |       3 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |   chr25a.dat |           200 |        22,508.7 ns |        109.76 ns |         102.67 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |   chr25a.dat |           200 |        77,443.7 ns |        589.33 ns |         522.43 ns | 1.4648 |      - |   74844 B |
|           LocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |            10 |   250,176,577.8 ns |  1,588,988.30 ns |   1,486,340.65 ns |      - |      - |     696 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |            10 |     2,769,430.7 ns |     19,441.24 ns |      18,185.35 ns |      - |      - |   10804 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |  tai256c.dat |            10 |     2,746,795.7 ns |     14,098.63 ns |      13,187.87 ns |      - |      - |   10804 B |
|          LocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |            10 |   249,867,660.0 ns |  1,177,090.71 ns |   1,101,051.38 ns |      - |      - |    4488 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |            10 |        10,698.4 ns |         42.96 ns |          40.18 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |  tai256c.dat |            10 |       566,038.0 ns |        735.32 ns |         651.84 ns |      - |      - |   13289 B |
|           LocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           100 | 2,514,109,123.1 ns |  7,561,840.36 ns |   6,314,479.17 ns |      - |      - |    4328 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           100 |    27,435,833.3 ns |    166,584.69 ns |     155,823.42 ns |      - |      - |  108035 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |  tai256c.dat |           100 |    27,391,034.4 ns |    156,597.43 ns |     146,481.34 ns |      - |      - |  108035 B |
|          LocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           100 | 2,471,041,913.4 ns |  7,508,594.19 ns |   6,656,173.17 ns |      - |      - |      70 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           100 |       107,901.7 ns |        370.39 ns |         346.46 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |  tai256c.dat |           100 |     3,616,934.7 ns |     30,613.90 ns |      27,138.42 ns |      - |      - |  129636 B |
|           LocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           200 | 5,021,394,157.1 ns | 45,500,134.86 ns |  40,334,684.41 ns |      - |      - |    7528 B |
|   ImprovedLocalSearchBestImprovement_ImproveSolutions |  tai256c.dat |           200 |    56,399,742.8 ns |    976,923.94 ns |   1,045,297.67 ns |      - |      - |  216119 B |
|  LocalSearchBestImprovement_ImproveSolutions_Parallel |  tai256c.dat |           200 |    56,297,701.8 ns |    844,599.03 ns |     867,341.02 ns |      - |      - |  216113 B |
|          LocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           200 | 5,065,918,090.0 ns | 99,627,920.02 ns | 114,731,660.86 ns |      - |      - |    1128 B |
|  ImprovedLocalSearchFirstImprovement_ImproveSolutions |  tai256c.dat |           200 |       217,035.8 ns |      3,653.01 ns |       3,417.03 ns |      - |      - |         - |
| LocalSearchFirstImprovement_ImproveSolutions_Parallel |  tai256c.dat |           200 |     6,994,801.8 ns |     14,253.64 ns |      11,902.43 ns |      - |      - |  258913 B |
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
            
            var generationMethod = new RandomGeneratedPopulation();
            generationMethod.InitMethod(instance);
            var permutation = generationMethod.GeneratePopulation(1).First().SolutionPermutation;

            _solutions = new List<InstanceSolution>();
            for (int i = 0; i < NrOfSolutions; i++)
            {
                var qapSolution = new InstanceSolution(instance, permutation);
                _solutions.Add(qapSolution);
            }
        }

        // [Benchmark]
        // public void LocalSearchBestImprovement_ImproveSolutions()
        // {
        //     _bestImprovementMethod.ImproveSolutions(_solutions);
        // }
        //
        // [Benchmark]
        // public void ImprovedLocalSearchBestImprovement_ImproveSolutions()
        // {
        //     _improvedBestImprovementMethod.ImproveSolutions(_solutions);
        // }
        //
        // [Benchmark]
        // public void LocalSearchBestImprovement_ImproveSolutions_Parallel()
        // {
        //     _improvedBestImprovementParallelMethod.ImproveSolutions(_solutions);
        // }
        //
        // [Benchmark]
        // public void LocalSearchFirstImprovement_ImproveSolutions()
        // {
        //     _firstImprovementMethod.ImproveSolutions(_solutions);
        // }
        //
        // [Benchmark]
        // public void ImprovedLocalSearchFirstImprovement_ImproveSolutions()
        // {
        //     _improvedFirstImprovementMethod.ImproveSolutions(_solutions);
        // }

        [Benchmark]
        public void LocalSearchFirstImprovement_ImproveSolutions_Parallel()
        {
            _improvedFirstImprovementParallelMethod.ImproveSolutions(_solutions);
        }
        
    }
}

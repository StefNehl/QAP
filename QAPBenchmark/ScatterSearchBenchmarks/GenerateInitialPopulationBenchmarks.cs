using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;

/*
|                       Method | SolutionName | NrOfSolutions |          Mean |       Error |      StdDev |        Median |   Gen0 | Allocated |
|----------------------------- |------------- |-------------- |--------------:|------------:|------------:|--------------:|-------:|----------:|
| StepWisePopulationGeneration |   chr12a.dat |            10 |      4.874 us |   0.0974 us |   0.1365 us |      4.840 us | 0.0153 |    1016 B |
|    RandomGeneratedPopulation |   chr12a.dat |            10 |      7.363 us |   0.1299 us |   0.1215 us |      7.419 us | 0.0153 |    1016 B |
| StepWisePopulationGeneration |   chr12a.dat |           100 |     48.247 us |   0.9508 us |   1.1319 us |     47.491 us | 0.1831 |    9656 B |
|    RandomGeneratedPopulation |   chr12a.dat |           100 |     73.880 us |   1.4565 us |   1.4305 us |     74.652 us | 0.1221 |    9656 B |
| StepWisePopulationGeneration |   chr12a.dat |           200 |     97.961 us |   1.9206 us |   1.8863 us |     97.865 us | 0.3662 |   19256 B |
|    RandomGeneratedPopulation |   chr12a.dat |           200 |    150.276 us |   2.8657 us |   3.7262 us |    149.364 us | 0.2441 |   19256 B |
| StepWisePopulationGeneration |   chr25a.dat |            10 |     14.548 us |   0.2871 us |   0.3191 us |     14.574 us | 0.0305 |    1576 B |
|    RandomGeneratedPopulation |   chr25a.dat |            10 |     20.315 us |   0.3975 us |   0.4082 us |     20.432 us | 0.0305 |    1576 B |
| StepWisePopulationGeneration |   chr25a.dat |           100 |    147.654 us |   2.8489 us |   3.3914 us |    148.920 us | 0.2441 |   15256 B |
|    RandomGeneratedPopulation |   chr25a.dat |           100 |    203.134 us |   3.3428 us |   2.9633 us |    204.264 us | 0.2441 |   15256 B |
| StepWisePopulationGeneration |   chr25a.dat |           200 |    292.767 us |   5.7077 us |   6.7946 us |    294.985 us | 0.4883 |   30456 B |
|    RandomGeneratedPopulation |   chr25a.dat |           200 |    406.864 us |   7.8363 us |   8.3848 us |    408.455 us | 0.4883 |   30456 B |
| StepWisePopulationGeneration |  tai256c.dat |            10 |  1,024.597 us |   3.4116 us |   2.6636 us |  1,023.710 us |      - |   10778 B |
|    RandomGeneratedPopulation |  tai256c.dat |            10 |  1,128.514 us |  22.1456 us |  25.5029 us |  1,134.361 us |      - |   10778 B |
| StepWisePopulationGeneration |  tai256c.dat |           100 | 10,402.005 us | 130.7088 us | 102.0488 us | 10,443.596 us |      - |  107275 B |
|    RandomGeneratedPopulation |  tai256c.dat |           100 | 11,398.299 us | 218.6564 us | 224.5440 us | 11,491.681 us |      - |  107274 B |
| StepWisePopulationGeneration |  tai256c.dat |           200 | 20,904.533 us | 369.8356 us | 345.9445 us | 21,082.506 us |      - |  214491 B |
|    RandomGeneratedPopulation |  tai256c.dat |           200 | 22,909.002 us | 445.1163 us | 476.2695 us | 23,022.966 us |      - |  214491 B |
 */

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    public class GenerateInitialPopulationBenchmarks
    {
        private int solutionSize;
        private IGenerateInitPopulationMethod _stepWiseGenerationPopulation;
        private IGenerateInitPopulationMethod _randomGenerationPopulation;
        private ParallelRandomGeneratedPopulation _parallelRandomGeneratedPopulation;

        [Params(10, 100, 200)]
        public int NrOfSolutions { get; set; } = 10;
        
        [Params("chr12a.dat", "chr25a.dat", "tai256c.dat")]
        public string SolutionName = "tai256c.dat";
        
        [GlobalSetup] 
        public void Setup() 
        {
            solutionSize = 10;
            var folderName = "QAPLIB";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            var instance = qapReader.ReadFileAsync(folderName, SolutionName).Result;
            
            _stepWiseGenerationPopulation = new StepWisePopulationGeneration(1);
            _stepWiseGenerationPopulation.InitMethod(instance);

            _randomGenerationPopulation = new RandomGeneratedPopulation(42);
            _randomGenerationPopulation.InitMethod(instance);

            _parallelRandomGeneratedPopulation = new ParallelRandomGeneratedPopulation(42);
            
        }

        [Benchmark]
        public void StepWisePopulationGeneration()
        {
            _stepWiseGenerationPopulation.GeneratePopulation(NrOfSolutions);
        }
        
        [Benchmark]
        public void RandomGeneratedPopulation()
        {
            _randomGenerationPopulation.GeneratePopulation(NrOfSolutions);
        }
        
        [Benchmark]
        public void ParallelRandomGeneratedPopulation()
        {
            _parallelRandomGeneratedPopulation.GeneratePopulation(NrOfSolutions);
        }

    }
}

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
|                       Method | NrOfSolutions |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|----------------------------- |-------------- |----------:|---------:|---------:|-------:|----------:|
| StepWisePopulationGeneration |           100 |  48.56 us | 0.938 us | 1.081 us | 0.1831 |   9.43 KB |
|    RandomGeneratedPopulation |           100 |  71.33 us | 1.409 us | 1.730 us |      - |    2.4 KB |
| StepWisePopulationGeneration |           200 |  96.39 us | 1.302 us | 1.017 us | 0.3662 |   18.8 KB |
|    RandomGeneratedPopulation |           200 | 144.05 us | 2.816 us | 2.766 us |      - |   4.74 KB |

 */

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    public class GenerateInitialPopulationBenchmarks
    {
        private int solutionSize;
        private IGenerateInitPopulationMethod _stepWiseGenerationPopulation;
        private IGenerateInitPopulationMethod _randomGenerationPopulation;

        [Params(100, 200)]
        public int NrOfSolutions { get; set; } = 10;
        
        [GlobalSetup] 
        public void Setup() 
        {
            solutionSize = 10;
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            var instance = qapReader.ReadFileAsync(folderName, fileName).Result;
            
            _stepWiseGenerationPopulation = new StepWisePopulationGeneration(1);
            _stepWiseGenerationPopulation.InitMethod(instance);

            _randomGenerationPopulation = new RandomGeneratedPopulation(42);
            _randomGenerationPopulation.InitMethod(instance);
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

    }
}

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

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    [RPlotExporter]
    public class GenerateInitialPopulationBenchmarks
    {
        private int solutionSize;
        private IGenerateInitPopulationMethod generationMethod;

        [GlobalSetup] 
        public void Setup() 
        {
            solutionSize = 10;
            //generationMethod = new StepWisePopulationGenerationMethod(1, );
        }

        [Benchmark]
        public void GenerateInitialPopulation()
        {
            generationMethod.GeneratePopulation(10);
        }

    }
}

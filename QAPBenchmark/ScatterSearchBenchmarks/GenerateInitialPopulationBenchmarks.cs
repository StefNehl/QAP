using BenchmarkDotNet.Attributes;
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
    [RPlotExporter]
    public class GenerateInitialPopulationBenchmarks
    {
        private int solutionSize;
        private IGenerateInitPopulationMethod generationMethod;

        [GlobalSetup] 
        public void Setup() 
        {
            solutionSize = 10;
            var newInstance = new QAPInstance(10, null, null);
            generationMethod = new StepWisePopulationGenerationMethod(1);
        }

        [Benchmark]
        public void GenerateInitialPopulation()
        {
            generationMethod.GeneratePopulation(10, solutionSize);
        }
    }
}

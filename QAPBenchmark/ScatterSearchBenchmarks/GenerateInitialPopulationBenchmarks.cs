using BenchmarkDotNet.Attributes;
using Domain.Models;
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
        private ScatterSearchStart scatterSearch;

        [GlobalSetup] 
        public void Setup() 
        {
            solutionSize = 10;
            var newInstance = new QAPInstance(10, null, null);
            scatterSearch = new ScatterSearchStart(newInstance);
        }

        [Benchmark]
        public void GenerateInitialPopulation()
        {
            scatterSearch.GenerateInitialPopulation();
        }
    }
}

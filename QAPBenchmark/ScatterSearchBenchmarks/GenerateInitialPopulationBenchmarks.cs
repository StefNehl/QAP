﻿using BenchmarkDotNet.Attributes;
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
|                            Method | SolutionName | NrOfSolutions |          Mean |       Error |      StdDev |        Median |    Gen0 |   Gen1 | Allocated |
|---------------------------------- |------------- |-------------- |--------------:|------------:|------------:|--------------:|--------:|-------:|----------:|
|      StepWisePopulationGeneration |   chr12a.dat |            10 |      4.843 us |   0.0945 us |   0.0928 us |      4.811 us |  0.0153 |      - |    1016 B |
|         RandomGeneratedPopulation |   chr12a.dat |            10 |      7.514 us |   0.1410 us |   0.1319 us |      7.553 us |  0.0153 |      - |    1016 B |
| ParallelRandomGeneratedPopulation |   chr12a.dat |            10 |      8.794 us |   0.1267 us |   0.1185 us |      8.758 us |  0.1068 |      - |    5809 B |
|      StepWisePopulationGeneration |   chr12a.dat |           100 |     49.469 us |   0.9659 us |   0.9035 us |     49.377 us |  0.1831 |      - |    9656 B |
|         RandomGeneratedPopulation |   chr12a.dat |           100 |     74.060 us |   1.2752 us |   1.7023 us |     73.683 us |  0.1221 |      - |    9656 B |
| ParallelRandomGeneratedPopulation |   chr12a.dat |           100 |     76.739 us |   0.9537 us |   0.8921 us |     77.163 us |  0.9766 |      - |   53462 B |
|      StepWisePopulationGeneration |   chr12a.dat |           200 |     98.217 us |   1.9158 us |   2.1294 us |     98.070 us |  0.3662 |      - |   19256 B |
|         RandomGeneratedPopulation |   chr12a.dat |           200 |    147.632 us |   2.9410 us |   3.5011 us |    147.416 us |  0.2441 |      - |   19256 B |
| ParallelRandomGeneratedPopulation |   chr12a.dat |           200 |    148.614 us |   1.9131 us |   1.7895 us |    148.192 us |  1.9531 |      - |  106539 B |
|      StepWisePopulationGeneration |   chr25a.dat |            10 |     14.651 us |   0.2783 us |   0.2603 us |     14.728 us |  0.0305 |      - |    1576 B |
|         RandomGeneratedPopulation |   chr25a.dat |            10 |     20.169 us |   0.3886 us |   0.4915 us |     20.090 us |  0.0305 |      - |    1576 B |
| ParallelRandomGeneratedPopulation |   chr25a.dat |            10 |     20.047 us |   0.3176 us |   0.2971 us |     20.045 us |  0.1526 |      - |    7951 B |
|      StepWisePopulationGeneration |   chr25a.dat |           100 |    146.729 us |   2.8841 us |   3.7502 us |    147.126 us |  0.2441 |      - |   15256 B |
|         RandomGeneratedPopulation |   chr25a.dat |           100 |    206.422 us |   4.0633 us |   6.6761 us |    205.858 us |  0.2441 |      - |   15256 B |
| ParallelRandomGeneratedPopulation |   chr25a.dat |           100 |    133.706 us |   0.1667 us |   0.1478 us |    133.663 us |  1.4648 |      - |   74297 B |
|      StepWisePopulationGeneration |   chr25a.dat |           200 |    290.482 us |   2.8887 us |   2.5607 us |    290.916 us |  0.4883 |      - |   30456 B |
|         RandomGeneratedPopulation |   chr25a.dat |           200 |    397.811 us |   4.5838 us |   4.0634 us |    396.668 us |  0.4883 |      - |   30456 B |
| ParallelRandomGeneratedPopulation |   chr25a.dat |           200 |    250.008 us |   0.6600 us |   0.6174 us |    249.835 us |  2.9297 |      - |  148140 B |
|      StepWisePopulationGeneration |  tai256c.dat |            10 |  1,032.983 us |  12.2610 us |  10.8691 us |  1,029.769 us |       - |      - |   10778 B |
|         RandomGeneratedPopulation |  tai256c.dat |            10 |  1,120.071 us |   9.0009 us |   8.4194 us |  1,114.568 us |       - |      - |   10778 B |
| ParallelRandomGeneratedPopulation |  tai256c.dat |            10 |    283.161 us |   0.3709 us |   0.3288 us |    283.108 us |  0.4883 |      - |   35793 B |
|      StepWisePopulationGeneration |  tai256c.dat |           100 | 10,149.696 us |  47.3119 us |  44.2556 us | 10,118.733 us |       - |      - |  107274 B |
|         RandomGeneratedPopulation |  tai256c.dat |           100 | 11,163.472 us | 102.3644 us |  95.7518 us | 11,087.425 us |       - |      - |  107274 B |
| ParallelRandomGeneratedPopulation |  tai256c.dat |           100 |  1,717.496 us |   1.9562 us |   1.5272 us |  1,717.831 us |  5.8594 |      - |  352706 B |
|      StepWisePopulationGeneration |  tai256c.dat |           200 | 20,376.865 us | 120.7136 us | 112.9156 us | 20,457.331 us |       - |      - |  214491 B |
|         RandomGeneratedPopulation |  tai256c.dat |           200 | 22,199.417 us | 212.4541 us | 198.7297 us | 22,084.953 us |       - |      - |  214493 B |
| ParallelRandomGeneratedPopulation |  tai256c.dat |           200 |  3,263.804 us |   9.5618 us |   7.9845 us |  3,261.271 us | 11.7188 | 3.9063 |  704948 B |
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
            _parallelRandomGeneratedPopulation.InitMethod(instance);
            
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

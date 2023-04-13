using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

/*
|                   Method | NrOfCalls |       Mean |    Error |   StdDev |    Gen0 |    Gen1 |  Allocated |
|------------------------- |---------- |-----------:|---------:|---------:|--------:|--------:|-----------:|
|  SubSetGen_Typ1_Parallel |       100 | 2,007.1 us |  4.58 us |  3.82 us | 39.0625 | 15.6250 | 1958.28 KB |
|           SubSetGen_Typ1 |       100 |   551.5 us | 10.95 us | 11.25 us | 11.7188 |       - |   612.5 KB |
| SubSetGen_Type2_Parallel |       100 | 1,626.5 us |  3.61 us |  3.01 us | 33.2031 | 31.2500 | 1628.02 KB |
|           SubSetGen_Typ2 |       100 |   500.7 us |  9.62 us | 10.29 us |  9.7656 |       - |  521.09 KB |
| SubSetGen_Type3_Parallel |       100 | 1,284.9 us |  3.11 us |  2.43 us | 25.3906 | 23.4375 | 1273.02 KB |
|           SubSetGen_Typ3 |       100 |   467.0 us |  6.43 us |  6.02 us |  8.3008 |       - |  430.47 KB |
| SubSetGen_Type4_Parallel |       100 |   129.7 us |  2.45 us |  2.17 us |  1.9531 |       - |   98.44 KB |
|           SubSetGen_Typ4 |       100 |   125.2 us |  1.27 us |  1.19 us |  1.4648 |       - |   72.66 KB |
|  SubSetGen_Typ1_Parallel |       200 | 3,975.4 us | 12.94 us | 10.11 us | 78.1250 | 31.2500 | 3916.42 KB |
|           SubSetGen_Typ1 |       200 | 1,055.8 us | 14.87 us | 13.18 us | 23.4375 |       - |    1225 KB |
| SubSetGen_Type2_Parallel |       200 | 3,216.0 us |  9.15 us |  8.56 us | 66.4063 | 62.5000 | 3256.03 KB |
|           SubSetGen_Typ2 |       200 | 1,046.7 us | 17.35 us | 16.23 us | 19.5313 |       - | 1042.19 KB |
| SubSetGen_Type3_Parallel |       200 | 2,623.5 us | 32.98 us | 30.84 us | 50.7813 | 46.8750 | 2545.96 KB |
|           SubSetGen_Typ3 |       200 |   955.5 us |  4.54 us |  4.02 us | 16.6016 |       - |  860.94 KB |
| SubSetGen_Type4_Parallel |       200 |   264.0 us |  5.27 us |  4.93 us |  3.9063 |       - |  196.88 KB |
|           SubSetGen_Typ4 |       200 |   241.0 us |  3.85 us |  3.41 us |  2.9297 |       - |  145.31 KB |

 */

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    public class SubSetGenerationBenchmark
    {
        private ParallelSubSetGeneration _parallelSubSetGeneration;
        private SubSetGeneration _subSetGeneration;

        private IImprovementMethod improvementMethod;
        private ICombinationMethod combinationMethod;

        private List<InstanceSolution> referenceList;

        [Params(100, 200)] 
        public int NrOfCalls { get; set; } = 10;

        [GlobalSetup]
        public void Setup()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            var instance = qapReader.ReadFileAsync(folderName, fileName).Result;

            improvementMethod = new ImprovedLocalSearchFirstImprovement();
            improvementMethod.InitMethod(instance);
            combinationMethod = new ExhaustingPairwiseCombination();
            combinationMethod.InitMethod(instance);
            
            var generationMethod = new StepWisePopulationGeneration(2);
            generationMethod.InitMethod(instance);
            referenceList = generationMethod.GeneratePopulation(10);

            _parallelSubSetGeneration = new ParallelSubSetGeneration( 1, SubSetGenerationMethodType.Cycle, combinationMethod, improvementMethod);
            _parallelSubSetGeneration.InitMethod(instance);
            
            _subSetGeneration = new SubSetGeneration( 1, SubSetGenerationMethodType.Cycle, combinationMethod, improvementMethod);
            _subSetGeneration.InitMethod(instance);
        }

        [Benchmark]
        public void SubSetGen_Parallel()
        {
            for(int i = 0; i < NrOfCalls; i++)
            {
                _parallelSubSetGeneration.GetSolutions(referenceList);
            }
        }

        [Benchmark]
        public void SubSetGen()
        {
            for (int i = 0; i < NrOfCalls; i++)
            {
                _subSetGeneration.GenerateType1SubSet(referenceList);
            }
        }
        
    }
}

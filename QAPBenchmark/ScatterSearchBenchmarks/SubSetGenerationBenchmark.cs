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
|             Method | NrOfCalls |       Mean |    Error |   StdDev |    Gen0 |    Gen1 | Allocated |
|------------------- |---------- |-----------:|---------:|---------:|--------:|--------:|----------:|
| SubSetGen_Parallel |       100 | 1,234.8 us |  9.24 us |  8.64 us | 27.3438 | 11.7188 |   1.35 MB |
|          SubSetGen |       100 |   572.2 us |  9.52 us |  9.35 us | 22.4609 |       - |    1.1 MB |
| SubSetGen_Parallel |       200 | 2,482.7 us | 21.79 us | 20.38 us | 54.6875 | 23.4375 |   2.69 MB |
|          SubSetGen |       200 | 1,179.4 us | 20.19 us | 18.89 us | 44.9219 |       - |   2.21 MB |

 */

namespace QAPBenchmark.ScatterSearchBenchmarks
{
    [MemoryDiagnoser]
    public class SubSetGenerationBenchmark
    {
        private ParallelSubSetGeneration _parallelSubSetGeneration;
        private SubSetGeneration _subSetGeneration;

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

            var improvementMethod = new ImprovedLocalSearchFirstImprovement();
            improvementMethod.InitMethod(instance);
            var combinationMethod = new ExhaustingPairwiseCombination();
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

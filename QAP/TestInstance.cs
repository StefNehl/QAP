using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace QAP
{
    public static class TestInstance
    {
        public static TestResult StartTest(QAPInstance instance, 
            int populationSize, 
            int referenceSetSize, 
            int runTimeInSeconds, 
            int subSetGenerationTypes,
            SubSetGenerationMethodType subSetGenerationMethodType,
            ICombinationMethod combinationMethod,
            IGenerateInitPopulationMethod generateInitPopulationMethod,
            IImprovementMethod improvementMethod,
            IDiversificationMethod diversificationMethod,
            bool displayProgressInConsole = false)
        {
            var scatterSearch = new ScatterSearchStart(instance, generateInitPopulationMethod, diversificationMethod, combinationMethod, improvementMethod, populationSize, referenceSetSize);
            var result = scatterSearch.Solve(runTimeInSeconds, subSetGenerationTypes, subSetGenerationMethodType, displayProgressInConsole);

            var newTestResult = new TestResult(
                instance.InstanceName, 
                instance.N, 
                result.Item1.SolutionValue, 
                result.Item3, 
                result.Item2,
                result.Item1.SolutionPermutation, 
                combinationMethod.GetType().Name, 
                generateInitPopulationMethod.GetType().Name, 
                improvementMethod.GetType().Name);
            return newTestResult;
        }

        public static async Task<TestResult> StartTestAsync(QAPInstance instance,
            int populationSize,
            int referenceSetSize,
            int runTimeInSeconds,
            int subSetGenerationTypes,
            SubSetGenerationMethodType subSetGenerationMethodType,
            ICombinationMethod combinationMethod,
            IGenerateInitPopulationMethod generateInitPopulationMethod,
            IImprovementMethod improvementMethod,
            IDiversificationMethod diversificationMethod,
            bool displayProgressInConsole, 
            CancellationToken ct)
        {
            var scatterSearch = new ScatterSearchStart(instance, generateInitPopulationMethod, diversificationMethod, combinationMethod, improvementMethod, populationSize, referenceSetSize);
            var result = await scatterSearch.SolveAsync(runTimeInSeconds, subSetGenerationTypes, subSetGenerationMethodType, displayProgressInConsole, ct);

            var newTestResult = new TestResult(
                instance.InstanceName,
                instance.N,
                result.Item1.SolutionValue,
                result.Item3,
                result.Item2,
                result.Item1.SolutionPermutation,
                combinationMethod.GetType().Name,
                generateInitPopulationMethod.GetType().Name,
                improvementMethod.GetType().Name);
            return newTestResult;
        }
    }
}

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
        public static TestResult StartTest(TestSettings testSettings)
        {
            var scatterSearch = new ScatterSearchStart(
                testSettings.Instance, 
                testSettings.GenerateInitPopulationMethod, 
                testSettings.DiversificationMethod,
                testSettings.CombinationMethod,
                testSettings.ImprovementMethod,
                testSettings.PopulationSize,
                testSettings.ReferenceSetSize);
            var result = scatterSearch.Solve(
                testSettings.RunTimeInSeconds, 
                testSettings.SubSetGenerationTypes,
                testSettings.SubSetGenerationMethodType,
                testSettings.DisplayProgressInConsole);

            var newTestResult = new TestResult(
                testSettings.Instance.InstanceName, 
                testSettings.Instance.N, 
                result.Item1.SolutionValue, 
                result.Item3, 
                result.Item2,
                result.Item1.SolutionPermutation, 
                testSettings.CombinationMethod.GetType().Name, 
                testSettings.GenerateInitPopulationMethod.GetType().Name, 
                testSettings.ImprovementMethod.GetType().Name);
            return newTestResult;
        }

        public static async Task<TestResult> StartTestAsync(TestSettings testSettings, 
            CancellationToken ct)
        {
            var scatterSearch = new ScatterSearchStart(
                testSettings.Instance, 
                testSettings.GenerateInitPopulationMethod, 
                testSettings.DiversificationMethod,
                testSettings.CombinationMethod,
                testSettings.ImprovementMethod,
                testSettings.PopulationSize,
                testSettings.ReferenceSetSize);
            var result = await scatterSearch.SolveAsync(
                testSettings.RunTimeInSeconds, 
                testSettings.SubSetGenerationTypes,
                testSettings.SubSetGenerationMethodType,
                testSettings.DisplayProgressInConsole,
                ct);

            var newTestResult = new TestResult(
                testSettings.Instance.InstanceName, 
                testSettings.Instance.N, 
                result.Item1.SolutionValue, 
                result.Item3, 
                result.Item2,
                result.Item1.SolutionPermutation, 
                testSettings.CombinationMethod.GetType().Name, 
                testSettings.GenerateInitPopulationMethod.GetType().Name, 
                testSettings.ImprovementMethod.GetType().Name);
            return newTestResult;
        }
    }
}

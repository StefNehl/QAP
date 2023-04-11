using QAPAlgorithms.ScatterSearch;

namespace QAP
{
    public static class TestInstance
    {
        public static TestResult StartTest(TestSettings testSettings)
        {
            var scatterSearch = new ScatterSearchStart(
                testSettings.GenerateInitPopulationMethod, 
                testSettings.DiversificationMethod,
                testSettings.CombinationMethod,
                testSettings.ImprovementMethod,
                testSettings.SolutionGenerationMethod,
                testSettings.PopulationSize,
                testSettings.ReferenceSetSize);
            var result = scatterSearch.Solve(
                testSettings.Instance,
                testSettings.RunTimeInSeconds, 
                testSettings.DisplayProgressInConsole);

            var newTestResult = new TestResult(
                testSettings,
                result.Item1.SolutionValue, 
                result.Item3, 
                result.Item2,
                result.Item1.SolutionPermutation);
            return newTestResult;
        }
    }
}

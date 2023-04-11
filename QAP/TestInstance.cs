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
                testSettings.SubSetGenerationTypes,
                testSettings.SubSetGenerationMethodType,
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

        public static async Task<TestResult> StartTestAsync(TestSettings testSettings, 
            CancellationToken ct)
        {
            var scatterSearch = new ScatterSearchStart(
                testSettings.GenerateInitPopulationMethod, 
                testSettings.DiversificationMethod,
                testSettings.CombinationMethod,
                testSettings.ImprovementMethod,
                testSettings.SubSetGenerationTypes,
                testSettings.SubSetGenerationMethodType,
                testSettings.PopulationSize,
                testSettings.ReferenceSetSize);
            var result = await scatterSearch.SolveAsync(
                testSettings.Instance,
                testSettings.RunTimeInSeconds, 
                testSettings.DisplayProgressInConsole,
                ct);

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

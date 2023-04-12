using QAPAlgorithms.ScatterSearch;

namespace QAP
{
    public static class TestInstance
    {
        public static TestResult StartTest(TestSetting testSetting)
        {
            var scatterSearch = new ScatterSearch(
                testSetting.GenerateInitPopulationMethod, 
                testSetting.DiversificationMethod,
                testSetting.CombinationMethod,
                testSetting.ImprovementMethod,
                testSetting.SolutionGenerationMethod,
                testSetting.PopulationSize,
                testSetting.ReferenceSetSize);
            
            var result = scatterSearch.Solve(
                testSetting.Instance,
                testSetting.RunTimeInSeconds, 
                testSetting.DisplayProgressInConsole);

            var newTestResult = new TestResult(
                testSetting,
                result.Item1.SolutionValue, 
                result.Item3, 
                result.Item2,
                result.Item1.SolutionPermutation);
            return newTestResult;
        }
    }
}

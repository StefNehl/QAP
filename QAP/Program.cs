// See https://aka.ms/new-console-template for more information

using Domain.Models;
using QAP;
using QAPAlgorithms.ScatterSearch;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();



var folderName = "QAPLIB";
var instanceName = "chr12a.dat";

var filesInFolder = new List<string>() { instanceName };

var runtimeInSeconds = 60;

filesInFolder = qapReader.GetFilesInFolder(folderName);

var testResults = new List<TestResult>();

for(int i = 0; i < filesInFolder.Count; i++)
{
    var instance = await qapReader.ReadFileAsync(folderName, filesInFolder[i]);

    testResults.Add(GetTestResult(GetInstanceWithFirstImprove(instance), instance));
    testResults.Add(GetTestResult(GetInstanceWithBestImprove(instance), instance));

    Console.WriteLine($"{i + 1} of {filesInFolder.Count} calculated");

}

Console.WriteLine(testResults.First().ToStringColumnNames());
for(int i = 0; i < testResults.Count; i++)
{
    Console.WriteLine(testResults[i].ToString());
}


TestResult GetTestResult(TestInstance testInstance, QAPInstance instance)
{
    return testInstance.StartTest(instance, 20, 10, runtimeInSeconds, 1, SubSetGenerationMethodType.Cycle, true);
}

TestInstance GetInstanceWithFirstImprove(QAPInstance instance)      
{
    var improvementMethod = new LocalSearchFirstImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod();

    return new TestInstance(combinationMethod, generationInitPopMethod, improvementMethod);
}

TestInstance GetInstanceWithBestImprove(QAPInstance instance)
{
    var improvementMethod = new LocalSearchBestImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, false);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod();

    return new TestInstance(combinationMethod, generationInitPopMethod, improvementMethod);
}
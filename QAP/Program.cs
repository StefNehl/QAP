// See https://aka.ms/new-console-template for more information

using Domain.Models;
using QAP;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.DiversificationMethods;
using QAPAlgorithms.ScatterSearch.GenerationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();



var folderName = "QAPLIB";
var chr12a = "chr12a.dat";
var chr25a = "chr25a.dat";
var tho150 = "tho150.dat";

var filesInFolder = new List<string>() 
{
    chr12a,
    //chr25a,
    //tho150
};

var runtimeInSeconds = 60;

//filesInFolder = qapReader.GetFilesInFolder(folderName);

var testResults = new List<TestResult>();
var cancellationTokenSource = new CancellationTokenSource();

for(int i = 0; i < filesInFolder.Count; i++)
{
    var instance = await qapReader.ReadFileAsync(folderName, filesInFolder[i]);

    var testResult = GetTestResult(GetInstanceWithFirstImprove(instance), instance);
    Console.WriteLine(testResult.ToStringForConsole());
    testResults.Add(testResult);

    Console.WriteLine($"{i + 1} of {filesInFolder.Count} calculated");
}

Console.WriteLine(testResults.First().ToStringColumnNames());
for(int i = 0; i < testResults.Count; i++)
{
    Console.WriteLine(testResults[i].ToString());
}


TestResult GetTestResult(TestInstance testInstance, QAPInstance instance)
{
    int refSetSize = 10;
    //17 P_25 P is generally set at max(lOO, 5*refSetSize)
    int populationSetSize = 5 * refSetSize;

    return testInstance.StartTest(instance, populationSetSize, refSetSize, runtimeInSeconds, 1, SubSetGenerationMethodType.Cycle, false);
}

async Task<TestResult> GetTestResultAsync(TestInstance testInstance, QAPInstance instance, CancellationToken ct)
{
    int refSetSize = 10;
    //17 P_25 P is generally set at max(lOO, 5*refSetSize)
    int populationSetSize = 5 * refSetSize; 

    return await testInstance.StartTestAsync(instance, populationSetSize, refSetSize, runtimeInSeconds, 1, SubSetGenerationMethodType.Cycle, false, ct);
}

TestInstance GetInstanceWithFirstImprove(QAPInstance instance)      
{
    var improvementMethod = new LocalSearchFirstImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod(instance, 42);
    var diversificationMethod = new HashCodeDiversificationMethod(instance);

    return new TestInstance(generationInitPopMethod, diversificationMethod, combinationMethod, improvementMethod);
}

TestInstance GetInstanceWithBestImprove(QAPInstance instance)
{
    var improvementMethod = new LocalSearchBestImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, checkIfSolutionsWereAlreadyCombined: true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod(instance);
    var diversificationMethod = new HashCodeDiversificationMethod(instance);

    return new TestInstance(generationInitPopMethod, diversificationMethod, combinationMethod, improvementMethod);
}
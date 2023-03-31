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
int refSetSize = 10;
//17 P_25 P is generally set at max(lOO, 5*refSetSize)
int populationSetSize = 5 * refSetSize;

//filesInFolder = qapReader.GetFilesInFolder(folderName);

var testResults = new List<TestResult>();
var cancellationTokenSource = new CancellationTokenSource();

for(int i = 0; i < filesInFolder.Count; i++)
{
    var instance = await qapReader.ReadFileAsync(folderName, filesInFolder[i]);

    Console.WriteLine("First improvement without parallel");
    var testResult = GetInstanceWithFirstImprovement(instance, refSetSize, populationSetSize, runtimeInSeconds);
    Console.WriteLine(testResult.ToStringForConsole());
    testResults.Add(testResult);
    Console.WriteLine();
    
    Console.WriteLine("Improved First improvement without parallel");
    testResult = GetInstanceWithImprovedFirstImprovement(instance, refSetSize, populationSetSize, runtimeInSeconds);
    Console.WriteLine(testResult.ToStringForConsole());
    testResults.Add(testResult);
    Console.WriteLine();

    Console.WriteLine("Best improvement with parallel");
    testResult = await GetInstanceWithBestImprovement(instance, refSetSize, populationSetSize, runtimeInSeconds);
    Console.WriteLine(testResult.ToStringForConsole());
    testResults.Add(testResult);
    Console.WriteLine();
    
    Console.WriteLine("Improved Best improvement with parallel");
    testResult = await GetInstanceWithImprovedBestImprovement(instance, refSetSize, populationSetSize, runtimeInSeconds);
    Console.WriteLine(testResult.ToStringForConsole());
    testResults.Add(testResult);
    Console.WriteLine();
    
    Console.WriteLine($"{i + 1} of {filesInFolder.Count} calculated");
}

Console.WriteLine(testResults.First().ToStringColumnNames());
for(int i = 0; i < testResults.Count; i++)
{
    Console.WriteLine(testResults[i].ToString());
}

//await CSVExport.ExportToCSV(testResults, "C:\\");


TestResult GetInstanceWithFirstImprovement(QAPInstance instance, int referenceSetSize, int populationSize, int runTimeInSeconds)      
{
    var improvementMethod = new LocalSearchFirstImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod(instance, populationSize, instance.N, 42);
    var diversificationMethod = new HashCodeDiversificationMethod(instance);

    var testSettings = new TestSettings(
        instance, 
        populationSize, 
        referenceSetSize, 
        runtimeInSeconds, 
        1,
        SubSetGenerationMethodType.Cycle, 
        combinationMethod,
        generationInitPopMethod,
        improvementMethod,
        diversificationMethod);
    
    return TestInstance.StartTest(testSettings);
}

TestResult GetInstanceWithImprovedFirstImprovement(QAPInstance instance, int referenceSetSize, int populationSize, int runTimeInSeconds)      
{
    var improvementMethod = new ImprovedLocalSearchFirstImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod(instance, populationSize, instance.N, 42);
    var diversificationMethod = new HashCodeDiversificationMethod(instance);

    var testSettings = new TestSettings(
        instance, 
        populationSize, 
        referenceSetSize, 
        runtimeInSeconds, 
        1,
        SubSetGenerationMethodType.Cycle, 
        combinationMethod,
        generationInitPopMethod,
        improvementMethod,
        diversificationMethod);
    
    return TestInstance.StartTest(testSettings);
}

async Task<TestResult> GetInstanceWithBestImprovement(QAPInstance instance, int referenceSetSize, int populationSize, int runTimeInSeconds)      
{
    var improvementMethod = new LocalSearchBestImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod(instance, populationSize, instance.N, 42);
    var diversificationMethod = new HashCodeDiversificationMethod(instance);
    
    var testSettings = new TestSettings(
        instance, 
        populationSize, 
        referenceSetSize, 
        runtimeInSeconds, 
        1,
        SubSetGenerationMethodType.Cycle, 
        combinationMethod,
        generationInitPopMethod,
        improvementMethod,
        diversificationMethod);
    return await TestInstance.StartTestAsync(testSettings,
        cancellationTokenSource.Token);
}

async Task<TestResult> GetInstanceWithImprovedBestImprovement(QAPInstance instance, int referenceSetSize, int populationSize, int runTimeInSeconds)      
{
    var improvementMethod = new ImprovedLocalSearchBestImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
    var generationInitPopMethod = new RandomGeneratedPopulationMethod(instance, populationSize, instance.N, 42);
    var diversificationMethod = new HashCodeDiversificationMethod(instance);
    
    var testSettings = new TestSettings(
        instance, 
        populationSize, 
        referenceSetSize, 
        runtimeInSeconds, 
        1,
        SubSetGenerationMethodType.Cycle, 
        combinationMethod,
        generationInitPopMethod,
        improvementMethod,
        diversificationMethod);
    return await TestInstance.StartTestAsync(testSettings,
        cancellationTokenSource.Token);
}

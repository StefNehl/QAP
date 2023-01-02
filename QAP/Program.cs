// See https://aka.ms/new-console-template for more information

using QAP;
using QAPAlgorithms.ScatterSearch;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();

var testResults = new List<TestResult>();


var folderName = "QAPLIB";
//var instanceName = "chr12a.dat";


var runtimeInSeconds = 60;

var filesInFolder = qapReader.GetFilesInFolder(folderName);
foreach(var fileName in filesInFolder)
{
    var instance = await qapReader.ReadFileAsync(folderName, fileName);

    var improvementMethod = new LocalSearchFirstImprovement(instance);
    var combinationMethod = new ExhaustingPairwiseCombination(1, true);
    var generationInitPopMethod = new StepWisePopulationGenerationMethod(1);

    var testInstance = new TestInstance(combinationMethod, generationInitPopMethod, improvementMethod);
    var testResult = testInstance.StartTest(instance, 20, 10, runtimeInSeconds, 1, SubSetGenerationMethodType.Cycle, true);

    Console.WriteLine(testResult.ToStringForConsole());
}




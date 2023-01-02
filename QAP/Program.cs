// See https://aka.ms/new-console-template for more information

using QAP;
using QAPAlgorithms.ScatterSearch;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();

var testResults = new List<TestResult>();


var folderName = "QAPLIB";
var instanceName = "chr12a.dat";
var instance = await qapReader.ReadFileAsync(folderName, instanceName);

var improvementMethod = new LocalSearchFirstImprovement(instance);
var combinationMethod = new ExhaustingPairwiseCombination(1, true);
var generationInitPopMethod = new StepWisePopulationGenerationMethod(1);

var runtimeInSeconds = 10;
var testInstance = new TestInstance(combinationMethod, generationInitPopMethod, improvementMethod);
var testResult = testInstance.StartTest(instance, 40, 10, runtimeInSeconds, 4, SubSetGenerationMethodType.Cycle, true);

Console.WriteLine(testResult.ToStringColumnNames());
Console.WriteLine(testResult.ToString());


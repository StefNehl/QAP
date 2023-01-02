// See https://aka.ms/new-console-template for more information

using Domain;
using QAPAlgorithms.ScatterSearch;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();


var folderName = "QAPLIB";
var instanceName = "chr12a.dat";
var instance = await qapReader.ReadFileAsync(folderName, instanceName);

var improvementMethod = new LocalSearchFirstImprovement(instance);
var combinationMethod = new ExhaustingPairwiseCombination(1, true);
var generationInitPopMethod = new StepWisePopulationGenerationMethod(1);

var newScatterSearch = new ScatterSearchStart(instance, improvementMethod, combinationMethod, generationInitPopMethod);
var runtimeInSeconds = 30;
var resultTuple = newScatterSearch.Solve(runtimeInSeconds, 1, SubSetGenerationMethodType.Cycle, true);

Console.WriteLine();
Console.WriteLine($"QAP Instance solved after {resultTuple.Item2} iterations (Runtime: {runtimeInSeconds} [s])");
resultTuple.Item1.DisplayInConsole();


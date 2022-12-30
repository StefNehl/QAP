// See https://aka.ms/new-console-template for more information

using Domain;
using QAPAlgorithms.ScatterSearch;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();

Console.WriteLine("Folders:");
foreach(var folder in qapReader.Folders)
    Console.WriteLine(folder);
Console.WriteLine();

string folderName = "Small";
Console.WriteLine($"Files in Folder {folderName}:");
var files = qapReader.GetFilesInFolder(folderName);
foreach(var file in files)
    Console.WriteLine(file);
Console.WriteLine();

string instanceName = "TestN2.dat";
Console.WriteLine($"Values in {instanceName}:");
var instance = await qapReader.ReadFileAsync(folderName, instanceName);
if (instance == null)
    return;

Console.WriteLine(instance);

var testPermutation = new int[] { 0, 1 };
var result = InstanceHelpers.GetSolutionValue(instance, testPermutation);

Console.WriteLine("Result: " + result);
Console.WriteLine();

folderName = "QAPLIB";
instanceName = "chr12a.dat";

instance = await qapReader.ReadFileAsync(folderName, instanceName);

var improvementMethod = new LocalSearchFirstImprovement(instance);
var combinationMethod = new ExhaustingPairwiseCombination(1);

var newScatterSearch = new ScatterSearchStart(instance, improvementMethod, combinationMethod);
var maxIterations = 10000;
var resultTuple = newScatterSearch.Solve(maxIterations);

Console.WriteLine();
Console.WriteLine($"QAP Instance solved after {resultTuple.Item2} iterations (Maxiterations: {maxIterations})");
resultTuple.Item1.DisplayInConsole();


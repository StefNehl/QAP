﻿using Domain.Models;
using QAP;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.DiversificationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();

var filesWithKnownOptimum = new List<Tuple<string, string>>
{
    new ("QAPLIB","chr12a.dat"),
    new ("QAPLIB","chr15b.dat"),
    new ("QAPLIB","chr25a.dat"),
    new ("QAPLIB","esc16b.dat"),
    new ("QAPLIB","esc32c.dat"),
    new ("QAPLIB","esc128.dat"),
    new ("QAPLIB","nug24.dat"),
    new ("QAPLIB","nug30.dat"),
    new ("QAPLIB","sko64.dat"),
    new ("QAPLIB","tai256c.dat"),
};

//new ("QAPLIBNoOptimum", "sko42.dat")

const int runtimeInSeconds = 60;

//17 P_25 P is generally set at max(lOO, 5*refSetSize)
const int refSetSize = 10;
const int populationSetSize = 5 * refSetSize;

var testResults = new List<TestResult>();

for(int i = 0; i < filesWithKnownOptimum.Count; i++)
{
    var instance = await qapReader.ReadFileAsync(filesWithKnownOptimum[i].Item1, filesWithKnownOptimum[i].Item2);
    var testSettingsProvider = new TestSettingsProvider(instance, refSetSize, populationSetSize, runtimeInSeconds);

    foreach (var testSetting in testSettingsProvider.GetTestSettings())
    {
        Console.WriteLine($"Start {i + 1} of {filesWithKnownOptimum.Count}.");
        var testResult = TestInstance.StartTest(testSetting);
        Console.WriteLine(testResult.ToStringForConsole());
        testResults.Add(testResult);
        Console.WriteLine();
        Console.WriteLine($"{i + 1} of {filesWithKnownOptimum.Count} calculated");
        Console.WriteLine();
    }
}

Console.WriteLine(testResults.First().ToStringColumnNames());
foreach (var t in testResults)
    Console.WriteLine(t.ToString());

await CSVExport.ExportToCSV(testResults, @"C:\Master_Results", DateTime.Now.ToString("hh-mm-ss_dd-mm-yyyy"));


// See https://aka.ms/new-console-template for more information

using Domain;

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

var firstPermutation = new int[] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
var secondPermutation = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

instance = await qapReader.ReadFileAsync(folderName, instanceName);

Console.WriteLine($"First: {InstanceHelpers.GetSolutionValue(instance, firstPermutation)}");
Console.WriteLine($"Second: {InstanceHelpers.GetSolutionValue(instance, secondPermutation)}");


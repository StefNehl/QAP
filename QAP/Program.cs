// See https://aka.ms/new-console-template for more information

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
var result = instance.GetInstanceValue(testPermutation);

Console.WriteLine("Result: " + result);

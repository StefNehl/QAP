// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");



var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();

foreach(var folder in qapReader.Folders)
    Console.WriteLine(folder);
Console.WriteLine();

string folderName = "QAPLIB";
var files = qapReader.GetFilesInFolder(folderName);
foreach(var file in files)
    Console.WriteLine(file);
Console.WriteLine();

string instanceName = "chr12a.dat";
var instance = await qapReader.ReadFileAsync(folderName, instanceName);
Console.WriteLine(instance);
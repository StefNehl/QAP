// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

string instanceName = "chr12a.dat";

var fileReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
var instance = await fileReader.ReadFileAsync(instanceName);

Console.WriteLine(instance);
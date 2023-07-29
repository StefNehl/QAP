using QAP;

// await TestParallelPopulationGeneration.Test();
// return;

var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();

var filesWithKnownOptimum = new List<TestFiles>
{
    new ("QAPLIB","chr12a.dat", 9552),
    new ("QAPLIB","chr15b.dat", 7990),
    new ("QAPLIB","chr25a.dat", 3796),
    new ("QAPLIB","esc16b.dat", 292),
    new ("QAPLIB","esc32c.dat", 642),
    new ("QAPLIB","esc128.dat", 64),
    new ("QAPLIB","nug24.dat", 3488),
    new ("QAPLIB","nug30.dat", 6124),
    new ("QAPLIB","sko64.dat", 48498),
    new ("QAPLIB","tai256c.dat", 44759294),
};

//new ("QAPLIBNoOptimum", "sko42.dat")

const int runtimeInSeconds = 600;
// const int runtimeInSeconds = 600 * 3;
//17 P_25 P is generally set at max(lOO, 5*refSetSize)
int nrOfRepetitions = 1;
var testResults = new List<TestResult>();

var calculatedRuntime = filesWithKnownOptimum.Count * runtimeInSeconds * nrOfRepetitions * 4; // 4 = nr of tests 
Console.WriteLine("Runtime till: " + DateTime.Now.AddSeconds(calculatedRuntime));

for(int i = 0; i < filesWithKnownOptimum.Count; i++)
{
    int refSetSize = 20;
    int populationSetSize = 5 * refSetSize;
    Console.WriteLine($"Instance: {filesWithKnownOptimum[i].FileName} {i + 1} of {filesWithKnownOptimum.Count}.");
    for (int r = 0; r < nrOfRepetitions; r++)
    {
        Console.WriteLine("Repetition: " + (r + 1) + " of " + nrOfRepetitions);
        var instance = await qapReader.ReadFileAsync(filesWithKnownOptimum[i].FolderName, 
            filesWithKnownOptimum[i].FileName);
        var testSettingsProvider = new TestSettingsProvider(instance, refSetSize, populationSetSize, runtimeInSeconds);

        int nrOfTest = testSettingsProvider.GetTestSettings().Count;
        int testSettingsCount = 0;
        foreach (var testSetting in testSettingsProvider.GetTestSettings())
        {
            Console.WriteLine($"Test {testSettingsCount + 1} of {nrOfTest}.");
            var testResult = TestInstance.StartTest(
                testSetting, 
                false, 
                filesWithKnownOptimum[i].KnownOptimum, 
                true);
            Console.WriteLine(testResult.ToString());
            testResults.Add(testResult);
            Console.WriteLine();
            testSettingsCount++;
        }

        // refSetSize += 10;
        // populationSetSize += 50;
    }
}


Console.WriteLine(testResults.First().ToStringColumnNames());
foreach (var t in testResults)
    Console.WriteLine(t.ToString());

await CSVExport.ExportToCSV(testResults, @"C:\Master_Results", 
    DateTime.Now.ToString("HH-mm-ss_dd-MM-yyyy"));


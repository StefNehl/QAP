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

var filesWithUnknownOptimum = new List<TestFiles>
{
    new("QAPLIBNoOptimum", "sko42.dat", 14934),
    new("QAPLIBNoOptimum", "sko64.dat", 45736),
    new("QAPLIBNoOptimum", "sko90.dat", 108493),
    new("QAPLIBNoOptimum", "sko100c.dat", 139402),
    new("QAPLIBNoOptimum", "tai30a.dat", 1529135),
    new("QAPLIBNoOptimum", "tai50a.dat", 3854359),
    new("QAPLIBNoOptimum", "tai100a.dat", 15824355),
    new("QAPLIBNoOptimum", "tho40.dat", 214218),
    new("QAPLIBNoOptimum", "tho150.dat", 7620628),
    new("QAPLIBNoOptimum", "wil100.dat", 263909),
};

const int runtimeInSeconds = 6;
// const int runtimeInSeconds = 600 * 3;
//17 P_25 P is generally set at max(lOO, 5*refSetSize)
int nrOfRepetitions = 1;
var testResults = new List<TestResult>();

var calculatedRuntime = filesWithUnknownOptimum.Count * runtimeInSeconds * nrOfRepetitions * 4; // 4 = nr of tests 
Console.WriteLine("Runtime till: " + DateTime.Now.AddSeconds(calculatedRuntime));

for(int i = 0; i < filesWithUnknownOptimum.Count; i++)
{
    int refSetSize = 20;
    int populationSetSize = 5 * refSetSize;
    Console.WriteLine($"Instance: {filesWithUnknownOptimum[i].FileName} {i + 1} of {filesWithUnknownOptimum.Count}.");
    for (int r = 0; r < nrOfRepetitions; r++)
    {
        Console.WriteLine("Repetition: " + (r + 1) + " of " + nrOfRepetitions);
        var instance = await qapReader.ReadFileAsync(filesWithUnknownOptimum[i].FolderName, 
            filesWithUnknownOptimum[i].FileName);
        var testSettingsProvider = new TestSettingsProvider(instance, refSetSize, populationSetSize, runtimeInSeconds);

        int nrOfTest = testSettingsProvider.GetTestSettings().Count;
        int testSettingsCount = 0;
        foreach (var testSetting in testSettingsProvider.GetTestSettings())
        {
            Console.WriteLine($"Test {testSettingsCount + 1} of {nrOfTest}.");
            var testResult = TestInstance.StartTest(
                testSetting, 
                false, 
                filesWithUnknownOptimum[i].KnownOptimum, 
                true);
            Console.WriteLine(testResult.ToString());
            testResults.Add(testResult);
            Console.WriteLine();
            testSettingsCount++;
        }
    }
}


Console.WriteLine(testResults.First().ToStringColumnNames());
foreach (var t in testResults)
    Console.WriteLine(t.ToString());

await CSVExport.ExportToCSV(testResults, @"C:\Master_Results", 
    DateTime.Now.ToString("HH-mm-ss_dd-MM-yyyy"));


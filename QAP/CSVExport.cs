using System.Text;

namespace QAP
{
    public class CSVExport
    {
        public static async Task ExportToCSV(IList<TestResult> results, string filePath, string fileName)
        {
            var stringBuilder = new StringBuilder();    

            foreach(var result in results) 
                stringBuilder.AppendLine(result.ToCSVString());

            await File.WriteAllTextAsync(filePath, stringBuilder.ToString());
        }
    }
}

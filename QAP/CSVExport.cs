using System.Text;

namespace QAP
{
    public class CSVExport
    {
        public static async Task ExportToCSV(IList<TestResult> results, string filePath, string fileName)
        {
            var fullPath = filePath + "\\" + fileName + ".csv";
            var stringBuilder = new StringBuilder();    

            foreach(var result in results) 
                stringBuilder.AppendLine(result.ToString());

            await using var streamReader = File.CreateText(fullPath);
            await streamReader.WriteAsync(stringBuilder.ToString());
        }
    }
}

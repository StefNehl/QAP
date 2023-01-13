using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAP
{
    public class CSVExport
    {
        public static async Task ExportToCSV(IList<TestResult> results, string filePath)
        {
            var stringBuilder = new StringBuilder();    

            foreach(var result in results) 
                stringBuilder.AppendLine(result.ToCSVString());

            await File.WriteAllTextAsync(filePath, stringBuilder.ToString());
        }
    }
}

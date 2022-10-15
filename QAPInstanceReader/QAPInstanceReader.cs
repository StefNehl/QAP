using Domain.Models;
using System.Configuration;
using System.Numerics;

namespace QAPInstanceReader
{
    public class QAPInstanceReader
    {
        private static QAPInstanceReader fileReader;
        private const string path = "TestInstances\\";
        private string folderPath; 

        private QAPInstanceReader(string instancePath)
        {
            var pathValue = ConfigurationManager.AppSettings["QAPLIB"];
            folderPath = path + pathValue;
        }

        public static QAPInstanceReader GetInstance()
        {
            if(fileReader== null)
                fileReader = new QAPInstanceReader(path);

            return fileReader;
        }

        public async Task<Instance?> ReadFileAsync(string fileName)
        {
            int n = 0;
            int[,] a = new int[n, n];
            int[,] b = new int[n, n];

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + folderPath + "\\" + fileName;
            
            if (!File.Exists(fullPath))
                return null;

            string? line;
            using(StreamReader file = new StreamReader(fullPath))
            {
                int count = 0;

                int nIndex = 0;
                
                int aStartIndex = 0;
                int aEndIndex = 0;
                int aRowCount = 0;

                int bStartIndex = 0;
                int bEndIndex = 0;
                int bRowCount = 0;

                while(true)
                {
                    line = await file.ReadLineAsync();
                    if(line == null) 
                        break;

                    if(count == 0)
                    {
                        n = int.Parse(line);
                        a = new int[n, n];
                        b = new int[n, n];

                        aStartIndex = nIndex + 2;
                        aEndIndex = aStartIndex + n;

                        bStartIndex = aEndIndex + 1;
                        bEndIndex = bStartIndex + n;    
                    }

                    if(count >= aStartIndex && count <= aEndIndex)
                    {
                        var stringValuesArray = line.Split(" ");
                        parseStringValuesAndInsertInIntMatrix(stringValuesArray, a, aRowCount);
                        aRowCount++;
                    }

                    if (count >= bStartIndex && count <= bEndIndex)
                    {
                        var stringValuesArray = line.Split(" ");
                        parseStringValuesAndInsertInIntMatrix(stringValuesArray, b, bRowCount);
                        bRowCount++;
                    }

                    count++;
                    Console.WriteLine(line);
                }
            }

            return new Instance(n, a, b);
        }

        private void parseStringValuesAndInsertInIntMatrix(string[]? stringValues, int[,] matrix, int rowIndex)
        {
            if (stringValues == null)
                return;

            int columnIndex = 0;
            for(int i = 0; i < stringValues.Length; i++)
            {
                var stringValue = stringValues[i];
                if (string.IsNullOrWhiteSpace(stringValue)) 
                    continue;

                matrix[rowIndex, columnIndex] = int.Parse(stringValue);
                columnIndex++;
            }
        }
    }
}
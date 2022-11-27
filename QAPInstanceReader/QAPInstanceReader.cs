using Domain.Models;
using System.Configuration;
using System.Numerics;

namespace QAPInstanceReader
{
    public class QAPInstanceReader
    {
        private static QAPInstanceReader fileReader;
        private const string path = "TestInstances";

        private QAPInstanceReader()
        {
            Folders = new();
            Folders.Add("QAPLIB");
            Folders.Add("QAPLIBOptimum");

        }

    public static QAPInstanceReader GetInstance()
        {
            if(fileReader== null)
                fileReader = new QAPInstanceReader();

            return fileReader;
        }

        public List<string> Folders 
        {
            get; private set;
        }

        public string[] GetFilesInFolder(string folder)
        {
            var files = Directory.GetFiles(GetFolderPath(folder));

            for(int i = 0; i < files.Length; i++) 
            {
                var fullFilePathArray = files[i].Split("\\");
                files[i] = fullFilePathArray.Last();
            }

            return files;
        }

        private string GetFolderPath(string folder)
        {
            string folderPath = path + "\\" + folder;
            string fullFolderPath = AppDomain.CurrentDomain.BaseDirectory + folderPath + "\\";
            return fullFolderPath;
        }

        public async Task<Instance?> ReadFileAsync(string folder, string fileName)
        {
            int n = 0;
            int[,] a = new int[n, n];
            int[,] b = new int[n, n];

            var fullPath = GetFolderPath(folder) + fileName;
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
                        ParseStringValuesAndInsertInIntMatrix(stringValuesArray, a, aRowCount);
                        aRowCount++;
                    }

                    if (count >= bStartIndex && count <= bEndIndex)
                    {
                        var stringValuesArray = line.Split(" ");
                        ParseStringValuesAndInsertInIntMatrix(stringValuesArray, b, bRowCount);
                        bRowCount++;
                    }
                    count++;
                }
            }

            return new Instance(n, a, b);
        }

        private void ParseStringValuesAndInsertInIntMatrix(string[]? stringValues, int[,] matrix, int rowIndex)
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record QAPInstance(int N, int[,] A, int[,] B)
    {
        public override string ToString()
        {
            string instanceString = "N: " + N + Environment.NewLine;

            instanceString += GetMatrixString(A);
            instanceString += Environment.NewLine;

            instanceString += GetMatrixString(B);

            return instanceString;
        }

        private static string GetMatrixString(int[,] matrix)
        {
            var matrixString = string.Empty;

            var n = matrix.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                string line = matrix[i, 0] + "";

                for (int j = 1; j < n; j++)
                {
                    line += "; " + matrix[i, j];
                }
                line += Environment.NewLine;
                matrixString += line;
            }

            return matrixString;
        }


    }
}

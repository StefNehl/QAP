using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest
{
    public class QAPInstanceProvider
    {
        public static async Task<QAPInstance> GetChr12a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return await qapReader.ReadFileAsync(folderName, fileName);
        }

        public static int[] GetOptimalPermutationForChr12a()
        {
            var optimalPermutation =  new int[] { 7, 5, 12, 2, 1, 3, 9, 11, 10, 6, 8, 4 };
            ReduceIndexOfPermutation(optimalPermutation);
            return optimalPermutation;
        }

        public static long GetOptimalSolutionValueForChr12a()
        {
            return 9552;
        }

        public static async Task<QAPInstance> GetChr12b()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12b.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return await qapReader.ReadFileAsync(folderName, fileName);
        }

        public static int[] GetOptimalPermutationForChr12b()
        {
            var optimalPermutation = new int[] { 5, 7, 1, 10, 11, 3, 4, 2, 9, 6, 12, 8 };
            ReduceIndexOfPermutation(optimalPermutation);
            return optimalPermutation;
        }

        public static long GetOptimalSolutionValueForChr12b()
        {
            return 9742;
        }

        public static async Task<QAPInstance> GetChr25a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr25a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return await qapReader.ReadFileAsync(folderName, fileName);
        }

        public static int[] GetOptimalPermutationForChr25a()
        {
            var optimalPermutation = new int[] { 25, 12, 5, 3, 18, 4, 16, 8, 20, 10, 14, 6, 15, 23, 24, 19, 13, 1, 21, 11, 17, 2, 22, 7, 9 };
            ReduceIndexOfPermutation(optimalPermutation);
            return optimalPermutation;
        }

        public static long GetOptimalSolutionValueForChr25a()
        {
            return 3796;
        }


        public static async Task<QAPInstance> GetTestN3()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return qapReader.ReadFileAsync("Small", "TestN3.dat").Result;
        }
        
        public static async Task<QAPInstance> GetTestN4()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return qapReader.ReadFileAsync("Small", "TestN4.dat").Result;
        }
        
        public static async Task<QAPInstance> GetTestN5()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            return qapReader.ReadFileAsync("Small", "TestN5.dat").Result;
        }

        private static void ReduceIndexOfPermutation(int[] permutation)
        {
            for (int i = 0; i < permutation.Length; i++)
                permutation[i] = permutation[i] - 1;
        }
    }
}

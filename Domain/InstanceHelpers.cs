using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class InstanceHelpers
    {
        public static long GetSolutionValue(QAPInstance instance, int[] solution)
        {
            return GetSolutionValue(instance, solution, 0, instance.N - 1);
        }

        public static long GetSolutionValue(QAPInstance instance, int[] solution, int startIndex, int endIndex)
        {
            if ((endIndex+1) > instance.N)
                return long.MaxValue;

            //ToDo
            //Improve new calculation of the Value Erenda Cela p.77


            long result = 0;
            for (int i = startIndex; i <= endIndex; i++)
            {
                for (int j = startIndex; j <= endIndex; j++)
                {
                    int resultA = instance.A[i, j];
                    int resultB = instance.B[solution[i], solution[j]];
                    result += resultA * resultB;
                }
            }

            return result;
        }

        //14_Principles of Scatter Search P.6
        public static long GenerateHashCode(int[] permutation)
        {
            long hashCode = 0;
            for (int i = 0; i < permutation.Length; i++)
            {
                hashCode += (i + 1) * (long)Math.Pow((permutation[i] + 1), 2);
            }

            return hashCode;
        }

        public static bool IsEqual(int[] permutation1, int[] permutation2)
        {
            if (GenerateHashCode(permutation1) == GenerateHashCode(permutation2))
                return true;

            return false;
        }

        public static bool IsBetterSolution(long oldSolutionValue, long newSolutionValue)
        {
            if (newSolutionValue < oldSolutionValue)
                return true;
            return false;
        }
    }
}

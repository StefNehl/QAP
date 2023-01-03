﻿using Domain.Models;
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
            //ToDo
            //Improve new calculation of the Value Erenda Cela p.77

            if (solution.Length != instance.N)
                return long.MaxValue;

            long result = 0;
            for (int i = 0; i < instance.N; i++)
            {
                for (int j = 0; j < instance.N; j++)
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

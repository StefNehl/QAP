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
        public static long GetSolutionValue(QAPInstance instance, int[] permutation)
        {
            return GetSolutionValue(instance, permutation, 0, instance.N - 1);
        }

        public static long GetSolutionValue(QAPInstance instance, int[] permutation, int startIndex, int endIndex)
        {
            if ((endIndex+1) > instance.N)
                return long.MaxValue;

            long result = 0;
            for (int i = startIndex; i <= endIndex; i++)
            {
                for (int j = startIndex; j <= endIndex; j++)
                {
                    long resultA = instance.A[i, j];
                    long resultB = instance.B[permutation[i], permutation[j]];
                    result += resultA * resultB;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the difference after a swap. Taken from Rostislav Stanek qap (File O2.hpp)
        /// line 32.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="perm">the current permutation without the swap</param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static long GetSolutionDifferenceAfterSwap(QAPInstance instance, 
            int[] perm,
            int i,
            int j)
        {
            long delta = 0;

            for (var k = 0; k < perm.Length; k++)
            {
                delta -= instance.A[i, k] * instance.B[perm[i], perm[k]];
                delta -= instance.A[j, k] * instance.B[perm[j], perm[k]];
                
                delta += instance.A[i, k] * instance.B[perm[j], perm[k]];
                delta += instance.A[j, k] * instance.B[perm[i], perm[k]];
            }
            delta += (instance.A[i, j] * 
                         instance.B[perm[i], perm[j]]) * 2;
            return delta * 2 ;
        }

        public static long GetSolutionValueOp(QAPInstance instance, int[] permutation)
        {
            var permutationSpan = permutation.AsSpan();
            var n = instance.N;
            
            long result = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int resultA = instance.A[i, j];
                    int resultB = instance.B[permutationSpan[i], permutationSpan[j]];
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

        public static bool IsValueAlreadyInThePermutation(int newValue, int[] permutation)
        {
            return IsValueAlreadyInThePermutation(newValue, permutation, 0, permutation.Length-1);
        }

        public static bool IsValueAlreadyInThePermutation(int newValue, int[] permutation, int startIndexToLook, int endIndexToLook)
        {
            for (int i = startIndexToLook; i <= endIndexToLook; i++)
            {
                if (permutation[i] == newValue)
                    return true;
            }

            return false;
        }

        public static int GetIndexOfWorstPart(int[] permutation, int sizeOfPart, QAPInstance qAPInstance)
        {
            if (sizeOfPart < 2)
                throw new Exception("Size of Part can't be smaller than 2");
            var worstIndex = -1;
            long worstSolutionValue = 0;

            for (int i = 0; i <= permutation.Length - sizeOfPart; i++)
            {
                var endIndex = (i + sizeOfPart) - 1;
                var solutionValue = GetSolutionValue(qAPInstance, permutation, i, endIndex);
                if (!IsBetterSolution(worstSolutionValue, solutionValue))
                {
                    worstSolutionValue = solutionValue;
                    worstIndex = i;
                }
            }
            return worstIndex;
        }

        public static int GetIndexOfBestPart(int[] permutation, int sizeOfPart, QAPInstance qAPInstance)
        {
            if (sizeOfPart < 2)
                throw new Exception("Size of Part can't be smaller than 2");
            var worstIndex = -1;
            long worstSolutionValue = long.MaxValue;

            for (int i = 0; i <= permutation.Length - sizeOfPart; i++)
            {
                var endIndex = (i + sizeOfPart) - 1;
                var solutionValue = GetSolutionValue(qAPInstance, permutation, i, endIndex);
                if (IsBetterSolution(worstSolutionValue, solutionValue))
                {
                    worstSolutionValue = solutionValue;
                    worstIndex = i;
                }
            }
            return worstIndex;
        }
    }
}

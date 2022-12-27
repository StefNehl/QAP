using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch
{
    public static class SubSetGenerationMethod
    {
        /// <summary>
        /// Returns an array of new 2-elment with pairwise combination solution of the reference list. 
        /// 
        /// If the array can not be filled up every position with a new solution the position stays null
        /// 18_P.27
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public static InstanceSolution[] GenerateType1SubSet(List<InstanceSolution> referenceSolutions, 
            int sizeOfSubSet)
        {
            var result = new InstanceSolution[sizeOfSubSet];

            for(int i = 0; i < referenceSolutions.Count - 1; i++)
            {
                var firstSolution = referenceSolutions[i];
                for(int j = i+1; i < referenceSolutions.Count; j++)
                {
                    var secondSolution = referenceSolutions[j];
                    CombineSolutionsPairWise(new List<IInstanceSolution> { firstSolution, secondSolution });
                }
            }

            return result;
        }

        /// <summary>
        /// Combines the given solutions pair wise with each other. This happens by filling a
        /// new created solution with pairs until the size of a solution is reached. If not enough 
        /// pairs are available, the function takes the next pair from the first solution
        /// </summary>
        /// <param name="solutions"></param>
        /// <param name="stepSizeForPairs"></param>
        /// <returns></returns>
        public static List<int[]> CombineSolutionsPairWise(List<IInstanceSolution> solutions, 
            int stepSizeForPairs = 1)
        {
            List<int[]> newSolutions = new List<int[]>();
            var solutionPairs = new List<int[]>();

            var solutionLenght = solutions[0].SolutionPermutation.Length;
            var nrOfPairsPerSolution = (int)Math.Ceiling(solutionLenght / (decimal)stepSizeForPairs);

            foreach (var instanceSolution in solutions)
            {
                var solution = instanceSolution.SolutionPermutation;
                for (int i = 0; i < nrOfPairsPerSolution; i += stepSizeForPairs)
                {
                    var newSolutionPair = new int[2];

                    newSolutionPair[0] = solution[i];

                    var nextIndex = i + 1;
                    if (nextIndex == solution.Length)
                    {
                        nextIndex = 0;
                    }
                    newSolutionPair[1] = solution[nextIndex];

                    if(!IsPairAlreadyInList(newSolutionPair, solutionPairs))
                        solutionPairs.Add(newSolutionPair);
                }
            }

            foreach(var pair in solutionPairs)
            {
                var newSolution = new int[solutionLenght];
                for (int i = 0; i < newSolution.Length; i++)
                    newSolution[i] = -1;
                
                newSolution[0] = pair[0];
                newSolution[1] = pair[1];

                int pairCounter = 1;
                var newIndex = 0 + (pairCounter * 2);

                foreach (var nextPair in solutionPairs)
                {

                    if (!IsPairFeasible(nextPair, newSolution))
                        continue;

                    newSolution[newIndex] = nextPair[0];
                    newSolution[newIndex + 1] = nextPair[1];

                    pairCounter++;
                    newIndex = 0 + (pairCounter * 2);

                    if (newIndex == solutionLenght)
                    {
                        if(!IsSolutionInTheStartSolutionList(newSolution, solutions))
                            newSolutions.Add(newSolution);
                        break;
                    }

                }
            }

            return newSolutions;
        }

        private static bool IsPairAlreadyInList(int[] newPair, List<int[]> listOfPairs)
        {
            foreach(var pair in listOfPairs)
            {
                if (pair[0] == newPair[0] &&
                    pair[1] == newPair[1])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Is good is indicated if the solution is feasible (no dublicated numbers)
        /// </summary>
        /// <param name="newPair"></param>
        /// <param name="currentSolution"></param>
        /// <returns></returns>
        private static bool IsPairFeasible(int[] newPair, 
            int[] currentSolution)
        {
            for(int i = 0; i < currentSolution.Length; i++)
            {
                if (currentSolution[i] == newPair[0] ||
                    currentSolution[i] == newPair[1])
                    return false;
            }

            return true;
        }

        private static bool IsSolutionInTheStartSolutionList(int[] newSolution, 
            List<IInstanceSolution> startSolutions)
        {
            var newSolutionHashCode = InstanceHelpers.GenerateHashCode(newSolution);
            
            foreach (var solution in startSolutions) 
            {
                if(newSolutionHashCode == solution.HashCode)
                    return true;
            }

            return false;
        }

    }
}

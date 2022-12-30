﻿using Domain.Models;
using Domain;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch
{
    /// <summary>
    /// Gets every possible pairs of the given solutions and tries to combine those pairs in every possible way.
    /// </summary>
    public class ExhaustingPairwiseCombination : ICombinationMethod
    {
        private readonly int stepSizeForPairs;

        /// <summary>
        /// Gets every possible pairs of the given solutions and tries to combine those pairs in every possible way.
        /// </summary>
        /// <param name="stepSizeForPairs">this parameter sets the step for the pairs. 
        /// Step size 1 will take every pair => [0, 1, 2, 3] = (0,1)(1,2)(2,3)(3,0)
        /// Step size 2 => [0,1,2,3] = (0,1)(2,3)</param>
        public ExhaustingPairwiseCombination(int stepSizeForPairs = 1)
        {
            this.stepSizeForPairs = stepSizeForPairs;
        }
        /// <summary>
        /// Combines the given solutions pair wise with each other. This happens by filling a
        /// new created solution with pairs until the size of a solution is reached. If not enough 
        /// pairs are available, the function takes the next pair from the first solution
        /// </summary>
        /// <param name="solutions"></param>
        /// <returns></returns>
        public List<int[]> CombineSolutionsPairWise(List<IInstanceSolution> solutions)
        {
            if (stepSizeForPairs > 2)
                throw new Exception("Stepsize higher than 2 is not supported and verified");

            List<int[]> newSolutions = new List<int[]>();
            var solutionPairs = new List<int[]>();

            var solutionLenght = solutions[0].SolutionPermutation.Length;
            var nrOfPairsPerSolution = (int)Math.Ceiling(solutionLenght / (decimal)stepSizeForPairs);

            foreach (var instanceSolution in solutions)
            {
                var solution = instanceSolution.SolutionPermutation;
                var startIndex = 0;
                for (int i = 0; i < nrOfPairsPerSolution; i++)
                {
                    var newSolutionPair = new int[2];

                    newSolutionPair[0] = solution[startIndex];

                    var nextIndex = startIndex + 1;
                    if (nextIndex == solution.Length)
                    {
                        nextIndex = 0;
                    }
                    newSolutionPair[1] = solution[nextIndex];

                    if (!IsPairAlreadyInList(newSolutionPair, solutionPairs))
                        solutionPairs.Add(newSolutionPair);

                    startIndex += stepSizeForPairs;
                }
            }

            foreach (var pair in solutionPairs)
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
                    //Fill last element
                    if ((solutionLenght - newIndex) == 1)
                    {
                        if (!IsNumberAlreadyInTheSolution(nextPair[0], newSolution))
                        {
                            newSolution[newIndex] = nextPair[0];
                            if (!IsSolutionInTheStartSolutionList(newSolution, solutions))
                                newSolutions.Add(newSolution);
                            break;
                        }

                    }

                    if (!IsPairFeasible(nextPair, newSolution))
                        continue;

                    newSolution[newIndex] = nextPair[0];

                    if (newIndex < solutionLenght)
                        newSolution[newIndex + 1] = nextPair[1];

                    pairCounter++;
                    newIndex = 0 + (pairCounter * 2);

                    if (newIndex == solutionLenght)
                    {
                        if (!IsSolutionInTheStartSolutionList(newSolution, solutions))
                            newSolutions.Add(newSolution);
                        break;
                    }

                }
            }

            return newSolutions;
        }

        private static bool IsPairAlreadyInList(int[] newPair, List<int[]> listOfPairs)
        {
            foreach (var pair in listOfPairs)
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
            for (int i = 0; i < currentSolution.Length; i++)
            {
                if (currentSolution[i] == newPair[0] ||
                    currentSolution[i] == newPair[1])
                    return false;
            }

            return true;
        }

        private static bool IsNumberAlreadyInTheSolution(int newNumber, int[] currentSolution)
        {
            for (int i = 0; i < currentSolution.Length; i++)
            {
                if (currentSolution[i] == newNumber)
                    return true;
            }

            return false;
        }

        private static bool IsSolutionInTheStartSolutionList(int[] newSolution,
            List<IInstanceSolution> startSolutions)
        {
            var newSolutionHashCode = InstanceHelpers.GenerateHashCode(newSolution);

            foreach (var solution in startSolutions)
            {
                if (newSolutionHashCode == solution.HashCode)
                    return true;
            }

            return false;
        }
    }
}
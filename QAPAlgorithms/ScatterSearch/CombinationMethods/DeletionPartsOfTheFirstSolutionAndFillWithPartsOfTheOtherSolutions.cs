using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.CombinationMethods
{
    public class DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions : CombinationBase, ICombinationMethod
    {
        private readonly Random randomGenerator;
        private readonly double percentageOfSolutionToDelete;
        private readonly bool deleteWorstPart;
        private readonly int? maxNrOfSolutions;
        private readonly QAPInstance qAPInstance;

        /// <summary>
        /// This combination method generates a new permutation  a part (determined by the percentage value) of the best solution and filles the deleted with parts of the other solutions
        /// </summary>
        /// <param name="deleteWorstPart">True to delete the worst part of the solution. If false the method deletes the parts random</param>
        /// <param name="percentageOfSolutionToDelete">percentage of the solution to delete</param>
        /// <param name="seed">seed for the random generator</param>
        /// <param name="checkIfSolutionsWereAlreadyCombined"></param>
        public DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(
            bool deleteWorstPart,
            double percentageOfSolutionToDelete,
            QAPInstance qAPInstance,
            int? maxNrOfSolutions = null,
            int? seed = null, 
            bool checkIfSolutionsWereAlreadyCombined = true) : base(checkIfSolutionsWereAlreadyCombined) 
        {
            if(seed.HasValue)
                this.randomGenerator= new Random(seed.Value);
            else
                this.randomGenerator= new Random();

            this.deleteWorstPart = deleteWorstPart;
            this.percentageOfSolutionToDelete = percentageOfSolutionToDelete;
            this.maxNrOfSolutions = maxNrOfSolutions;
            this.qAPInstance = qAPInstance;
        }

        public List<int[]> CombineSolutions(List<IInstanceSolution> solutions)
        {
            if (base.WereSolutionsAlreadyCombined(solutions))
                return new List<int[]>();

            return Combine(solutions);
        }

        public List<int[]> CombineSolutionsThreadSafe(List<IInstanceSolution> solutions)
        {
            if (base.WereSolutionsAlreadyCombinedThreadSafe(solutions))
                return new List<int[]>();

            return Combine(solutions);
        }

        private List<int[]> Combine(List<IInstanceSolution> solutions)
        {
            var newSolutions = new List<int[]>();

            int startIndexToDelete;
            var bestSolution = solutions[0];

            var nrOfIndizesToDelete = (int)(qAPInstance.N * (percentageOfSolutionToDelete / 100));

            if (deleteWorstPart)
                startIndexToDelete = InstanceHelpers.GetIndexOfWorstPart(bestSolution.SolutionPermutation, nrOfIndizesToDelete, qAPInstance);
            else
                startIndexToDelete = randomGenerator.Next(0, qAPInstance.N - 1);

            var lastIndexToDelete = startIndexToDelete + nrOfIndizesToDelete - 1;

            var newBasePermutation = new int[bestSolution.SolutionPermutation.Length];
            for (int i = 0; i < newBasePermutation.Length; i++)
            {
                if (i < startIndexToDelete || i > lastIndexToDelete)
                    newBasePermutation[i] = bestSolution.SolutionPermutation[i];
                else
                    newBasePermutation[i] = -1;
            }

            for (int i = 1; i < solutions.Count; i++)
            {
                var solutionForCombination = solutions[i];
                var listOfValuesWithoutCorrectIndex = new List<int>();

                var newPermutation = newBasePermutation.ToArray();

                for (int j = 0; j < newPermutation.Length; j++)
                {
                    if (!InstanceHelpers.IsValueAlreadyInThePermutation(
                        solutionForCombination.SolutionPermutation[j],
                        newPermutation))
                    {
                        newPermutation[j] = solutionForCombination.SolutionPermutation[j];
                        continue;
                    }

                    listOfValuesWithoutCorrectIndex.Add(solutionForCombination.SolutionPermutation[j]);
                }

                for (int j = 0; j < newPermutation.Length; j++)
                {
                    if (newPermutation[j] == -1)
                    {
                        var valueToAdd = listOfValuesWithoutCorrectIndex[0];
                        listOfValuesWithoutCorrectIndex.RemoveAt(0);
                        newPermutation[j] = valueToAdd;
                    }
                }

                var newHashCode = InstanceHelpers.GenerateHashCode(newPermutation);
                if (newHashCode == bestSolution.HashCode ||
                    newHashCode == solutionForCombination.HashCode)
                    continue;

                newSolutions.Add(newPermutation);

                if (maxNrOfSolutions.HasValue &&
                    newSolutions.Count == maxNrOfSolutions.Value)
                    return newSolutions;
            }

            return newSolutions;
        }

    }
}

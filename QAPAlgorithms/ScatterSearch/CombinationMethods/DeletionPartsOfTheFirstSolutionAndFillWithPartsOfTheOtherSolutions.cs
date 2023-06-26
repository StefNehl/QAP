using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.CombinationMethods
{
    public class DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions : CombinationBase, ICombinationMethod
    {
        private readonly Random _randomGenerator;
        private readonly double _percentageOfSolutionToDelete;
        private readonly bool _deleteWorstPart;
        private readonly int? _maxNrOfSolutions;
        
        private QAPInstance _qapInstance;

        /// <summary>
        /// This combination method generates a new permutation  a part (determined by the percentage value) of the best solution and fills the deleted with parts of the other solutions
        /// </summary>
        /// <param name="deleteWorstPart">True to delete the worst part of the solution. If false the method deletes the parts random</param>
        /// <param name="percentageOfSolutionToDelete">percentage of the solution to delete</param>
        /// <param name="maxNrOfSolutions">Maximum number of solutions</param>
        /// <param name="seed">seed for the random generator</param>
        /// <param name="checkIfSolutionsWereAlreadyCombined"></param>
        public DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(
            bool deleteWorstPart,
            double percentageOfSolutionToDelete,
            int? maxNrOfSolutions = null,
            int? seed = null,
            bool checkIfSolutionsWereAlreadyCombined = true) : base(checkIfSolutionsWereAlreadyCombined)
        {
            if(seed.HasValue)
                _randomGenerator= new Random(seed.Value);
            else
                _randomGenerator= new Random();
            
            _deleteWorstPart = deleteWorstPart;
            _percentageOfSolutionToDelete = percentageOfSolutionToDelete;
            _maxNrOfSolutions = maxNrOfSolutions;
        }

        public void InitMethod(QAPInstance instance)
        {
            _qapInstance = instance;
        }

        public List<int[]> CombineSolutions(List<InstanceSolution> solutions)
        {
            if (WereSolutionsAlreadyCombined(solutions))
                return new List<int[]>();

            return Combine(solutions);
        }

        public List<int[]> CombineSolutionsThreadSafe(List<InstanceSolution> solutions)
        {
            if (WereSolutionsAlreadyCombinedThreadSafe(solutions))
                return new List<int[]>();

            return Combine(solutions);
        }

        private List<int[]> Combine(List<InstanceSolution> solutions)
        {
            var newSolutions = new List<int[]>();

            int startIndexToDelete;
            var bestSolution = solutions[0];

            var nrOfIndicesToDelete = (int)(_qapInstance.N * (_percentageOfSolutionToDelete / 100));

            if (_deleteWorstPart)
                startIndexToDelete = InstanceHelpers.GetIndexOfWorstPart(bestSolution.SolutionPermutation, nrOfIndicesToDelete, _qapInstance);
            else
                startIndexToDelete = _randomGenerator.Next(0, _qapInstance.N - 1);

            var lastIndexToDelete = startIndexToDelete + nrOfIndicesToDelete - 1;

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

                var copyOfList = listOfValuesWithoutCorrectIndex.ToArray(); 
                foreach (var valueWithoutCorrectIndex in copyOfList)
                {
                    if (InstanceHelpers.IsValueAlreadyInThePermutation(valueWithoutCorrectIndex, newPermutation))
                        listOfValuesWithoutCorrectIndex.Remove(valueWithoutCorrectIndex);
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
                InstanceHelpers.CheckIfValueIsDoubledInArray(newPermutation.ToList());

                if (_maxNrOfSolutions.HasValue &&
                    newSolutions.Count == _maxNrOfSolutions.Value)
                    return newSolutions;
            }

            return newSolutions;
        }

    }
}

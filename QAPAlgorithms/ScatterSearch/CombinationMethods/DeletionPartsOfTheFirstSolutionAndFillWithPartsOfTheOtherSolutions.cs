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
        private readonly int maxNrOfSolutions;
        private readonly QAPInstance qAPInstance;

        /// <summary>
        /// This combination method deletes a part (determined by the percentage value) of a solution and filles the deleted with parts of the other solutions
        /// </summary>
        /// <param name="deleteWorstPart">True to delete the worst part of the solution. If false the method deletes the part random</param>
        /// <param name="percentageOfSolutionToDelete">percentage of the solution to delete</param>
        /// <param name="seed">seed for the random generator</param>
        /// <param name="checkIfSolutionsWereAlreadyCombined"></param>
        public DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(
            bool deleteWorstPart,
            double percentageOfSolutionToDelete,
            int maxNrOfSolutions,
            QAPInstance qAPInstance,
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

            var newSolutions = new List<int[]>();




            return newSolutions;

        }

        public List<int[]> CombineSolutionsThreadSafe(List<IInstanceSolution> solutions)
        {
            if (base.WereSolutionsAlreadyCombinedThreadSafe(solutions))
                return new List<int[]>();

            throw new NotImplementedException();
        }

        private int GetIndexOfWorstPart(List<int[]> permutation, int nrOfIndixesToDelete)
        {
            return 0;
        }
    }
}

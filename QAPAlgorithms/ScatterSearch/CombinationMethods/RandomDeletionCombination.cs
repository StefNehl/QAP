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
    public class RandomDeletionCombination : ICombinationMethod
    {
        private readonly HashSet<long> alreadyCombinedSolutions;
        private readonly ConcurrentDictionary<long, long> alreadyCombinedSolutionsForAsync;

        public RandomDeletionCombination()
        {
            alreadyCombinedSolutions = new HashSet<long>();
            alreadyCombinedSolutionsForAsync= new ConcurrentDictionary<long, long>();


        }

        public List<int[]> CombineSolutions(List<IInstanceSolution> solutions)
        {
            throw new NotImplementedException();
        }

        public List<int[]> CombineSolutionsThreadSafe(List<IInstanceSolution> solutions)
        {
            throw new NotImplementedException();
        }
    }
}

using Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.CombinationMethods
{
    public class CombinationBase
    {
        private readonly HashSet<long> alreadyCombinedSolutions;
        private readonly ConcurrentDictionary<long, long> alreadyCombinedSolutionsForAsync;
        private readonly bool checkIfSolutionsWereAlreadyCombined;

        public CombinationBase(bool checkIfSolutionsWereAlreadyCombined)
        {
            this.checkIfSolutionsWereAlreadyCombined = checkIfSolutionsWereAlreadyCombined; 
            alreadyCombinedSolutions= new HashSet<long>();
            alreadyCombinedSolutionsForAsync= new ConcurrentDictionary<long, long>();
        }

        public bool WereSolutionsAlreadyCombined(List<IInstanceSolution> solutions)
        {
            if (checkIfSolutionsWereAlreadyCombined)
            {
                var hashCodeOfSolutions = GenerateHashCodeFromCombinedSolutions(solutions);
                if (alreadyCombinedSolutions.Contains(hashCodeOfSolutions))
                    return true;
                alreadyCombinedSolutions.Add(hashCodeOfSolutions);
            }

            return false;
        }

        public bool WereSolutionsAlreadyCombinedThreadSafe(List<IInstanceSolution> solutions)
        {
            if (checkIfSolutionsWereAlreadyCombined)
            {
                var hashCodeOfSolutions = GenerateHashCodeFromCombinedSolutions(solutions);
                if (alreadyCombinedSolutionsForAsync.ContainsKey(hashCodeOfSolutions))
                    return true;
                alreadyCombinedSolutionsForAsync.GetOrAdd(hashCodeOfSolutions, 0);
            }

            return false;
        }

        private long GenerateHashCodeFromCombinedSolutions(List<IInstanceSolution> solutions)
        {
            long newHashCode = 0;
            for (int i = 0; i < solutions.Count; i++)
            {
                newHashCode += (long)Math.Pow(solutions[i].HashCode, i + 1);
            }
            return newHashCode;
        }
    }
}

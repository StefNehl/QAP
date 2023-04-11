using Domain.Models;
using System.Collections.Concurrent;

namespace QAPAlgorithms.ScatterSearch.CombinationMethods
{
    public class CombinationBase
    {
        private readonly HashSet<long> _alreadyCombinedSolutions;
        private readonly ConcurrentDictionary<long, long> _alreadyCombinedSolutionsForAsync;
        private readonly bool _checkIfSolutionsWereAlreadyCombined;

        public CombinationBase(bool checkIfSolutionsWereAlreadyCombined)
        {
            _checkIfSolutionsWereAlreadyCombined = checkIfSolutionsWereAlreadyCombined; 
            _alreadyCombinedSolutions= new HashSet<long>();
            _alreadyCombinedSolutionsForAsync= new ConcurrentDictionary<long, long>();
        }

        public bool WereSolutionsAlreadyCombined(List<InstanceSolution> solutions)
        {
            if (_checkIfSolutionsWereAlreadyCombined)
            {
                var hashCodeOfSolutions = GenerateHashCodeFromCombinedSolutions(solutions);
                if (_alreadyCombinedSolutions.Contains(hashCodeOfSolutions))
                    return true;
                _alreadyCombinedSolutions.Add(hashCodeOfSolutions);
            }

            return false;
        }

        public bool WereSolutionsAlreadyCombinedThreadSafe(List<InstanceSolution> solutions)
        {
            if (_checkIfSolutionsWereAlreadyCombined)
            {
                var hashCodeOfSolutions = GenerateHashCodeFromCombinedSolutions(solutions);
                if (_alreadyCombinedSolutionsForAsync.ContainsKey(hashCodeOfSolutions))
                    return true;
                _alreadyCombinedSolutionsForAsync.GetOrAdd(hashCodeOfSolutions, 0);
            }

            return false;
        }

        private long GenerateHashCodeFromCombinedSolutions(List<InstanceSolution> solutions)
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

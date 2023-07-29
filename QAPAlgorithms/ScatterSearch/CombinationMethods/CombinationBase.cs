using Domain.Models;
using System.Collections.Concurrent;
using System.Numerics;

namespace QAPAlgorithms.ScatterSearch.CombinationMethods
{
    public class CombinationBase
    {
        private readonly HashSet<BigInteger> _alreadyCombinedSolutions;
        private readonly ConcurrentDictionary<BigInteger, long> _alreadyCombinedSolutionsForAsync;
        private long nrOfCombinationsAlreadyDone = 0;
        private readonly bool _checkIfSolutionsWereAlreadyCombined;

        public CombinationBase(bool checkIfSolutionsWereAlreadyCombined)
        {
            _checkIfSolutionsWereAlreadyCombined = checkIfSolutionsWereAlreadyCombined; 
            _alreadyCombinedSolutions= new HashSet<BigInteger>();
            _alreadyCombinedSolutionsForAsync= new ConcurrentDictionary<BigInteger, long>();
        }

        public bool WereSolutionsAlreadyCombined(List<InstanceSolution> solutions,
            bool checkOrderOfTheSolutions)
        {
            if (_checkIfSolutionsWereAlreadyCombined)
            {
                var hashCodeOfSolutions = GenerateHashCodeFromCombinedSolutions(solutions,
                    checkOrderOfTheSolutions);
                if (_alreadyCombinedSolutions.Contains(hashCodeOfSolutions))
                    return true;
                _alreadyCombinedSolutions.Add(hashCodeOfSolutions);
            }

            return false;
        }

        public bool WereSolutionsAlreadyCombinedThreadSafe(List<InstanceSolution> solutions,
            bool checkOrderOfTheSolutions)
        {
            if (_checkIfSolutionsWereAlreadyCombined)
            {
                var hashCodeOfSolutions = GenerateHashCodeFromCombinedSolutions(solutions,
                    checkOrderOfTheSolutions);
                if (_alreadyCombinedSolutionsForAsync.ContainsKey(hashCodeOfSolutions))
                {
                    // nrOfCombinationsAlreadyDone++;
                    // Console.WriteLine("Solutions already combined: " + nrOfCombinationsAlreadyDone);
                    return true;
                }
                _alreadyCombinedSolutionsForAsync.GetOrAdd(hashCodeOfSolutions, 0);
            }

            return false;
        }

        private BigInteger GenerateHashCodeFromCombinedSolutions(
            List<InstanceSolution> solutions,
            bool checkOrderOfTheSolutions)
        {
            BigInteger newHashCode = 0;
            for (int i = 0; i < solutions.Count; i++)
            {
                var multiplier = 1;
                if (checkOrderOfTheSolutions)
                    multiplier = (i + 1);
                newHashCode += (BigInteger)solutions[i].HashCode * multiplier; ;
            }
            return newHashCode;
        }
    }
}

using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.DiversificationMethods
{
    /**
     * Removed a part of the reference set. This part of the reference set has three different parts.
     * First part has the hashcode near the best solutions. The second are solutions lower than the hashcode, the third
     * higher than the hashcode of the best solution. Both parts include solutions as far away as possible from the
     * hashcode of the best solution. 
     */
    public class HashCodeThreePartDiversification : IDiversificationMethod
    {
        private long _averageHashCode;

        public void InitMethod(QAPInstance instance)
        {
            var n = instance.N;
            var permutationWithMaxHashCode = new int[n];
            var permutationWithMinHashCode = new int[n];

            for (int i = 0; i < instance.N; i++)
            {
                permutationWithMaxHashCode[i] = i;
                permutationWithMinHashCode[i] = n - i;
            }
            var minHashCode = InstanceHelpers.GenerateHashCode(permutationWithMinHashCode);
            var maxHashCode = InstanceHelpers.GenerateHashCode(permutationWithMaxHashCode);
            _averageHashCode = (minHashCode + maxHashCode) / 2;
        }
        
        public void ApplyDiversificationMethod(List<InstanceSolution> referenceSet, 
            List<InstanceSolution> population, 
            double percentageOfSolutionToRemove = 0.5)
        {
            int refSetSize = referenceSet.Count;
            int newRefSetSize = (int)(refSetSize * (1 - percentageOfSolutionToRemove));

            while (referenceSet.Count != newRefSetSize)
                referenceSet.RemoveAt(referenceSet.Count - 1);

            List<InstanceSolution> orderedPopulationAfterHashCode = population.OrderBy(s => s.HashCode).ToList();
            var bestSolutionHashcode = referenceSet[0].HashCode;

            var minDiff = long.MaxValue;
            var indexOfMinDiff = 0;
            
            foreach (var instanceSolution in population)
            {
                var hashCodeDiff = Math.Abs(instanceSolution.HashCode - bestSolutionHashcode);
                if (hashCodeDiff < minDiff)
                {
                    indexOfMinDiff = population.IndexOf(instanceSolution);
                    minDiff = hashCodeDiff;
                }
            }

            var partSize = newRefSetSize / 3;
            var diverseSolutions = new List<InstanceSolution>();

            var startEndTuple = GetStartAndEndIndexForPart(indexOfMinDiff, partSize, population.Count-1);
            var startIndex = startEndTuple.Item1;
            var endIndex = startEndTuple.Item2;
            
            
            for (int i = 0; i < partSize; i++)
            {
                diverseSolutions.Add(population[i]);
                diverseSolutions.Add(population[(population.Count-1)-i]);
            }
            for (var i = startIndex; i <= endIndex; i++)
            {
                diverseSolutions.Add(population[i]);
            }

            foreach (var solution in diverseSolutions)
            {
                ScatterSearch.ReferenceSetUpdate(solution, referenceSet, refSetSize);
            }
            
            if(referenceSet.Count == refSetSize)
                return;
            
            // Fill reference set with solutions
            long hashCodeOfReferenceSet = 0;
            for (int i = 0; i < partSize; i++)
            {
                var solution = referenceSet[i];
                hashCodeOfReferenceSet += solution.HashCode;
            }

            var averageRefSetHashCode = hashCodeOfReferenceSet / referenceSet.Count;
            
            if (averageRefSetHashCode < _averageHashCode)
                orderedPopulationAfterHashCode = population.OrderByDescending(s => s.HashCode).ToList();

            foreach (var newSolution in orderedPopulationAfterHashCode)
            {
                if (referenceSet.Count == refSetSize)
                    return;
                ScatterSearch.ReferenceSetUpdate(newSolution, referenceSet, refSetSize);
            }
        }

        public static Tuple<int, int> GetStartAndEndIndexForPart(int midIndex, int partSize, int maxIndex)
        {
            var midPart = partSize / 2;
            
            var startIndex = midIndex - midPart;
            var endIndex = midIndex + midPart;

            if (startIndex < 0)
            {
                endIndex += Math.Abs(startIndex);
                startIndex = 0;
            }

            if (endIndex > maxIndex)
            {
                startIndex -= (maxIndex - startIndex);
                endIndex = maxIndex;
            }

            if (partSize % 2 == 0)
                startIndex++;

            return new Tuple<int, int>(startIndex, endIndex);
        }
    }
}

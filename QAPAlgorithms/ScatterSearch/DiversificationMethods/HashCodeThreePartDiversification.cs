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
            ScatterSearch scatterSearch,
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
                if (Math.Abs(instanceSolution.HashCode - bestSolutionHashcode) < minDiff)
                    indexOfMinDiff = population.IndexOf(instanceSolution);
            }

            var partSize = newRefSetSize / 3;
            

            foreach (var newSolution in orderedPopulationAfterHashCode)
            {
                scatterSearch.ReferenceSetUpdate(newSolution);
            }
        }
    }
}

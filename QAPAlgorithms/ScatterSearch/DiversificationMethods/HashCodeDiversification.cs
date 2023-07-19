﻿using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.DiversificationMethods
{
    public class HashCodeDiversification : IDiversificationMethod
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
            int halfRefSetSize = (int)(refSetSize * (1 - percentageOfSolutionToRemove));

            long hashCodeOfReferenceSet = 0;

            for (int i = 0; i < halfRefSetSize; i++)
            {
                var solution = referenceSet[i];
                hashCodeOfReferenceSet += solution.HashCode;
            }

            var averageRefSetHashCode = hashCodeOfReferenceSet / halfRefSetSize;

            while (referenceSet.Count != halfRefSetSize)
                referenceSet.RemoveAt(referenceSet.Count - 1);

            List<InstanceSolution> orderedPopulationAfterHashCode;

            if (averageRefSetHashCode > _averageHashCode)
                orderedPopulationAfterHashCode = population.OrderBy(s => s.HashCode).ToList();
            else
                orderedPopulationAfterHashCode = population.OrderByDescending(s => s.HashCode).ToList();

            foreach (var newSolution in orderedPopulationAfterHashCode)
            {
                if (referenceSet.Count == refSetSize)
                    return;
                ScatterSearch.ReferenceSetUpdate(newSolution, referenceSet, refSetSize);
            }
        }
    }
}

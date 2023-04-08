using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.DiversificationMethods
{
    public class HashCodeDiversificationMethod : IDiversificationMethod
    {
        private readonly QAPInstance qAPInstance;
        private readonly long minHashCode;
        private readonly long maxHashCode;
        private readonly long averageHashCode;

        public HashCodeDiversificationMethod(QAPInstance qAPInstance)
        {
            this.qAPInstance = qAPInstance;

            var n = qAPInstance.N;
            var permutationWithMaxHashCode = new int[n];
            var permutationWithMinHashCode = new int[n];

            for (int i = 0; i < qAPInstance.N; i++)
            {
                permutationWithMaxHashCode[i] = i;
                permutationWithMinHashCode[i] = n - i;
            }
            minHashCode = InstanceHelpers.GenerateHashCode(permutationWithMinHashCode);
            maxHashCode = InstanceHelpers.GenerateHashCode(permutationWithMaxHashCode);
            averageHashCode = (minHashCode + maxHashCode) / 2;

        }
        public void ApplyDiversificationMethod(List<InstanceSolution> referenceSet, List<InstanceSolution> population, ScatterSearchStart scatterSearchStart)
        {

            int refSetSize = referenceSet.Count;
            int halfRefSetSize = (int)(refSetSize / (double)2);
            int countForAverage = halfRefSetSize;

            long hashCodeOfReferenceSet = 0;

            for (int i = 0; i < countForAverage; i++)
            {
                var solution = referenceSet[i];
                hashCodeOfReferenceSet += solution.HashCode;
            }

            var averageRefSetHashCode = hashCodeOfReferenceSet / countForAverage;
            //Console.WriteLine(averageRefSetHashCode);

            while (referenceSet.Count != halfRefSetSize)
                referenceSet.RemoveAt(referenceSet.Count - 1);

            var orderdPopulationAfterHashCode = new List<InstanceSolution>();

            if (averageRefSetHashCode > averageHashCode)
                orderdPopulationAfterHashCode = population.OrderBy(s => s.HashCode).ToList();
            else
                orderdPopulationAfterHashCode = population.OrderByDescending(s => s.HashCode).ToList();

            foreach (var newSolution in orderdPopulationAfterHashCode)
            {
                scatterSearchStart.ReferenceSetUpdate(newSolution);
            }
        }
    }
}

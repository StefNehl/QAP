using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch
{
    public class RandomGeneratedPopulationMethod : IGenerateInitPopulationMethod
    {
        public List<int[]> GeneratePopulation(int populationSize, int permutationSize)
        {
            var population = new List<int[]>();
            var rg = new Random();

            for(int j = 0; j < populationSize; j++)
            {
                var permutation = new int[permutationSize];

                var listWithPossibilities = new List<int>();
                for (int i = 0; i < permutationSize; i++)
                    listWithPossibilities.Add(i);


                for (int i = 0; i < permutationSize; i++)
                {
                    var newRandomIndex = rg.Next(listWithPossibilities.Count - 1);
                    permutation[i] = listWithPossibilities[newRandomIndex];
                    listWithPossibilities.RemoveAt(newRandomIndex);
                }

                population.Add(permutation);
            }

            return population;
        }
    }
}

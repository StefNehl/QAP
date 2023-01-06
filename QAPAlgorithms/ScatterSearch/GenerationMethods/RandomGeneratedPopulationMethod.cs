using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.GenerationMethods
{
    public class RandomGeneratedPopulationMethod : IGenerateInitPopulationMethod
    {
        private readonly QAPInstance qAPInstance;
        private readonly Random randomGenerator;

        public RandomGeneratedPopulationMethod(QAPInstance qAPInstance, int? seed = null)
        {
            this.qAPInstance = qAPInstance;

            if (seed.HasValue)
                randomGenerator = new Random(Seed: seed.Value);
            else
                randomGenerator = new Random();
        }

        public List<IInstanceSolution> GeneratePopulation(int populationSize, int permutationSize)
        {
            var population = new List<IInstanceSolution>();

            for (int j = 0; j < populationSize; j++)
            {
                var permutation = new int[permutationSize];

                var listWithPossibilities = new List<int>();
                for (int i = 0; i < permutationSize; i++)
                    listWithPossibilities.Add(i);


                for (int i = 0; i < permutationSize; i++)
                {
                    var newRandomIndex = randomGenerator.Next(listWithPossibilities.Count - 1);
                    permutation[i] = listWithPossibilities[newRandomIndex];
                    listWithPossibilities.RemoveAt(newRandomIndex);
                }

                var newSolution = new InstanceSolution(qAPInstance, permutation);
                population.Add(newSolution);
            }

            return population;
        }
    }
}

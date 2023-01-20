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
        private readonly List<IInstanceSolution> population;
        private readonly int[] permutation;
        private readonly List<int> listWithPossibilities;
        private readonly List<int> copyofListWithPosibilities;
        private readonly int populationSize;

        public RandomGeneratedPopulationMethod(QAPInstance qAPInstance,
            int populationSize,
            int permutationSize,
            int? seed = null)
        {
            this.qAPInstance = qAPInstance;
            this.populationSize = populationSize;
            this.population = new List<IInstanceSolution>(populationSize);
            this.permutation = new int[permutationSize];
            
            listWithPossibilities = new List<int>(permutationSize);
            randomGenerator = seed.HasValue ? new Random(Seed: seed.Value) : new Random();
        }

        public List<IInstanceSolution> GeneratePopulation()
        {
            population.Clear();
            for (int j = 0; j < populationSize; j++)
            {
                for (int i = 0; i < permutation.Length; i++)
                    listWithPossibilities.Add(i);
                
                for (int i = 0; i < permutation.Length; i++)
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

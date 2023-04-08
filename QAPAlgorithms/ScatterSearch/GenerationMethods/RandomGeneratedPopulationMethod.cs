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
        private List<InstanceSolution> population;
        private readonly int[] permutation;
        private readonly List<int> listWithPossibilities;

        public RandomGeneratedPopulationMethod(QAPInstance qAPInstance,
            int? seed = null)
        {
            this.qAPInstance = qAPInstance;
            listWithPossibilities = new List<int>();
            randomGenerator = seed.HasValue ? new Random(Seed: seed.Value) : new Random();
            permutation = new int[this.qAPInstance.N];
        }

        public List<InstanceSolution> GeneratePopulation(int populationSize)
        {
            population = new List<InstanceSolution>(populationSize);
            listWithPossibilities.Clear();
            
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

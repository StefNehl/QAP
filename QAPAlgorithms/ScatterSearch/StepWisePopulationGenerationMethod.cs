using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch
{
    public class StepWisePopulationGenerationMethod : IGenerateInitPopulationMethod
    {
        private readonly QAPInstance qAPInstance;


        private int nrOfIndexesToMovePerIteration;
        public StepWisePopulationGenerationMethod(int nrOfIndexesToMovePerIteration, QAPInstance qAPInstance)
        {
            this.nrOfIndexesToMovePerIteration = nrOfIndexesToMovePerIteration;
            this.qAPInstance = qAPInstance;

        }
        public List<IInstanceSolution> GeneratePopulation(int populationSize, int permutationSize)
        {
            var population = new List<IInstanceSolution>();

            for (int s = 0; s < populationSize; s++)
            {
                var newPermutation = new int[permutationSize];
                for (int i = 0; i < newPermutation.Length; i++)
                {
                    if (s == 0)
                        newPermutation[i] = i;
                    else
                    {
                        int newIndex = i - nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = permutationSize + newIndex;
                        newPermutation[i] = population[s - 1].SolutionPermutation[newIndex];
                    }

                }

                var newSolution = new InstanceSolution(qAPInstance, newPermutation);
                population.Add(newSolution);
            }

            return population;
        }
    }
}

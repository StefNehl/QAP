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
        private int nrOfIndexesToMovePerIteration;
        public StepWisePopulationGenerationMethod(int nrOfIndexesToMovePerIteration)
        {
            this.nrOfIndexesToMovePerIteration = nrOfIndexesToMovePerIteration;
        }
        public List<int[]> GeneratePopulation(int populationSize, int permutationSize)
        {
            var population = new List<int[]>();

            for (int s = 0; s < populationSize; s++)
            {
                var newSolution = new int[permutationSize];
                for (int i = 0; i < newSolution.Length; i++)
                {
                    if (s == 0)
                        newSolution[i] = i;
                    else
                    {
                        int newIndex = i - nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = permutationSize + newIndex;
                        newSolution[i] = population[s - 1][newIndex];
                    }

                }
                population.Add(newSolution);
            }

            return population;
        }
    }
}

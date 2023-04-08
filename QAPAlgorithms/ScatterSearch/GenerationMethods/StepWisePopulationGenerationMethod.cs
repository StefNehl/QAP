using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.GenerationMethods
{
    public class StepWisePopulationGenerationMethod : IGenerateInitPopulationMethod
    {
        private readonly QAPInstance qAPInstance;
        private readonly int populationSize;
        private readonly List<InstanceSolution> population;
        private readonly int[] permutation;

        private int nrOfIndexesToMovePerIteration;
        public StepWisePopulationGenerationMethod(int nrOfIndexesToMovePerIteration, 
            QAPInstance qAPInstance, int populationSize)
        {
            this.nrOfIndexesToMovePerIteration = nrOfIndexesToMovePerIteration;
            this.qAPInstance = qAPInstance;

            this.populationSize = populationSize;
            population = new List<InstanceSolution>(populationSize);
            permutation = new int[qAPInstance.N];
        }
        public List<InstanceSolution> GeneratePopulation()
        {

            for (int s = 0; s < populationSize; s++)
            {
                for (int i = 0; i < permutation.Length; i++)
                {
                    if (s == 0)
                        permutation[i] = i;
                    else
                    {
                        int newIndex = i - nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = permutation.Length + newIndex;
                        permutation[i] = population[s - 1].SolutionPermutation[newIndex];
                    }

                }

                var newSolution = new InstanceSolution(qAPInstance, permutation);
                population.Add(newSolution);
            }

            return population;
        }
    }
}

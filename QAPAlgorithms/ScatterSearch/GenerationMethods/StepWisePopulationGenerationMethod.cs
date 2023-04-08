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
        private readonly QAPInstance _qAPInstance;
        private List<InstanceSolution> population;
        private int[] permutation;

        private int _nrOfIndexesToMovePerIteration;
        public StepWisePopulationGenerationMethod(int nrOfIndexesToMovePerIteration, 
            QAPInstance qApInstance)
        {
            _nrOfIndexesToMovePerIteration = nrOfIndexesToMovePerIteration;
            _qAPInstance = qApInstance;
            permutation = new int[qApInstance.N];
        }
        public List<InstanceSolution> GeneratePopulation(int populationSize)
        {
            population = new List<InstanceSolution>(populationSize);
            
            for (int s = 0; s < populationSize; s++)
            {
                for (int i = 0; i < permutation.Length; i++)
                {
                    if (s == 0)
                        permutation[i] = i;
                    else
                    {
                        int newIndex = i - _nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = permutation.Length + newIndex;
                        permutation[i] = population[s - 1].SolutionPermutation[newIndex];
                    }

                }

                var newSolution = new InstanceSolution(_qAPInstance, permutation.ToArray());
                population.Add(newSolution);
            }

            return population;
        }
    }
}

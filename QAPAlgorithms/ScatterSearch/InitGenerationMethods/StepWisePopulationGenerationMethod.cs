using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.GenerationMethods
{
    public class StepWisePopulationGenerationMethod : IGenerateInitPopulationMethod
    {
        private readonly QAPInstance _qAPInstance;
        private readonly int[] _permutation;

        private readonly int _nrOfIndexesToMovePerIteration;
        public StepWisePopulationGenerationMethod(int nrOfIndexesToMovePerIteration, 
            QAPInstance qApInstance)
        {
            _nrOfIndexesToMovePerIteration = nrOfIndexesToMovePerIteration;
            _qAPInstance = qApInstance;
            _permutation = new int[qApInstance.N];
        }
        public List<InstanceSolution> GeneratePopulation(int populationSize)
        {
            var population = new List<InstanceSolution>(populationSize);
            
            for (int s = 0; s < populationSize; s++)
            {
                for (int i = 0; i < _permutation.Length; i++)
                {
                    if (s == 0)
                        _permutation[i] = i;
                    else
                    {
                        int newIndex = i - _nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = _permutation.Length + newIndex;
                        _permutation[i] = population[s - 1].SolutionPermutation[newIndex];
                    }

                }

                var newSolution = new InstanceSolution(_qAPInstance, _permutation.ToArray());
                population.Add(newSolution);
            }

            return population;
        }
    }
}

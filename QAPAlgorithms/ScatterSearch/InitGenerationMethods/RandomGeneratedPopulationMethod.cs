using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.GenerationMethods
{
    public class RandomGeneratedPopulationMethod : IGenerateInitPopulationMethod
    {
        private readonly QAPInstance _qAPInstance;
        private readonly Random _randomGenerator;
        private readonly int[] _permutation;
        private readonly List<int> _listWithPossibilities;

        public RandomGeneratedPopulationMethod(QAPInstance qAPInstance,
            int? seed = null)
        {
            _qAPInstance = qAPInstance;
            _listWithPossibilities = new List<int>();
            _randomGenerator = seed.HasValue ? new Random(Seed: seed.Value) : new Random();
            _permutation = new int[_qAPInstance.N];
        }

        public List<InstanceSolution> GeneratePopulation(int populationSize)
        {
            var population = new List<InstanceSolution>(populationSize);
            _listWithPossibilities.Clear();
            
            for (int j = 0; j < populationSize; j++)
            {
                for (int i = 0; i < _permutation.Length; i++)
                    _listWithPossibilities.Add(i);
                
                for (int i = 0; i < _permutation.Length; i++)
                {
                    var newRandomIndex = _randomGenerator.Next(_listWithPossibilities.Count - 1);
                    _permutation[i] = _listWithPossibilities[newRandomIndex];
                    _listWithPossibilities.RemoveAt(newRandomIndex);
                }

                var newSolution = new InstanceSolution(_qAPInstance, _permutation);
                population.Add(newSolution);
            }

            return population;
        }
    }
}

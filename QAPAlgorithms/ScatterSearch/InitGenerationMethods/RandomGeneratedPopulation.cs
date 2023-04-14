using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.InitGenerationMethods
{
    public class RandomGeneratedPopulation : IGenerateInitPopulationMethod
    {
        private QAPInstance? _qApInstance;
        private int[]? _permutation;
        
        private readonly Random _randomGenerator;
        private readonly List<int> _listWithPossibilities;

        public RandomGeneratedPopulation(
            int? seed = null)
        {
            _listWithPossibilities = new List<int>();
            _randomGenerator = seed.HasValue ? new Random(Seed: seed.Value) : new Random();
        }

        public void InitMethod(QAPInstance instance)
        {
            _qApInstance = instance;
            _permutation = new int[_qApInstance.N];
        }

        private InstanceSolution GenerateSolution()
        {
            for (int i = 0; i < _permutation.Length; i++)
                _listWithPossibilities.Add(i);
                
            for (int i = 0; i < _permutation.Length; i++)
            {
                var newRandomIndex = _randomGenerator.Next(_listWithPossibilities.Count - 1);
                _permutation[i] = _listWithPossibilities[newRandomIndex];
                _listWithPossibilities.RemoveAt(newRandomIndex);
            }

            return new InstanceSolution(_qApInstance, _permutation.ToArray());
        }

        public List<InstanceSolution> GeneratePopulation(int populationSize)
        {
            var population = new List<InstanceSolution>(populationSize);
            _listWithPossibilities.Clear();

            for (int j = 0; j < populationSize; j++)
            {
                population.Add(GenerateSolution());
            }

            return population;
        }
    }
}

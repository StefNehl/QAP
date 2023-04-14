using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class RandomGeneratedPopulationMethodTests
    {
        private RandomGeneratedPopulation _generateInitPopulationMethod;
        private ParallelRandomGeneratedPopulation _parallelRandomGeneratedPopulation;
        private int _populationSize;

        [SetUp]
        public async Task SetUp()
        {
            var testInstance = await QAPInstanceProvider.GetTestN3();
            _populationSize = 10;
            _generateInitPopulationMethod = new RandomGeneratedPopulation();
            _generateInitPopulationMethod.InitMethod(testInstance);

            _parallelRandomGeneratedPopulation = new ParallelRandomGeneratedPopulation(42);
            _parallelRandomGeneratedPopulation.InitMethod(testInstance);
        }

        [Test]
        public void CheckGeneratePopulation_CheckPermutationSizeAndPopulationSize()
        {
            var population = _generateInitPopulationMethod.GeneratePopulation(_populationSize);

            Assert.Multiple(() =>
            {
                Assert.That(population, Has.Count.EqualTo(_populationSize));
                foreach (var solution in population)
                {
                    CheckPermutation(solution.SolutionPermutation);
                }
            });
        }
        
        [Test]
        public void CheckGeneratePopulation_CheckPermutationSizeAndPopulationSize_Parallel()
        {
            var population = _parallelRandomGeneratedPopulation.GeneratePopulation(_populationSize);

            Assert.Multiple(() =>
            {
                Assert.That(population, Has.Count.EqualTo(_populationSize));
                foreach (var solution in population)
                {
                    CheckPermutation(solution.SolutionPermutation);
                }
            });
        }

        private void CheckPermutation(int[] permutation)
        {
            for(int i = 0; i < permutation.Length-1; i++)
            {
                for (int j = (i+1); j < permutation.Length; j++)
                {
                    Assert.That(permutation[i], Is.Not.EqualTo(permutation[j]));
                }
            }
        }

    }
}

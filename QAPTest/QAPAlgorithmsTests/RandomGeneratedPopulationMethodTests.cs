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
        private IGenerateInitPopulationMethod generateInitPopulationMethod;
        private int populationSize;

        [SetUp]
        public async Task SetUp()
        {
            var testInstance = await QAPInstanceProvider.GetTestN3();
            populationSize = 10;
            var permutationSize = 3;
            generateInitPopulationMethod = new RandomGeneratedPopulationMethod();
            generateInitPopulationMethod.InitMethod(testInstance);
        }

        [Test]
        public void CheckGeneratePopulation_CheckPermutationSizeAndPopulationSize()
        {
            var population = generateInitPopulationMethod.GeneratePopulation(populationSize);

            Assert.Multiple(() =>
            {
                Assert.That(population, Has.Count.EqualTo(populationSize));
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

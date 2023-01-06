using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.GenerationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class RandomGeneratedPopulationMethodTests
    {
        private IGenerateInitPopulationMethod generateInitPopulationMethod;

        [SetUp]
        public async Task SetUp()
        {
            var testInstance = await QAPInstanceProvider.GetTestN3();
            generateInitPopulationMethod = new RandomGeneratedPopulationMethod(testInstance); 
        }

        [Test]
        public void CheckGeneratePopulation_CheckPermutationSizeAndPopulationSize()
        {
            var populationSize = 10;
            var permutationSize = 10;

            var population = generateInitPopulationMethod.GeneratePopulation(populationSize, permutationSize);

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

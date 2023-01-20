using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.GenerationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class StepWisePopulationGenerationMethodTests
    {
        private int populationSize = 6;
        private int instanceSize = 3;
        private IGenerateInitPopulationMethod generateInitPopulationMethod;
        private QAPInstance instance;

        [SetUp]
        public async Task SetUp()
        {
            instance = await QAPInstanceProvider.GetTestN3();
            
        }

        [Test]
        public void CheckGenerateInitialPopulation_WithOneStep()
        {
            var generateInitPopulationMethod = new StepWisePopulationGenerationMethod(1, instance, populationSize, instanceSize);
            var p = generateInitPopulationMethod.GeneratePopulation();

            var resultArray = new List<int[]>
            {
                new int[] { 0, 1, 2 },
                new int[] { 2, 0, 1 },
                new int[] { 1, 2, 0 },
                new int[] { 0, 1, 2 },
                new int[] { 2, 0, 1 },
                new int[] { 1, 2, 0 }
            };

            CompareSolutionSets(p.Select(s => s.SolutionPermutation).ToList(), resultArray);
            CompareSolutionSetsWithHashcode(p.Select(s => s.SolutionPermutation).ToList(), resultArray);
        }

        [Test]
        public void CheckGenerateInitialPopulation_WithTwoSteps()
        {
            var generateInitPopulationMethod = new StepWisePopulationGenerationMethod(2, instance, populationSize, instanceSize);
            var p = generateInitPopulationMethod.GeneratePopulation();

            var resultArray = new List<int[]>
            {
                new int[] { 0, 1, 2 },
                new int[] { 1, 2, 0 },
                new int[] { 2, 0, 1 },
                new int[] { 0, 1, 2 },
                new int[] { 1, 2, 0 },
                new int[] { 2, 0, 1 }
            };

            CompareSolutionSets(p.Select(s => s.SolutionPermutation).ToList(), resultArray);
            CompareSolutionSetsWithHashcode(p.Select(s => s.SolutionPermutation).ToList(), resultArray);
        }

        private void CompareSolutionSets(List<int[]> actualSolutionSet, List<int[]> expectedSolutionSet)
        {
            Assert.Multiple(() =>
            {
                for (int s = 0; s < actualSolutionSet.Count(); s++)
                {
                    for (int c = 0; c < actualSolutionSet[s].Length; c++)
                    {
                        Assert.That(actualSolutionSet[s][c], Is.EqualTo(expectedSolutionSet[s][c]), message: $"r:{s} c:{c}");
                    }
                }
            });
        }

        private void CompareSolutionSetsWithHashcode(List<int[]> actualSolutionSet, List<int[]> expectedSolutionSet)
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < actualSolutionSet.Count; i++)
                {
                    Assert.That(InstanceHelpers.IsEqual(actualSolutionSet[i], expectedSolutionSet[i]), Is.True);
                }
            });
        }

    }
}

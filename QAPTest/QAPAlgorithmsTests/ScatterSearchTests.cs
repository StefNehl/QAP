using Domain.Models;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class ScatterSearchTests
    {
        private Instance testInstance;
        private ScatterSearchStart scatterSearch;
        [SetUp]
        public void SetUp()
        {
            var testInstance = new Instance(3, null, null);
            scatterSearch = new ScatterSearchStart(testInstance, 6);
        }

        [Test]
        public void CheckGenerateInitialPopulation_WithOneStep()
        {
            scatterSearch.GenerateInitialPopulation();
            var p = scatterSearch.Population;

            var resultArray = new int[,] { { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0}, { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0 } };
            CompareArray(p, resultArray);
        }

        [Test]
        public void CheckGenerateInitialPopulation_WithTwoSteps()
        {
            scatterSearch.GenerateInitialPopulation(2);
            var p = scatterSearch.Population;

            var resultArray = new int[,] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 }, { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 } };
            CompareArray(p, resultArray);
        }

        [Test]
        public void CheckEliminateIdenticalSolution()
        {
            scatterSearch.GenerateInitialPopulation(1);
            scatterSearch.EliminateIdenticalSolutions();
            var p = scatterSearch.Population;

            var resultArray = new int[,] { { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0 }, { -1, 1, 2 }, { -1, 0, 1 }, { -1, 2, 0 } };
            CompareArray(p, resultArray);
        }



        private void CompareArray(int[,] actualMatrix, int[,] expectedMatrix)
        {
            Assert.Multiple(() =>
            {
                for (int r = 0; r < actualMatrix.GetLength(0); r++)
                {
                    for (int c = 0; c < actualMatrix.GetLength(1); c++)
                    {
                        Assert.That(actualMatrix[r, c], Is.EqualTo(expectedMatrix[r, c]), message: $"r:{r} c:{c}");
                    }
                }
            });
        }
    }
}

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
        [Test]
        public void CheckGenerateInitialPopulation_WithOneStep()
        {
            var testInstance = new Instance(3, null, null);
            var newScatterSearch = new ScatterSearchStart(testInstance, 6);

            newScatterSearch.GenerateInitialPopulation();

            var p = newScatterSearch.Population;

            var resultArray = new int[,] { { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0}, { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0 } };

            Assert.Multiple(() =>
            {
                for(int r = 0; r < p.GetLength(0); r++)
                {
                    for(int c = 0; c < p.GetLength(1); c++)
                    {
                        Assert.That(p[r, c], Is.EqualTo(resultArray[r, c]),message: $"r:{r} c:{c}");
                    }
                }
            });
        }

        [Test]
        public void CheckGenerateInitialPopulation_WithTwoSteps()
        {
            var testInstance = new Instance(3, null, null);
            var newScatterSearch = new ScatterSearchStart(testInstance, 6);

            newScatterSearch.GenerateInitialPopulation(2);

            var p = newScatterSearch.Population;

            var resultArray = new int[,] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 }, { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 } };

            Assert.Multiple(() =>
            {
                for (int r = 0; r < p.GetLength(0); r++)
                {
                    for (int c = 0; c < p.GetLength(1); c++)
                    {
                        Assert.That(p[r, c], Is.EqualTo(resultArray[r, c]), message: $"r:{r} c:{c}");
                    }
                }
            });
        }
    }
}

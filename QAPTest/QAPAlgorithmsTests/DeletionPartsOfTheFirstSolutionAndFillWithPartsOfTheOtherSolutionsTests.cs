using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutionsTests
    {
        private QAPInstance qAPInstance;

        [SetUp]
        public async Task SetUp()
        {
            qAPInstance = await QAPInstanceProvider.GetChr12a();
        }

        [Test]
        public void CombineSolutions_TwoSolutions_DeleteWorstPartOfHalftOfTheSolution_ResultIsTheSameAsSecondSolution()
        {
            var firstSolution = new InstanceSolution(qAPInstance, new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
            var secondSolution = new InstanceSolution(qAPInstance, new int[12] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 });
        
            var listOfSolutions = new List<InstanceSolution>() { firstSolution, secondSolution };

            var combinationMethod = new DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(true, 50, qAPInstance);

            var newSolutions = combinationMethod.CombineSolutions(listOfSolutions);

            Assert.Multiple(() =>
            {
                Assert.That(newSolutions, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void CombineSolutions_TwoSolutions_DeleteWorstPartOfHalftOfTheSolution()
        {
            var firstSolution = new InstanceSolution(qAPInstance, new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
            var secondSolution = new InstanceSolution(qAPInstance, new int[12] { 0, 1, 2, 3, 4, 6, 5, 9, 8, 11, 10, 7 });

            var listOfSolutions = new List<InstanceSolution>() { firstSolution, secondSolution };

            var combinationMethod = new DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(true, 50, qAPInstance);

            var newSolutions = combinationMethod.CombineSolutions(listOfSolutions);

            Assert.Multiple(() =>
            {
                Assert.That(newSolutions, Has.Count.EqualTo(1));
                Assert.That(firstSolution.HashCode, Is.Not.EqualTo(InstanceHelpers.GenerateHashCode(newSolutions[0])));
                Assert.That(secondSolution.HashCode, Is.Not.EqualTo(InstanceHelpers.GenerateHashCode(newSolutions[0])));
            });
        }
    }
}

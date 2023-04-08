using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class LocalSearchBestImprovementTests
    {
        private QAPInstance instance;
        private IImprovementMethod improvementMethod;
        private IImprovementMethod improvedImprovementMethod;
        private int[] worsePermutation;
        private int[] betterPermutation;

        [SetUp]
        public async Task SetUp()
        {
            instance = await QAPInstanceProvider.GetChr12a();
            improvementMethod = new LocalSearchBestImprovement(instance);
            improvedImprovementMethod = new ImprovedLocalSearchBestImprovement(instance);
            
            worsePermutation = new int[] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            betterPermutation = new int[] { 1, 0, 2, 3, 5, 4, 6, 7, 8, 9, 10, 11 };
        }

        [Test]
        public void ImproveSolution()
        {
            var qapSolution = new InstanceSolution(instance, worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;

            improvementMethod.ImproveSolution(qapSolution);
            var betterSolutionValue = qapSolution.SolutionValue;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                for (int i = 0; i < betterPermutation.Length; i++)
                    Assert.That(betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }

        [Test]
        public async Task ImproveSolutions()
        {

            var qapSolution = new InstanceSolution(instance, worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;

            await improvementMethod.ImproveSolutionsInParallelAsync(new List<InstanceSolution>() { qapSolution }, default);
            var betterSolutionValue = qapSolution.SolutionValue;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                for (int i = 0; i < betterPermutation.Length; i++)
                    Assert.That(betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }
        
        [Test]
        public void ImproveSolution_ImprovedAlgo()
        {
            var qapSolution = new InstanceSolution(instance, worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;

            improvedImprovementMethod.ImproveSolution(qapSolution);
            var betterSolutionValue = qapSolution.SolutionValue;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                for (int i = 0; i < betterPermutation.Length; i++)
                    Assert.That(betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }

        [Test]
        public async Task ImproveSolutions_ImprovedAlgo()
        {

            var qapSolution = new InstanceSolution(instance, worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;

            await improvedImprovementMethod.ImproveSolutionsInParallelAsync(new List<InstanceSolution>() { qapSolution }, default);
            var betterSolutionValue = qapSolution.SolutionValue;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                for (int i = 0; i < betterPermutation.Length; i++)
                    Assert.That(betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }
    }
}

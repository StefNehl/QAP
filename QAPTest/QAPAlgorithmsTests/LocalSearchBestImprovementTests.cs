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
        private QAPInstance _instance;
        private IImprovementMethod _improvementMethod;
        private IImprovementMethod _improvedImprovementMethod;
        private IImprovementMethod _improvemenImprovementParallelMethod;
        private int[] _worsePermutation;
        private int[] _betterPermutation;

        [SetUp]
        public async Task SetUp()
        {
            _instance = await QAPInstanceProvider.GetChr12a();
            _improvementMethod = new LocalSearchBestImprovement();
            _improvementMethod.InitMethod(_instance);
            _improvedImprovementMethod = new ImprovedLocalSearchBestImprovement();
            _improvedImprovementMethod.InitMethod(_instance);
            _improvemenImprovementParallelMethod = new ParallelImprovedLocalSearchBestImprovement();
            _improvemenImprovementParallelMethod.InitMethod(_instance);
            
            _worsePermutation = new [] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            _betterPermutation = new [] { 1, 0, 2, 3, 4, 6, 5, 7, 8, 9, 10, 11 };
        }

        [Test]
        public void ImproveSolution()
        {
            var qapSolution = new InstanceSolution(_instance, _worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;
            var worseHashCode = qapSolution.HashCode;

            var betterSolution = _improvementMethod.ImproveSolution(qapSolution);
            var betterSolutionValue = betterSolution.SolutionValue;
            var betterHashCode = betterSolution.HashCode;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                Assert.That(betterHashCode, Is.Not.EqualTo(worseHashCode));
                for (int i = 0; i < _betterPermutation.Length; i++)
                    Assert.That(_betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }

        [Test]
        public void ImproveSolutions()
        {

            var qapSolution = new InstanceSolution(_instance, _worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;
            var worseHashCode = qapSolution.HashCode;

            var solutions = new List<InstanceSolution>() { qapSolution };
            _improvedImprovementMethod.ImproveSolutions(solutions);
            var betterSolution = solutions[0];
            var betterSolutionValue = betterSolution.SolutionValue;
            var betterHashCode = betterSolution.HashCode;
            
            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                Assert.That(betterHashCode, Is.Not.EqualTo(worseHashCode));
                for (int i = 0; i < _betterPermutation.Length; i++)
                    Assert.That(_betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }
        
        [Test]
        public void ImproveSolution_ImprovedAlgo()
        {
            var qapSolution = new InstanceSolution(_instance, _worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;
            var worseHashCode = qapSolution.HashCode;

            var betterSolution = _improvedImprovementMethod.ImproveSolution(qapSolution);
            var betterSolutionValue = betterSolution.SolutionValue;
            var betterHashCode = betterSolution.HashCode;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                Assert.That(betterHashCode, Is.Not.EqualTo(worseHashCode));
                for (int i = 0; i < _betterPermutation.Length; i++)
                    Assert.That(_betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }

        [Test]
        public void ImproveSolutions_ImprovedAlgo()
        {

            var qapSolution = new InstanceSolution(_instance, _worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;
            var worseHashCode = qapSolution.HashCode;

            var solutions = new List<InstanceSolution>() { qapSolution };
            _improvedImprovementMethod.ImproveSolutions(solutions);
            var betterSolution = solutions[0];
            var betterSolutionValue = betterSolution.SolutionValue;
            var betterHashCode = betterSolution.HashCode;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                Assert.That(betterHashCode, Is.Not.EqualTo(worseHashCode));
                for (int i = 0; i < _betterPermutation.Length; i++)
                    Assert.That(_betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }

        [Test]
        public void ImproveSolutions_ImprovedAlgo_Parallel()
        {

            var solutions = new List<InstanceSolution>();
            
            for(int i = 0; i < 50; i++)
                solutions.Add(new InstanceSolution(_instance, _worsePermutation.ToArray()));

            var worseSolutionValue = solutions[0].SolutionValue;
            var worseHashCode = solutions[0].HashCode;

            _improvemenImprovementParallelMethod.ImproveSolutions(solutions);
            var betterSolution = solutions[0];
            var betterSolutionValue = betterSolution.SolutionValue;
            var betterHashCode = betterSolution.HashCode;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                Assert.That(betterHashCode, Is.Not.EqualTo(worseHashCode));
                for (int i = 0; i < _betterPermutation.Length; i++)
                    Assert.That(_betterPermutation[i], Is.EqualTo(solutions[0].SolutionPermutation[i]));
            });
        }
    }
}

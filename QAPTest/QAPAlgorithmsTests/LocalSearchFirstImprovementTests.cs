using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;


namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class LocalSearchFirstImprovementTests
    {
        private QAPInstance instance;
        private IImprovementMethod improvementMethod;
        private IImprovementMethod improvedImprovementMethod;
        private int[] worsePermutation;
        private int[] betterPermutation;

        [SetUp]
        public async Task SetUp()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            instance = await qapReader.ReadFileAsync(folderName, fileName);
            improvementMethod = new LocalSearchFirstImprovement();
            improvementMethod.InitMethod(instance);
            improvedImprovementMethod = new ImprovedLocalSearchFirstImprovement();
            improvedImprovementMethod.InitMethod(instance);

            worsePermutation = new [] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            betterPermutation = new [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        }

        [Test]
        public void ImproveSolution()
        {

            var qapSolution = new InstanceSolution(instance, worsePermutation);
            var worseSolutionValue = qapSolution.SolutionValue;
            var betterSolutionValue = improvementMethod.ImproveSolution(qapSolution).SolutionValue;

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

            var solutions = new List<InstanceSolution>() { qapSolution };
            await improvementMethod.ImproveSolutionsInParallelAsync(solutions);
            var betterSolutionValue = solutions[0].SolutionValue;

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

            
            var betterSolutionValue = improvedImprovementMethod.ImproveSolution(qapSolution).SolutionValue;

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

            var solutions = new List<InstanceSolution>() { qapSolution };
            await improvedImprovementMethod.ImproveSolutionsInParallelAsync(solutions);
            var betterSolutionValue = solutions[0].SolutionValue;

            Assert.Multiple(() =>
            {
                Assert.That(betterSolutionValue, Is.LessThan(worseSolutionValue));
                for (int i = 0; i < betterPermutation.Length; i++)
                    Assert.That(betterPermutation[i], Is.EqualTo(qapSolution.SolutionPermutation[i]));
            });
        }
    }
}

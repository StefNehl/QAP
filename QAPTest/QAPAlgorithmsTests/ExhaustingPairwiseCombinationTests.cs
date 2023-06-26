using Domain.Models;
using QAPAlgorithms.ScatterSearch.CombinationMethods;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class ExhaustingPairwiseCombinationTests
    {
        private ExhaustingPairwiseCombination _combinationMethod;
        private QAPInstance _qapInstanceN4;
        private QAPInstance _qapInstanceN5;

        [SetUp]
        public void SetUp()
        {
            _combinationMethod = new ExhaustingPairwiseCombination();
            _qapInstanceN4 = QAPInstanceProvider.GetTestN4();
            _qapInstanceN5 = QAPInstanceProvider.GetTestN5();
        }

        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_WithN4_StepSize_1()
        {
            var firstPermutation = new [] { 0, 1, 2, 3 };
            var newSolutionA = new InstanceSolution(_qapInstanceN4, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN4, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };

            var result = _combinationMethod.CombineSolutions(list);

            var expectedCount = 2;
            var firstExpectedSolution = new [] { 2, 3, 0, 1 };
            var secondExpectedSolution = new [] { 3, 0, 1, 2 };

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expectedCount));
                Assert.That(result[0][0], Is.EqualTo(firstExpectedSolution[0]));
                Assert.That(result[0][1], Is.EqualTo(firstExpectedSolution[1]));
                Assert.That(result[0][2], Is.EqualTo(firstExpectedSolution[2]));
                Assert.That(result[0][3], Is.EqualTo(firstExpectedSolution[3]));

                Assert.That(result[1][0], Is.EqualTo(secondExpectedSolution[0]));
                Assert.That(result[1][1], Is.EqualTo(secondExpectedSolution[1]));
                Assert.That(result[1][2], Is.EqualTo(secondExpectedSolution[2]));
                Assert.That(result[1][3], Is.EqualTo(secondExpectedSolution[3]));
            });
        }

        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_WithN4_StepSize_2()
        {
            var firstPermutation = new [] { 0, 1, 2, 3 };

            var newSolutionA = new InstanceSolution(_qapInstanceN4, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN4, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };

            _combinationMethod = new ExhaustingPairwiseCombination(2);
            var result = _combinationMethod.CombineSolutions(list);

            var expectedCount = 2;
            var firstExpectedSolution = new [] { 2, 3, 0, 1 };
            var secondExpectedSolution = new [] { 3, 0, 1, 2 };

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expectedCount));
                Assert.That(result[0][0], Is.EqualTo(firstExpectedSolution[0]));
                Assert.That(result[0][1], Is.EqualTo(firstExpectedSolution[1]));
                Assert.That(result[0][2], Is.EqualTo(firstExpectedSolution[2]));
                Assert.That(result[0][3], Is.EqualTo(firstExpectedSolution[3]));

                Assert.That(result[1][0], Is.EqualTo(secondExpectedSolution[0]));
                Assert.That(result[1][1], Is.EqualTo(secondExpectedSolution[1]));
                Assert.That(result[1][2], Is.EqualTo(secondExpectedSolution[2]));
                Assert.That(result[1][3], Is.EqualTo(secondExpectedSolution[3]));
            });
        }

        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_WithN4_StepSize_3()
        {
            var firstPermutation = new [] { 0, 1, 2, 3 };

            var newSolutionA = new InstanceSolution(_qapInstanceN4, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN4, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };
            _combinationMethod = new ExhaustingPairwiseCombination(3);
            Assert.Throws<Exception>(() => _combinationMethod.CombineSolutions(list));
        }

        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_WithN5_StepSize_1()
        {
            var firstPermutation = new [] { 0, 1, 2, 3, 4 };

            var newSolutionA = new InstanceSolution(_qapInstanceN5, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 4, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN5, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };

            var result = _combinationMethod.CombineSolutions(list);

            var expectedCount = 3;

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expectedCount));
            });
        }

        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_WithN5_StepSize_2()
        {
            var firstPermutation = new [] { 0, 1, 2, 3, 4 };

            var newSolutionA = new InstanceSolution(_qapInstanceN5, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 4, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN5, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };

            _combinationMethod = new ExhaustingPairwiseCombination(2);
            var result = _combinationMethod.CombineSolutions(list);

            var expectedCount = 4;

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expectedCount));
            });
        }

        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_WithN5_StepSize_3()
        {
            var firstPermutation = new [] { 0, 1, 2, 3, 4 };

            var newSolutionA = new InstanceSolution(_qapInstanceN5, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 4, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN5, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };
            _combinationMethod = new ExhaustingPairwiseCombination(3);
            Assert.Throws<Exception>(() => _combinationMethod.CombineSolutions(list));
        }

        [Test]
        public void CombineTwoSolutions_MaxPairs2()
        {
            var firstPermutation = new [] { 0, 1, 2, 3, 4 };

            var newSolutionA = new InstanceSolution(_qapInstanceN5, firstPermutation);

            var secondPermutation = new [] { 1, 2, 3, 4, 0 };
            var newSolutionB = new InstanceSolution(_qapInstanceN5, secondPermutation);

            var list = new List<InstanceSolution> { newSolutionA, newSolutionB };
            _combinationMethod = new ExhaustingPairwiseCombination(2, 2);
            var result = _combinationMethod.CombineSolutions(list);
            Assert.That(result, Has.Count.EqualTo(2));
        }
    }
}

using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.GenerationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class ScatterSearchTests
    {
        private QAPInstance _testInstance;
        private ScatterSearch _scatterSearch;
        private IImprovementMethod _improvementMethod;
        private ICombinationMethod _combinationMethod;
        private IGenerateInitPopulationMethod _generateInitPopulationMethod;
        private IDiversificationMethod _diversificationMethod;
        private ISolutionGenerationMethod _solutionGenerationMethod;

        [SetUp]
        public async Task SetUp()
        {
            _testInstance = await QAPInstanceProvider.GetTestN3();
            _improvementMethod = new LocalSearchFirstImprovement(_testInstance);
            _combinationMethod = new ExhaustingPairwiseCombination();
            _generateInitPopulationMethod = new StepWisePopulationGenerationMethod(1, _testInstance);
            _solutionGenerationMethod = new SubSetGenerationMethod(_testInstance, 1, SubSetGenerationMethodType.Cycle, _combinationMethod, _improvementMethod);
            _scatterSearch = new ScatterSearch(_generateInitPopulationMethod, _diversificationMethod, _combinationMethod, _improvementMethod, _solutionGenerationMethod);
        }



        [Test]
        public void CheckEliminateIdenticalSolution()
        {
            var p = _generateInitPopulationMethod.GeneratePopulation(10);
            _scatterSearch.EliminateIdenticalSolutionsFromSet(p);

            var resultArray = new List<int[]> 
            { 
                new [] { 0, 1, 2 }, 
                new [] { 2, 0, 1 },
                new [] { 1, 2, 0 },
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

        [Test]
        public void CheckReferenceSetUpdate_AddEmptyList()
        {
            var result = _scatterSearch.ReferenceSetUpdate(new InstanceSolution( _testInstance, new[] { 0, 1, 2 }));

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void CheckReferenceSetUpdate_AddToWorseList_ReferenceSetFull()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            _testInstance = qapReader.ReadFileAsync("Small", "TestN3.dat").Result;
            _scatterSearch = new ScatterSearch(_generateInitPopulationMethod, _diversificationMethod, _combinationMethod, _improvementMethod, _solutionGenerationMethod, referenceSetSize:1);
            
            var newSolution = new InstanceSolution(_testInstance, new [] { 1, 0, 2 });
            _scatterSearch.ReferenceSetUpdate(newSolution);

            var newInstanceSolution = new InstanceSolution(_testInstance, new[] { 0, 1, 2 });
            var result = _scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(_scatterSearch.GetReferenceSetCount(), Is.EqualTo(1));
                Assert.That(_scatterSearch.GetBestSolution().SolutionValue, Is.Not.EqualTo(newInstanceSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddToWorseList()
        {
            var bestSolution = new InstanceSolution(_testInstance, new[] { 1, 0, 2 });
            _scatterSearch.ReferenceSetUpdate(bestSolution);

            var newInstanceSolution = new InstanceSolution(_testInstance, new[] { 0, 1, 2 });
            var result = _scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(true));
                Assert.That(_scatterSearch.GetReferenceSetCount(), Is.EqualTo(2));
                Assert.That(_scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(bestSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddBetterToList()
        {
            var newSolution = new InstanceSolution(_testInstance, new [] { 0, 1, 2 });
            _scatterSearch.ReferenceSetUpdate(newSolution);

            var newInstanceSolution = new InstanceSolution(_testInstance, new[] { 1, 0, 2 });
            var result = _scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(true));
                Assert.That(_scatterSearch.GetReferenceSetCount(), Is.EqualTo(2));
                Assert.That(_scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(newInstanceSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddBetterToList_ReferenceSetFull()
        {
            _scatterSearch = new ScatterSearch(_generateInitPopulationMethod, _diversificationMethod, _combinationMethod, _improvementMethod, _solutionGenerationMethod, referenceSetSize:1);

            var newSolution = new InstanceSolution(_testInstance, new [] { 0, 1, 2 });
            _scatterSearch.ReferenceSetUpdate(newSolution);

            var newInstanceSolution = new InstanceSolution(_testInstance, new[] { 1, 0, 2 });
            var result = _scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(true));
                Assert.That(_scatterSearch.GetReferenceSetCount(), Is.EqualTo(1));
                Assert.That(_scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(newInstanceSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddRandomSolutionValues_CheckCount()
        {
            _scatterSearch = new ScatterSearch(_generateInitPopulationMethod, _diversificationMethod, _combinationMethod, _improvementMethod, _solutionGenerationMethod);
            var rg = new Random();
            
            for (int i = 1; i <= 100; i++)
            {
                var newTestSolution = new InstanceSolution(_testInstance, new[] { rg.Next(0, _testInstance.N-1), rg.Next(0, _testInstance.N-1), rg.Next(0, _testInstance.N-1) });
                _scatterSearch.ReferenceSetUpdate(newTestSolution);
            }

            Assert.That(_scatterSearch.GetReferenceSetCount(), Is.EqualTo(5));
        }
    }
}

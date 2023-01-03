﻿using Domain;
using Domain.Models;
using Moq;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class ScatterSearchTests
    {
        private QAPInstance testInstance;
        private ScatterSearchStart scatterSearch;
        private IImprovementMethod improvementMethod;
        private ICombinationMethod combinationMethod;
        private IGenerateInitPopulationMethod generateInitPopulationMethod;
        private int populationSize;

        [SetUp]
        public void SetUp()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            testInstance = qapReader.ReadFileAsync("Small", "TestN3.dat").Result;
            improvementMethod = new LocalSearchFirstImprovement(testInstance);
            combinationMethod = new ExhaustingPairwiseCombination();
            generateInitPopulationMethod = new StepWisePopulationGenerationMethod(1);
            populationSize = 6;
            scatterSearch = new ScatterSearchStart(testInstance, improvementMethod, combinationMethod, generateInitPopulationMethod);
        }



        [Test]
        public void CheckEliminateIdenticalSolution()
        {
            var p = generateInitPopulationMethod.GeneratePopulation(1, testInstance.N);
            scatterSearch.EliminateIdenticalSolutionsFromSet(p);

            var resultArray = new List<int[]> 
            { 
                new int[] { 0, 1, 2 }, 
                new int[] { 2, 0, 1 },
                new int[] { 1, 2, 0 },
            };
            CompareSolutionSets(p, resultArray);
            CompareSolutionSetsWithHashcode(p, resultArray);
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
            var result = scatterSearch.ReferenceSetUpdate(new InstanceSolution( testInstance, new[] { 0, 1, 2 }));

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void CheckReferenceSetUpdate_AddToWorseList_ReferenceSetFull()
        {
            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            testInstance = qapReader.ReadFileAsync("Small", "TestN3.dat").Result;
            scatterSearch = new ScatterSearchStart(testInstance, improvementMethod, combinationMethod, generateInitPopulationMethod, 6, 1);
            
            var newSolution = new InstanceSolution(testInstance, new int[] { 1, 0, 2 });
            scatterSearch.ReferenceSetUpdate(newSolution);

            var newInstanceSolution = new InstanceSolution(testInstance, new[] { 0, 1, 2 });
            var result = scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(scatterSearch.GetReferenceSetCount(), Is.EqualTo(1));
                Assert.That(scatterSearch.GetBestSolution().SolutionValue, Is.Not.EqualTo(newInstanceSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddToWorseList()
        {
            var bestSolution = new InstanceSolution(testInstance, new int[] { 1, 0, 2 });
            scatterSearch.ReferenceSetUpdate(bestSolution);

            var newInstanceSolution = new InstanceSolution(testInstance, new[] { 0, 1, 2 });
            var result = scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(true));
                Assert.That(scatterSearch.GetReferenceSetCount(), Is.EqualTo(2));
                Assert.That(scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(bestSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddBetterToList()
        {
            var newSolution = new InstanceSolution(testInstance, new int[] { 0, 1, 2 });
            scatterSearch.ReferenceSetUpdate(newSolution);

            var newInstanceSolution = new InstanceSolution(testInstance, new[] { 1, 0, 2 });
            var result = scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(true));
                Assert.That(scatterSearch.GetReferenceSetCount(), Is.EqualTo(2));
                Assert.That(scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(newInstanceSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddBetterToList_ReferenceSetFull()
        {
            scatterSearch = new ScatterSearchStart(testInstance, improvementMethod, combinationMethod, generateInitPopulationMethod, 6, 1);

            var newSolution = new InstanceSolution(testInstance, new int[] { 0, 1, 2 });
            scatterSearch.ReferenceSetUpdate(newSolution);

            var newInstanceSolution = new InstanceSolution(testInstance, new[] { 1, 0, 2 });
            var result = scatterSearch.ReferenceSetUpdate(newInstanceSolution);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(true));
                Assert.That(scatterSearch.GetReferenceSetCount(), Is.EqualTo(1));
                Assert.That(scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(newInstanceSolution.SolutionValue));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddBiggerSolutionValues_CheckCount()
        {
            scatterSearch = new ScatterSearchStart(testInstance, improvementMethod, combinationMethod, generateInitPopulationMethod, 6, 5);

            var solutionList = new List<IInstanceSolution>();
            for(int i = 1; i <= 100; i++)
            {
                var newTestSolution = new TestInstanceSolution()
                {
                    HashCode = i,
                    SolutionValue = i  * 10
                };

                scatterSearch.ReferenceSetUpdate(newTestSolution);
            }

            Assert.Multiple(() =>
            {
                Assert.That(scatterSearch.GetReferenceSetCount(), Is.EqualTo(5));
                Assert.That(scatterSearch.GetBestSolution().SolutionValue, Is.EqualTo(10));
            });
        }

        [Test]
        public void CheckReferenceSetUpdate_AddRandomSolutionValues_CheckCount()
        {
            scatterSearch = new ScatterSearchStart(testInstance, improvementMethod, combinationMethod, generateInitPopulationMethod, 6, 5);

            var rg = new Random();

            for (int i = 1; i <= 100; i++)
            {
                var newTestSolution = new TestInstanceSolution()
                {
                    HashCode = i,
                    SolutionValue = rg.Next(1, 10) * 10
                };

                scatterSearch.ReferenceSetUpdate(newTestSolution);
            }

            Assert.That(scatterSearch.GetReferenceSetCount(), Is.EqualTo(5));
        }
    }

    public class TestInstanceSolution : IInstanceSolution
    {
        public long HashCode { get; set; }

        public int[] SolutionPermutation => throw new NotImplementedException();

        public long SolutionValue { get; set; }

        public int CompareTo(IInstanceSolution? other)
        {
            if (other == null)
                return 1;
            return SolutionValue.CompareTo(other.SolutionValue);
        }

        public string DisplayInConsole()
        {
            throw new NotImplementedException();
        }

        public void RefreshSolutionValue(QAPInstance instance)
        {
            throw new NotImplementedException();
        }
    }
}

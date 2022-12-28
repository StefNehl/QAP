﻿using Domain;
using Domain.Models;
using Moq;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPAlgorithmsTests
{
    [TestFixture]
    public class SubSetGenerationMethodTests
    {
        [Test]
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_StepSize_1()
        {
            var firstPermutation = new int[4] { 0, 1, 2, 3 };

            var newSolutionA = new Mock<IInstanceSolution>();
            newSolutionA.Setup(p => p.SolutionPermutation).Returns(firstPermutation);
            newSolutionA.Setup(p => p.HashCode).Returns(InstanceHelpers.GenerateHashCode(firstPermutation));

            var secondPermutation = new int[4] { 1, 2, 3, 0 };
            var newSolutionB = new Mock<IInstanceSolution>();
            newSolutionB.Setup(p => p.SolutionPermutation).Returns(secondPermutation);
            newSolutionB.Setup(p => p.HashCode).Returns(InstanceHelpers.GenerateHashCode(secondPermutation));

            var list = new List<IInstanceSolution> { newSolutionA.Object, newSolutionB.Object };

            var result = SubSetGenerationMethod.CombineSolutionsPairWise(list);

            var expectedCount = 2;
            var firstExpectedSolution = new int[4] { 2, 3, 0, 1 };
            var secondExpectedSolution = new int[4] { 3, 0, 1, 2 };

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
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_StepSize_2()
        {
            var firstPermutation = new int[4] { 0, 1, 2, 3 };

            var newSolutionA = new Mock<IInstanceSolution>();
            newSolutionA.Setup(p => p.SolutionPermutation).Returns(firstPermutation);
            newSolutionA.Setup(p => p.HashCode).Returns(InstanceHelpers.GenerateHashCode(firstPermutation));

            var secondPermutation = new int[4] { 1, 2, 3, 0 };
            var newSolutionB = new Mock<IInstanceSolution>();
            newSolutionB.Setup(p => p.SolutionPermutation).Returns(secondPermutation);
            newSolutionB.Setup(p => p.HashCode).Returns(InstanceHelpers.GenerateHashCode(secondPermutation));

            var list = new List<IInstanceSolution> { newSolutionA.Object, newSolutionB.Object };

            var result = SubSetGenerationMethod.CombineSolutionsPairWise(list, 2);

            var expectedCount = 2;
            var firstExpectedSolution = new int[4] { 2, 3, 0, 1 };
            var secondExpectedSolution = new int[4] { 3, 0, 1, 2 };

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
        public void CombineTwoSolutionsPairWise_WithTwoDifferentSolutions_StepSize_3()
        {
            var firstPermutation = new int[4] { 0, 1, 2, 3 };

            var newSolutionA = new Mock<IInstanceSolution>();
            newSolutionA.Setup(p => p.SolutionPermutation).Returns(firstPermutation);
            newSolutionA.Setup(p => p.HashCode).Returns(InstanceHelpers.GenerateHashCode(firstPermutation));

            var secondPermutation = new int[4] { 1, 2, 3, 0 };
            var newSolutionB = new Mock<IInstanceSolution>();
            newSolutionB.Setup(p => p.SolutionPermutation).Returns(secondPermutation);
            newSolutionB.Setup(p => p.HashCode).Returns(InstanceHelpers.GenerateHashCode(secondPermutation));

            var list = new List<IInstanceSolution> { newSolutionA.Object, newSolutionB.Object };
            Assert.Throws<Exception>(() => SubSetGenerationMethod.CombineSolutionsPairWise(list, 3));
        }
    }
}
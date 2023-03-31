using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.DomainTests
{
    [TestFixture]
    public class InstanceHelpersTests
    {
        private QAPInstance chr12a;
        private QAPInstance chr12b;
        private QAPInstance chr25a;

        [SetUp]
        public async Task SetUp()
        {
            chr12a = await QAPInstanceProvider.GetChr12a();
            chr12b = await QAPInstanceProvider.GetChr12b();
            chr25a = await QAPInstanceProvider.GetChr25a();
        }

        [Test]
        public void GenerateHashcode_HashCode()
        {
            var permutation = new[] { 0, 1, 2 };
            var hashCode = InstanceHelpers.GenerateHashCode(permutation);

            Assert.That(hashCode, Is.EqualTo(36));
        }


        [Test]
        public void GetSolutionValue_OptimalPermutations()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a());
            var solutionValueChr12b = InstanceHelpers.GetSolutionValue(chr12b, QAPInstanceProvider.GetOptimalPermutationForChr12b());
            var solutionValueChr25a = InstanceHelpers.GetSolutionValue(chr25a, QAPInstanceProvider.GetOptimalPermutationForChr25a());

            Assert.Multiple(() =>
            {
                Assert.That(solutionValueChr12a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12a()));
                Assert.That(solutionValueChr12b, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12b()));
                Assert.That(solutionValueChr25a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr25a()));
            });
        }

        [Test]
        public void GetSolutionValue_WithStartAndEndIndex_StartToEnd()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 0, chr12a.N - 1);

            Assert.Multiple(() =>
            {
                Assert.That(solutionValueChr12a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12a()));
            });
        }
        
        [Test]
        public void GetSolutionValueOp_OptimalPermutations()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a());
            var solutionValueChr12b = InstanceHelpers.GetSolutionValue(chr12b, QAPInstanceProvider.GetOptimalPermutationForChr12b());
            var solutionValueChr25a = InstanceHelpers.GetSolutionValue(chr25a, QAPInstanceProvider.GetOptimalPermutationForChr25a());

            Assert.Multiple(() =>
            {
                Assert.That(solutionValueChr12a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12a()));
                Assert.That(solutionValueChr12b, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12b()));
                Assert.That(solutionValueChr25a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr25a()));
            });
        }

        [Test]
        public void GetSolutionValueOp_WithStartAndEndIndex_StartToEnd()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a());

            Assert.Multiple(() =>
            {
                Assert.That(solutionValueChr12a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12a()));
            });
        }

        [Test]
        public void GetSolutionValue_WithStartAndEndIndex_StartTo5()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a());
            Assert.That(solutionValueChr12a, Is.EqualTo(9552));
        }

        [Test]
        public void GetSolutionValue_WithStartAndEndIndex_5ToEnd()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 5, chr12a.N-1);
            Assert.That(solutionValueChr12a, Is.EqualTo(1706));
        }

        [Test]
        public void GetSolutionValue_WithStartAndEndIndex_5To7()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 5, 9);
            Assert.That(solutionValueChr12a, Is.EqualTo(122));
        }

        [Test]
        public void IsNumberAlreadyInTheSolution_IsFalse()
        {
            var permutation = new int[3] { 1, 2, 3 };
            var result = InstanceHelpers.IsValueAlreadyInThePermutation(0, permutation);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNumberAlreadyInTheSolution_IsTrue()
        {
            var permutation = new int[3] { 1, 2, 3 };
            var result = InstanceHelpers.IsValueAlreadyInThePermutation(1, permutation);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValueAlreadyInTheSolution_WithStartEndIndex_IsFalse()
        {
            var permutation = new int[3] { 1, 2, 3 };
            var result = InstanceHelpers.IsValueAlreadyInThePermutation(3, permutation, 0, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValueAlreadyInTheSolution_WithStartEndIndex_IsTrue()
        {
            var permutation = new int[3] { 1, 2, 3 };
            var result = InstanceHelpers.IsValueAlreadyInThePermutation(1, permutation, 0, 1);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetIndexOfWorstPart_GetWorstHalf()
        {
            var permutation = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var index = InstanceHelpers.GetIndexOfWorstPart(permutation, 6, chr12a);

            Assert.That(index, Is.EqualTo(0));
        }

        [Test]
        public async Task GetIndexOfWorstPart_SmallerInstance()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutation = new int[3] { 0, 1, 2 };

            var index = InstanceHelpers.GetIndexOfWorstPart(permutation, 2, instance);
            Assert.That(index, Is.EqualTo(0));
        }

        [Test]
        public async Task GetIndexOfWorstPart_ThrowException()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutation = new int[3] { 0, 1, 2 };

            Assert.Throws<Exception>(() => InstanceHelpers.GetIndexOfWorstPart(permutation, 1, instance));
        }

        [Test]
        public void GetIndexOfBestPart_GetWorstHalf()
        {
            var permutation = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var index = InstanceHelpers.GetIndexOfBestPart(permutation, 6, chr12a);

            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public async Task GetIndexOfBestPart_SmallerInstance()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutation = new int[3] { 0, 1, 2 };

            var index = InstanceHelpers.GetIndexOfBestPart(permutation, 2, instance);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public async Task GetIndexOfBestPart_ThrowException()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutation = new int[3] { 0, 1, 2 };

            Assert.Throws<Exception>(() => InstanceHelpers.GetIndexOfBestPart(permutation, 1, instance));
        }

        [Test]
        public async Task GetSolutionDifferenceAfterSwap_fromZeroToOne()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutationBefore = new int[3] { 0, 1, 2 };
            var permutationAfter = new int[3] { 1, 0, 2 };

            int indexFrom = 0;
            int indexTo = 1;

            var resultDiff = InstanceHelpers.GetSolutionDifferenceAfterSwap(
                instance,
                permutationBefore,
                indexFrom,
                indexTo);

            var resultBefore = InstanceHelpers.GetSolutionValue(instance, permutationBefore);
            var resultAfter = InstanceHelpers.GetSolutionValue(instance, permutationAfter);
            
            Assert.That(resultDiff, Is.EqualTo(resultAfter - resultBefore));
        }
        
        [Test]
        public async Task GetSolutionDifferenceAfterSwap_fromOneToZero()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutationBefore = new int[3] { 0, 1, 2 };
            var permutationAfter = new int[3] { 1, 0, 2 };

            int indexFrom = 1;
            int indexTo = 0;

            var resultDiff = InstanceHelpers.GetSolutionDifferenceAfterSwap(
                instance,
                permutationBefore,
                indexFrom,
                indexTo);

            var resultBefore = InstanceHelpers.GetSolutionValue(instance, permutationBefore);
            var resultAfter = InstanceHelpers.GetSolutionValue(instance, permutationAfter);
            
            Assert.That(resultDiff, Is.EqualTo(resultAfter - resultBefore));
        }
        
        [Test]
        public async Task GetSolutionDifferenceAfterSwap_fromZeroToLast()
        {
            var instance = await QAPInstanceProvider.GetTestN3();
            var permutationBefore = new int[3] { 0, 1, 2 };
            var permutationAfter = new int[3] { 2, 1, 0};

            int indexFrom = 0;
            int indexTo = 2;

            var resultDiff = InstanceHelpers.GetSolutionDifferenceAfterSwap(
                instance,
                permutationBefore,
                indexFrom,
                indexTo);

            var resultBefore = InstanceHelpers.GetSolutionValue(instance, permutationBefore);
            var resultAfter = InstanceHelpers.GetSolutionValue(instance, permutationAfter);
            
            Assert.That(resultDiff, Is.EqualTo(resultAfter - resultBefore));
        }

        [Test]
        public async Task GetSolutionDifferenceAfterSwap_CHR12A()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            var instance = await qapReader.ReadFileAsync(folderName, fileName);
            
            var worsePermutation = new int[] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var betterPermutation = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            var resultDiff = InstanceHelpers.GetSolutionDifferenceAfterSwap(instance, worsePermutation, 0, 1);
            
            var resultBefore = InstanceHelpers.GetSolutionValue(instance, worsePermutation);
            var resultAfter = InstanceHelpers.GetSolutionValue(instance, betterPermutation);
            
            Assert.That(resultDiff, Is.EqualTo(resultAfter - resultBefore));
        }
        
        [Test]
        public async Task GetSolutionDifferenceAfterSwap_Random()
        {
            var instance = await QAPInstanceProvider.GetChr25a();
            var random = new Random();

            var permutationBefore = new int[instance.N];
            for (int i = 0; i < permutationBefore.Length; i++)
                permutationBefore[i] = i;
            
            for (int i = 0; i < 100; i++)
            {
                var indexFrom = random.Next(0, instance.N-1);
                var indexTo = random.Next(0, instance.N - 1);
                
                TestRandom(permutationBefore, indexFrom, indexTo, instance);
            }

        }

        private void TestRandom(int[] permutationBefore, int indexFrom, int indexTo, QAPInstance instance)
        {
            var permutationAfter = Swap(permutationBefore, indexFrom, indexTo);

            var resultDiff = InstanceHelpers.GetSolutionDifferenceAfterSwap(
                instance,
                permutationBefore,
                indexFrom,
                indexTo);

            var resultBefore = InstanceHelpers.GetSolutionValue(instance, permutationBefore);
            var resultAfter = InstanceHelpers.GetSolutionValue(instance, permutationAfter);
            
            Assert.That(resultDiff, Is.EqualTo(resultAfter - resultBefore));
        }

        private int[] Swap(int[] permutationBefore, int indexFrom, int indexTo)
        {
            var permutationAfter = (int[])permutationBefore.Clone();
            (permutationAfter[indexFrom], permutationAfter[indexTo]) = (permutationAfter[indexTo], permutationAfter[indexFrom]);
            return permutationAfter;
        }
        
        
        
    }
}

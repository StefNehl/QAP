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
        public void CheckGenerateHashcode_CheckHashCode()
        {
            var permutation = new[] { 0, 1, 2 };
            var hashCode = InstanceHelpers.GenerateHashCode(permutation);

            Assert.That(hashCode, Is.EqualTo(36));
        }


        [Test]
        public void CheckGetSolutionValue_CheckOptimalPermutations()
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
        public void CheckGetSolutionValue_CheckWithStartAndEndIndex_StartToEnd()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 0, chr12a.N - 1);

            Assert.Multiple(() =>
            {
                Assert.That(solutionValueChr12a, Is.EqualTo(QAPInstanceProvider.GetOptimalSolutionValueForChr12a()));
            });
        }

        [Test]
        public void CheckGetSolutionValue_CheckWithStartAndEndIndex_StartTo5()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 0, 5);
            Assert.That(solutionValueChr12a, Is.EqualTo(5554));
        }

        [Test]
        public void CheckGetSolutionValue_CheckWithStartAndEndIndex_5ToEnd()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 5, chr12a.N-1);
            Assert.That(solutionValueChr12a, Is.EqualTo(1706));
        }

        [Test]
        public void CheckGetSolutionValue_CheckWithStartAndEndIndex_5To7()
        {
            var solutionValueChr12a = InstanceHelpers.GetSolutionValue(chr12a, QAPInstanceProvider.GetOptimalPermutationForChr12a(), 5, 9);
            Assert.That(solutionValueChr12a, Is.EqualTo(122));
        }
    }
}

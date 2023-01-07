using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.QAPInstanceTests
{
    [TestFixture]
    public class QAPInstanceTests
    {
        private QAPInstanceReader.QAPInstanceReader _reader;
        
        [SetUp]
        public void SetUp()
        {
            _reader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        }



        [Test]
        public async Task Check_GetInstanceValue_chr12a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            //Optimal permuatation from https://www.opt.math.tugraz.at/qaplib/inst.html
            var optimalPermutation = QAPInstanceProvider.GetOptimalPermutationForChr12a();
            long optimalInstanceValue = QAPInstanceProvider.GetOptimalSolutionValueForChr12a();

            var instance = await _reader.ReadFileAsync(folderName, fileName);
            var instancevalue = InstanceHelpers.GetSolutionValue(instance, optimalPermutation);

            Assert.That(instancevalue, Is.EqualTo(optimalInstanceValue));
        }

        [Test]
        public async Task Check_GetInstanceValue_chr12b()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12b.dat";

            //Optimal permuatation from https://www.opt.math.tugraz.at/qaplib/inst.html
            var optimalPermutation = QAPInstanceProvider.GetOptimalPermutationForChr12b();
            long optimalInstanceValue = QAPInstanceProvider.GetOptimalSolutionValueForChr12b();

            var instance = await _reader.ReadFileAsync(folderName, fileName);
            var instancevalue = InstanceHelpers.GetSolutionValue(instance, optimalPermutation);

            Assert.That(instancevalue, Is.EqualTo(optimalInstanceValue));
        }

        [Test]
        public async Task Check_GetInstanceValue_chr25a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr25a.dat";

            //Optimal permuatation from https://www.opt.math.tugraz.at/qaplib/inst.html
            var optimalPermutation = QAPInstanceProvider.GetOptimalPermutationForChr25a();
            long optimalInstanceValue = QAPInstanceProvider.GetOptimalSolutionValueForChr25a();

            var instance = await _reader.ReadFileAsync(folderName, fileName);
            var instancevalue = InstanceHelpers.GetSolutionValue(instance, optimalPermutation);

            Assert.That(instancevalue, Is.EqualTo(optimalInstanceValue));
        }
    }
}

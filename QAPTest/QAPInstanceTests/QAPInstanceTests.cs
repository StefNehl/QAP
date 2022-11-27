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

        private void ReduceIndexOfPermutation(int[] permutation)
        {
            for(int i = 0; i < permutation.Length; i++)
                permutation[i] = permutation[i] - 1;
        }

        [Test]
        public async Task Check_GetInstanceValue_chr12a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12a.dat";

            //Optimal permuatation from https://www.opt.math.tugraz.at/qaplib/inst.html
            var optimalPermutation = new int[] { 7, 5, 12, 2, 1, 3, 9, 11, 10, 6, 8, 4 };
            ReduceIndexOfPermutation(optimalPermutation);

            int optimalInstanceValue = 9552;

            var instance = await _reader.ReadFileAsync(folderName, fileName);
            var instancevalue = instance.GetInstanceValue(optimalPermutation);

            Assert.That(instancevalue, Is.EqualTo(optimalInstanceValue));
        }

        [Test]
        public async Task Check_GetInstanceValue_chr12b()
        {
            var folderName = "QAPLIB";
            var fileName = "chr12b.dat";

            //Optimal permuatation from https://www.opt.math.tugraz.at/qaplib/inst.html
            var optimalPermutation = new int[] { 5, 7, 1, 10, 11, 3, 4, 2, 9, 6, 12, 8 };
            ReduceIndexOfPermutation(optimalPermutation);

            int optimalInstanceValue = 9742;

            var instance = await _reader.ReadFileAsync(folderName, fileName);
            var instancevalue = instance.GetInstanceValue(optimalPermutation);

            Assert.That(instancevalue, Is.EqualTo(optimalInstanceValue));
        }

        [Test]
        public async Task Check_GetInstanceValue_chr25a()
        {
            var folderName = "QAPLIB";
            var fileName = "chr25a.dat";

            //Optimal permuatation from https://www.opt.math.tugraz.at/qaplib/inst.html
            var optimalPermutation = new int[] { 25, 12, 5, 3, 18, 4, 16, 8, 20, 10, 14, 6, 15, 23, 24, 19, 13, 1, 21, 11, 17, 2, 22, 7, 9 };
            ReduceIndexOfPermutation(optimalPermutation);

            int optimalInstanceValue = 3796;

            var instance = await _reader.ReadFileAsync(folderName, fileName);
            var instancevalue = instance.GetInstanceValue(optimalPermutation);

            Assert.That(instancevalue, Is.EqualTo(optimalInstanceValue));
        }
    }
}

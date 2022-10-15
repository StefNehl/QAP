using QAPInstanceReader;

namespace QAPTest.QAPInstanceReaderTests
{
    public class QAPInstanceReaderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFolderNotEmpty()
        {
            var reader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            Assert.That(reader.Folders.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task TestInstanceLoadingMatrixSize()
        {
            var reader = QAPInstanceReader.QAPInstanceReader.GetInstance();
            foreach (var folder in reader.Folders) 
            {
                foreach(var file in reader.GetFilesInFolder(folder))
                {
                    var instance = await reader.ReadFileAsync(folder, file);
                    Assert.IsNotNull(instance);
                    int matrixlength = instance.N * instance.N;
                    Assert.Multiple(() =>
                    {
                        Assert.That(instance.A.Length, Is.EqualTo(matrixlength));
                        Assert.That(instance.B.Length, Is.EqualTo(matrixlength));
                    });
                }
            }
        }
    }
}
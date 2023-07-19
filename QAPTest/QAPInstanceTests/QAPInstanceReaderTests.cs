using QAPInstanceReader;

namespace QAPTest.QAPInstanceTests
{
    [TestFixture]
    public class QAPInstanceReaderTests
    {

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
                    try
                    {
                        var instance = await reader.ReadFileAsync(folder, file);
                        Assert.IsNotNull(instance);
                        int matrixlength = instance.N * instance.N;
                        Assert.Multiple(() =>
                        {
                            Assert.That(instance.A.Length, Is.EqualTo(matrixlength), file);
                            Assert.That(instance.B.Length, Is.EqualTo(matrixlength), file);
                        });
                    }
                    catch (Exception e)
                    {
                        throw new Exception(file, innerException:e);
                    }
                }
            }
        }
    }
}
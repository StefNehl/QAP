using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.DiversificationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPTest.QAPAlgorithmsTests;

[TestFixture]
public class DiversificationTests
{
    private HashCodeDiversification _hashCodeDiversificationMethod;
    private HashCodeThreePartDiversification _hashCodeThreePartDiversification;
    private IGenerateInitPopulationMethod _generateInitPopulationMethod;
    
    private QAPInstance _testInstance;

    private List<InstanceSolution> _population;
    private List<InstanceSolution> _referenceSet;
    private int _referenceSetSize;

    [SetUp]
    public async Task SetUp()
    {
        _testInstance = await QAPInstanceProvider.GetChr25a();
        _generateInitPopulationMethod = new StepWisePopulationGeneration(1);
        _generateInitPopulationMethod.InitMethod(_testInstance);

        _hashCodeDiversificationMethod = new HashCodeDiversification();
        _hashCodeDiversificationMethod.InitMethod(_testInstance);

        _hashCodeThreePartDiversification = new HashCodeThreePartDiversification();
        _hashCodeThreePartDiversification.InitMethod(_testInstance);
        
        _population = _generateInitPopulationMethod.GeneratePopulation(100);
        _referenceSet = new List<InstanceSolution>();
        _referenceSetSize = 24;
        
        foreach(var solution in _population)
        {
            ScatterSearch.ReferenceSetUpdate(solution, _referenceSet, _referenceSetSize);
        }

    }

    [Test]
    public void TestHashCodeDiversificationMethod_RefSetSize()
    { 
        _hashCodeDiversificationMethod.ApplyDiversificationMethod(_referenceSet, _population);
        Assert.AreEqual(_referenceSetSize, _referenceSet.Count);
    }

    [Test]
    public void TestHashCodeThreePartDiversification_RefSetSize()
    {
        _hashCodeThreePartDiversification.ApplyDiversificationMethod(_referenceSet, _population);
        Assert.AreEqual(_referenceSetSize, _referenceSet.Count);
    }

    [Test]
    public void TestHashCodeThreePartDiversification_GetStartAndEndIndexForPart_UnevenPartSize()
    {
        var partSize = 5;
        var bestIndex = 3;
        var maxSize = 5;

        var startEndTuple = HashCodeThreePartDiversification
            .GetStartAndEndIndexForPart(bestIndex, partSize, maxSize);
        
        Assert.Multiple(() =>
        {
            Assert.AreEqual(1, startEndTuple.Item1);
            Assert.AreEqual(5, startEndTuple.Item2);
        });
    }

    [Test]
    public void TestHashCodeThreePartDiversification_GetStartAndEndIndexForPart_EvenPartSize()
    {
        var partSize = 4;
        var bestIndex = 3;
        var maxSize = 5;

        var startEndTuple = HashCodeThreePartDiversification
            .GetStartAndEndIndexForPart(bestIndex, partSize, maxSize);
        
        Assert.Multiple(() =>
        {
            Assert.AreEqual(2, startEndTuple.Item1);
            Assert.AreEqual(5, startEndTuple.Item2);
        });
    }
    
    [Test]
    public void TestHashCodeThreePartDiversification_GetStartAndEndIndexForPart_FirstIndex()
    {
        var partSize = 5;
        var bestIndex = 0;
        var maxSize = 4;

        var startEndTuple = HashCodeThreePartDiversification
            .GetStartAndEndIndexForPart(bestIndex, partSize, maxSize);
        
        Assert.Multiple(() =>
        {
            Assert.AreEqual(0, startEndTuple.Item1);
            Assert.AreEqual(4, startEndTuple.Item2);
        });
    }
    
    [Test]
    public void TestHashCodeThreePartDiversification_GetStartAndEndIndexForPart_LastIndex()
    {
        var partSize = 5;
        var bestIndex = 4;
        var maxSize = 4;

        var startEndTuple = HashCodeThreePartDiversification
            .GetStartAndEndIndexForPart(bestIndex, partSize, maxSize);
        
        Assert.Multiple(() =>
        {
            Assert.AreEqual(0, startEndTuple.Item1);
            Assert.AreEqual(4, startEndTuple.Item2);
        });
    }
}
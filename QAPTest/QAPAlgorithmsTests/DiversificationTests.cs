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
    
    private QAPInstance _testInstance;
    private ScatterSearch _scatterSearch;
    private IImprovementMethod _improvementMethod;
    private ICombinationMethod _combinationMethod;
    private IGenerateInitPopulationMethod _generateInitPopulationMethod;
    private IDiversificationMethod _diversificationMethod;
    private ISolutionGenerationMethod _solutionGenerationMethod;

    [SetUp]
    public void SetUp()
    {
        _testInstance = QAPInstanceProvider.GetTestN3();
        _improvementMethod = new LocalSearchFirstImprovement();
        _improvementMethod.InitMethod(_testInstance);
        _combinationMethod = new ExhaustingPairwiseCombination();
        _combinationMethod.InitMethod(_testInstance);
        _generateInitPopulationMethod = new StepWisePopulationGeneration(1);
        _generateInitPopulationMethod.InitMethod(_testInstance);
        _solutionGenerationMethod = new SubSetGeneration( 1, SubSetGenerationMethodType.Cycle, _combinationMethod, _improvementMethod);
        _solutionGenerationMethod.InitMethod(_testInstance);
        _diversificationMethod = new HashCodeDiversification();
        _scatterSearch = new ScatterSearch(_generateInitPopulationMethod, _diversificationMethod, _combinationMethod, _improvementMethod, _solutionGenerationMethod);

        _hashCodeThreePartDiversification = new HashCodeThreePartDiversification();
        _hashCodeThreePartDiversification.InitMethod(_testInstance);

    }

    [Test]
    public void TestHashCodeDiversificationMethod_Test_RefSetSize()
    {

        
    }
}
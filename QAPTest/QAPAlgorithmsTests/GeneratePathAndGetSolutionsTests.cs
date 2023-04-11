using Domain.Models;
using QAPAlgorithms.ScatterSearch;

namespace QAPTest.QAPAlgorithmsTests;

[TestFixture]
public class GeneratePathAndGetSolutionsTests
{
    private QAPInstance _qapInstance;
    
    [SetUp]
    public async Task SetUp()
    {
        _qapInstance = await QAPInstanceProvider.GetTestN3();
    }

    [Test]
    public void GeneratePathAndGetSolutions_N3Permutation_1NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var solutions = PathRelinking.GeneratePathAndGetSolutions(startSolution, guidingSolution, _qapInstance);
        
        Assert.That(solutions.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void GeneratePathAndGetSolutions_N3Permutation_2NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var solutions = PathRelinking.GeneratePathAndGetSolutions(startSolution, guidingSolution, _qapInstance);
        
        Assert.That(solutions.Count, Is.EqualTo(2));
    }
}
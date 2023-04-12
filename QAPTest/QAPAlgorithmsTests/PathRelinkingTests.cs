using Domain.Models;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPTest.QAPAlgorithmsTests;

[TestFixture]
public class PathRelinkingTests
{
    private QAPInstance _qapInstance;
    private PathRelinking _pathRelinking;
    private ParallelPathRelinking _parallelPathRelinking;

    
    [SetUp]
    public async Task SetUp()
    {
        _qapInstance = await QAPInstanceProvider.GetTestN3();
        _pathRelinking = new PathRelinking();
        _pathRelinking.InitMethod(_qapInstance);
        _parallelPathRelinking = new ParallelPathRelinking();
        _parallelPathRelinking.InitMethod(_qapInstance);
    }

    [Test]
    public void GetSolutions_N3Permutation_1NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _pathRelinking.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(2));
    }
    
    [Test]
    public void GetSolutions_N3Permutation_2NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _pathRelinking.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(4));
    }
    
    [Test]
    public void GetSolutionsParallel_N3Permutation_1NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _parallelPathRelinking.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(2));
    }
    
    [Test]
    public void GetSolutionsParallel_N3Permutation_2NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _parallelPathRelinking.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(4));
    }
}
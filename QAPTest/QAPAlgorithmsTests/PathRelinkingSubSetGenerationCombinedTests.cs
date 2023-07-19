using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPTest.QAPAlgorithmsTests;

public class PathRelinkingSubSetGenerationCombinedTests
{
    private QAPInstance _qapInstance;
    private PathRelinkingSubSetGenerationCombined _pathRelinkingSubSetGenerationCombined;
    private ParallelPathRelinkingSubSetGenerationCombined _parallelPathRelinkingSubSetGenerationCombined;
    
    [SetUp]
    public void SetUp()
    {
        _qapInstance = QAPInstanceProvider.GetTestN3();
        var improvementMethod = new LocalSearchFirstImprovement(); 
        improvementMethod.InitMethod(_qapInstance);
        var combinationMethod = new ExhaustingPairwiseCombination();
        _pathRelinkingSubSetGenerationCombined = new PathRelinkingSubSetGenerationCombined( 1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _pathRelinkingSubSetGenerationCombined.InitMethod(_qapInstance);
        _parallelPathRelinkingSubSetGenerationCombined = new ParallelPathRelinkingSubSetGenerationCombined( 1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _parallelPathRelinkingSubSetGenerationCombined.InitMethod(_qapInstance);
    }

    [Test]
    public void GetSolutions_N3Permutation_4NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _pathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(4));
    }
    
    [Test]
    public void GetSolutions_N3Permutation_4NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _pathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(4));
    }
    
    [Test]
    public void GetSolutionsParallel_N3Permutation_4NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(4));
    }
    
    [Test]
    public void GetSolutionsParallel_N3Permutation_4NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(4));
    }
}
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
    public async Task SetUp()
    {
        _qapInstance = await QAPInstanceProvider.GetTestN3();
        var improvementMethod = new LocalSearchFirstImprovement(_qapInstance); 
        var combinationMethod = new ExhaustingPairwiseCombination();
        _pathRelinkingSubSetGenerationCombined = new PathRelinkingSubSetGenerationCombined(_qapInstance, 1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _parallelPathRelinkingSubSetGenerationCombined = new ParallelPathRelinkingSubSetGenerationCombined(_qapInstance, 1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
    }

    [Test]
    public void GetSolutions_N3Permutation_6NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _pathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(6));
    }
    
    [Test]
    public void GetSolutions_N3Permutation_5NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _pathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(5));
    }
    
    [Test]
    public void GetSolutionsParallel_N3Permutation_6NewSolution()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 1, 0 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(6));
    }
    
    [Test]
    public void GetSolutionsParallel_N3Permutation_5NewSolutions()
    {
        var firstPermutation = new[] {0, 1, 2};
        var startSolution = new InstanceSolution(_qapInstance, firstPermutation);
        
        var secondPermutation = new[] { 2, 0, 1 };
        var guidingSolution = new InstanceSolution(_qapInstance, secondPermutation);

        var referenceSet = new List<InstanceSolution>() { startSolution, guidingSolution };
        
        var solutions = _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(referenceSet);
        
        Assert.That(solutions.Count, Is.EqualTo(5));
    }
}
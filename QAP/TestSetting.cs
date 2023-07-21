using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.DiversificationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAP;

public record TestSetting(
    QAPInstance Instance,
    int PopulationSize,
    int ReferenceSetSize,
    int RunTimeInSeconds,
    ICombinationMethod CombinationMethod,
    IGenerateInitPopulationMethod GenerateInitPopulationMethod,
    IImprovementMethod ImprovementMethod,
    IDiversificationMethod DiversificationMethod,
    ISolutionGenerationMethod SolutionGenerationMethod);

public class TestSettingsProvider
{
    private readonly QAPInstance _instance;
    private readonly int _referenceSetSize;
    private readonly int _populationSize;
    private readonly int _runtimeInSeconds;

    private const int RandomSeed = 42;
    
    public TestSettingsProvider(QAPInstance instance, int referenceSetSize, int populationSize, int runtimeInSeconds)
    {
        _instance = instance;
        _referenceSetSize = referenceSetSize;
        _populationSize = populationSize;
        _runtimeInSeconds = runtimeInSeconds;
    }
    
    /// <summary>
    /// Returns 4 different Test Cases
    /// Generation Method: ParallelRandomGeneratedPopulation
    /// Diversification Method: HashCodeDiversification
    /// Solution Generation Method:
    ///     ParallelPathRelinkingSubSetGenerationCombined
    ///
    /// Test Case 1:
    ///     Combination: DeletionPartsOfTheFirstSolution
    ///                  AndFillWithPartsOfTheOtherSolutions
    ///     Improvement: BestImprovement
    /// Test Case 2:
    ///     Combination: ExhaustingPairwiseCombination
    ///     Improvement: BestImprovement
    /// Test Case 3:
    ///     Combination: DeletionPartsOfTheFirstSolution
    ///                  AndFillWithPartsOfTheOtherSolution
    ///     Improvement: FirstImprovement
    /// Test Case 4:
    ///     Combination: ExhaustingPairwiseCombination
    ///     Improvement: FirstImprovement
    /// </summary>
    /// <returns></returns>
    public List<TestSetting> GetTestSettings()
    {
        var tests = new List<TestSetting>
        {
            TestCase1(),
            TestCase2(),
            TestCase3(),
            TestCase4()
        };

        return tests; 
    }
    
    private TestSetting TestCase1()
    {
        var combinationMethod = new DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(true, 
            50, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeDiversification();
        var improvementMethod = new ParallelImprovedLocalSearchBestImprovement();
        var generationInitPopMethod = new ParallelRandomGeneratedPopulation(RandomSeed);
        var solutionGenerationMethod = new ParallelPathRelinkingSubSetGenerationCombined( 1, SubSetGenerationMethodType.Cycle,
            improvementMethod, combinationMethod, 100);
    
        var testSetting = new TestSetting(
            _instance, 
            _populationSize, 
            _referenceSetSize, 
            _runtimeInSeconds,
            combinationMethod,
            generationInitPopMethod,
            improvementMethod,
            diversificationMethod,
            solutionGenerationMethod);
        return testSetting;
    }

    private TestSetting TestCase2()
    {
        var combinationMethod = new ExhaustingPairwiseCombination(1, 
            0, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeThreePartDiversification();
        var improvementMethod = new ParallelImprovedLocalSearchBestImprovement();
        var generationInitPopMethod = new ParallelRandomGeneratedPopulation(RandomSeed);
        var solutionGenerationMethod = new ParallelPathRelinkingSubSetGenerationCombined( 1, SubSetGenerationMethodType.Cycle,
            improvementMethod, combinationMethod, 100);
    
        var testSetting = new TestSetting(
            _instance, 
            _populationSize, 
            _referenceSetSize, 
            _runtimeInSeconds,
            combinationMethod,
            generationInitPopMethod,
            improvementMethod,
            diversificationMethod,
            solutionGenerationMethod);
        return testSetting;
    }

    private TestSetting TestCase3()
    {
        var combinationMethod = new DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions(true, 
            50, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeDiversification();
        var improvementMethod = new ParallelImprovedLocalSearchFirstImprovement();
        var generationInitPopMethod = new ParallelRandomGeneratedPopulation(RandomSeed);
        var solutionGenerationMethod = new ParallelPathRelinkingSubSetGenerationCombined( 1, SubSetGenerationMethodType.Cycle,
            improvementMethod, combinationMethod, 100);
        
        var testSetting = new TestSetting(
            _instance, 
            _populationSize, 
            _referenceSetSize, 
            _runtimeInSeconds,
            combinationMethod,
            generationInitPopMethod,
            improvementMethod,
            diversificationMethod,
            solutionGenerationMethod);
        return testSetting;
    }

    private TestSetting TestCase4()
    {
        var combinationMethod = new ExhaustingPairwiseCombination(1, 
            0, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeDiversification();
        var improvementMethod = new ParallelImprovedLocalSearchFirstImprovement();
        var generationInitPopMethod = new ParallelRandomGeneratedPopulation(RandomSeed);
        var solutionGenerationMethod = new ParallelPathRelinkingSubSetGenerationCombined( 1, SubSetGenerationMethodType.Cycle,
            improvementMethod, combinationMethod, 100);
        
        var testSetting = new TestSetting(
            _instance, 
            _populationSize, 
            _referenceSetSize, 
            _runtimeInSeconds,
            combinationMethod,
            generationInitPopMethod,
            improvementMethod,
            diversificationMethod,
            solutionGenerationMethod);
        return testSetting;
    }
}


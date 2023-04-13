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
    ISolutionGenerationMethod SolutionGenerationMethod,
    bool DisplayProgressInConsole = false);

public class TestSettingsProvider
{
    private readonly QAPInstance _instance;
    private readonly int _referenceSetSize;
    private readonly int _populationSize;
    private readonly int _runtimeInSeconds;
    
    public TestSettingsProvider(QAPInstance instance, int referenceSetSize, int populationSize, int runtimeInSeconds)
    {
        _instance = instance;
        _referenceSetSize = referenceSetSize;
        _populationSize = populationSize;
        _runtimeInSeconds = runtimeInSeconds;
    }
    
    public List<TestSetting> GetTestSettings()
    {
        var tests = new List<TestSetting>
        {
            GetBaseLine(),
            GetPathRelinking(),
            GetCombinedSolutionGeneration()
        };

        return tests; 
    }

    //Generate Baseline
    private TestSetting GetBaseLine()
    {
        //Generate Baseline
        var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeDiversification();
        var improvementMethod = new ImprovedLocalSearchFirstImprovement();
        var generationInitPopMethod = new RandomGeneratedPopulation(42);
        var solutionGenerationMethod = new SubSetGeneration( 1, SubSetGenerationMethodType.Cycle,
            combinationMethod, improvementMethod);
    
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

    private TestSetting GetPathRelinking()
    {
        var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeDiversification();
        var improvementMethod = new ImprovedLocalSearchFirstImprovement();
        var generationInitPopMethod = new RandomGeneratedPopulation(42);
        var solutionGenerationMethod = new PathRelinking(improvementMethod);
    
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

    private TestSetting GetCombinedSolutionGeneration()
    {
        var combinationMethod = new ExhaustingPairwiseCombination(1, 10, checkIfSolutionsWereAlreadyCombined: true);
        var diversificationMethod = new HashCodeDiversification();
        var improvementMethod = new ImprovedLocalSearchFirstImprovement();
        var generationInitPopMethod = new RandomGeneratedPopulation(42);
        var solutionGenerationMethod = new PathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
    
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


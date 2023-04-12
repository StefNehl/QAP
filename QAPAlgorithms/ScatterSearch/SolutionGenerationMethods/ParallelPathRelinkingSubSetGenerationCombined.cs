using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class ParallelPathRelinkingSubSetGenerationCombined : ISolutionGenerationMethod
{
    private readonly ParallelPathRelinking _pathRelinking;
    private readonly ParallelSubSetGenerationMethod _subSetGeneration;
    
    public ParallelPathRelinkingSubSetGenerationCombined(
        int typeCount, 
        SubSetGenerationMethodType subSetGenerationMethodType, 
        IImprovementMethod improvementMethod, 
        ICombinationMethod combinationMethod)
    {
        _pathRelinking = new ParallelPathRelinking();
        _subSetGeneration = new ParallelSubSetGenerationMethod(typeCount, subSetGenerationMethodType,
            combinationMethod, improvementMethod);
    }

    public void InitMethod(QAPInstance instance)
    {
        _pathRelinking.InitMethod(instance);
        _subSetGeneration.InitMethod(instance);
    }
    
    public List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var solutions = new List<InstanceSolution>();
        
        solutions.AddRange(_subSetGeneration.GetSolutions(referenceSolutions));
        solutions.AddRange(_pathRelinking.GetSolutions(referenceSolutions));

        return solutions;
    }
}
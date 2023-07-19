using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class ParallelPathRelinkingSubSetGenerationCombined : ISolutionGenerationMethod
{
    private readonly ParallelPathRelinking _pathRelinking;
    private readonly ParallelSubSetGeneration _subSetGeneration;
    
    public ParallelPathRelinkingSubSetGenerationCombined(
        int typeCount, 
        SubSetGenerationMethodType subSetGenerationMethodType, 
        IImprovementMethod improvementMethod, 
        ICombinationMethod combinationMethod,
        int pathRelinkingImproveEveryNSolutions = 100)
    {
        _pathRelinking = new ParallelPathRelinking(improvementMethod, pathRelinkingImproveEveryNSolutions);
        _subSetGeneration = new ParallelSubSetGeneration(typeCount, subSetGenerationMethodType,
            combinationMethod, improvementMethod);
    }

    public void InitMethod(QAPInstance instance)
    {
        _pathRelinking.InitMethod(instance);
        _subSetGeneration.InitMethod(instance);
    }
    
    public HashSet<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var solutions = new HashSet<InstanceSolution>();
        
        solutions.UnionWith(_subSetGeneration.GetSolutions(referenceSolutions));
        solutions.UnionWith(_pathRelinking.GetSolutions(referenceSolutions));

        return solutions;
    }
}
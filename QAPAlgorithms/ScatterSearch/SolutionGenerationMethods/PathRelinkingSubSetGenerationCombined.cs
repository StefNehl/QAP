using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class PathRelinkingSubSetGenerationCombined : ISolutionGenerationMethod
{
    private readonly PathRelinking _pathRelinking;
    private readonly SubSetGeneration _subSetGeneration;
    
    public PathRelinkingSubSetGenerationCombined( 
        int typeCount, 
        SubSetGenerationMethodType subSetGenerationMethodType, 
        IImprovementMethod improvementMethod, 
        ICombinationMethod combinationMethod,
        int pathRelinkingImproveEveryNSolutions = 100)
    {
        _pathRelinking = new PathRelinking(improvementMethod, pathRelinkingImproveEveryNSolutions);
        _subSetGeneration = new SubSetGeneration(typeCount, subSetGenerationMethodType,
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
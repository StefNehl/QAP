using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class PathRelinkingSubSetGenerationCombined : ISolutionGenerationMethod
{
    private PathRelinking _pathRelinking;
    private SubSetGenerationMethod _subSetGeneration;
    
    public PathRelinkingSubSetGenerationCombined(QAPInstance qapInstance, 
        int typeCount, 
        SubSetGenerationMethodType subSetGenerationMethodType, 
        IImprovementMethod improvementMethod, 
        ICombinationMethod combinationMethod)
    {
        _pathRelinking = new PathRelinking(qapInstance);
        _subSetGeneration = new SubSetGenerationMethod(qapInstance, typeCount, subSetGenerationMethodType,
            combinationMethod, improvementMethod);
    }
    public List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var solutions = new List<InstanceSolution>();
        
        solutions.AddRange(_subSetGeneration.GetSolutions(referenceSolutions));
        solutions.AddRange(_pathRelinking.GetSolutions(referenceSolutions));

        return solutions;
    }
}
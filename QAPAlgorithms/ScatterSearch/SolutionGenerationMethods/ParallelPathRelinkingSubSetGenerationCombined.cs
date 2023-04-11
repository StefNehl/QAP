using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class ParallelPathRelinkingSubSetGenerationCombined
{
    private ParallelPathRelinking _pathRelinking;
    private ParallelSubSetGenerationMethod _subSetGeneration;
    
    public ParallelPathRelinkingSubSetGenerationCombined(QAPInstance qapInstance, 
        int typeCount, 
        SubSetGenerationMethodType subSetGenerationMethodType, 
        IImprovementMethod improvementMethod, 
        ICombinationMethod combinationMethod)
    {
        _pathRelinking = new ParallelPathRelinking(qapInstance);
        _subSetGeneration = new ParallelSubSetGenerationMethod(qapInstance, typeCount, subSetGenerationMethodType,
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
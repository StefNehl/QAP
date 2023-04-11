using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class PathRelinking : ISolutionGenerationMethod
{
    private QAPInstance _qapInstance;

    public PathRelinking(QAPInstance qapInstance)
    {
        _qapInstance = qapInstance;
    }
    
    public List<InstanceSolution> GeneratePathAndGetSolutions(InstanceSolution startingSolution, InstanceSolution guidingSolution)
    {
        var newSolutions = new List<InstanceSolution>();

        var newHashCode = startingSolution.HashCode;
        var newPermutation = startingSolution.SolutionPermutation.ToArray();
        while (newHashCode != guidingSolution.HashCode)
        {
            AddAttributeToSolutionFromGuidingSolution(newPermutation, guidingSolution.SolutionPermutation);
            var newSolution = new InstanceSolution(_qapInstance, newPermutation.ToArray());
            newHashCode = newSolution.HashCode;
            newSolutions.Add(newSolution);
        }
        
        return newSolutions;
    }

    private static void AddAttributeToSolutionFromGuidingSolution(int[] permutation, int[] guidingPermutation)
    {
        var notCorrectIndices = new List<int>();
        for (int i = 0; i < permutation.Length; i++)
        {
            if (permutation[i] == guidingPermutation[i])
                continue;
            
            notCorrectIndices.Add(i);
        }

        var indexForSwap = notCorrectIndices[0];
        var correctValueForIndex = guidingPermutation[indexForSwap];
        var indexOfCorrectValueInPermutation = -1;

        foreach (var notCorrectIndex in notCorrectIndices)
        {
            if (correctValueForIndex != permutation[notCorrectIndex])
                continue;
                
            indexOfCorrectValueInPermutation = notCorrectIndex;
            break;
        }
        
        (permutation[indexForSwap], permutation[indexOfCorrectValueInPermutation]) = (permutation[indexOfCorrectValueInPermutation], permutation[indexForSwap]);
    }


    public List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var newSolutions = new List<InstanceSolution>();
        
        for(int i = 0; i < referenceSolutions.Count; i++)
        {
            for (int j = i; j < referenceSolutions.Count; j++)
            {
                newSolutions.AddRange(GeneratePathAndGetSolutions(referenceSolutions[i], referenceSolutions[j]));
                newSolutions.AddRange(GeneratePathAndGetSolutions(referenceSolutions[j], referenceSolutions[i]));
            }
        }

        return newSolutions;
    }
}
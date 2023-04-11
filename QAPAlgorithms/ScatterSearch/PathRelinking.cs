using Domain;
using Domain.Models;

namespace QAPAlgorithms.ScatterSearch;

public class PathRelinking
{
    public static List<InstanceSolution> GeneratePathAndGetSolutions(InstanceSolution startingSolution, InstanceSolution guidingSolution, QAPInstance instance)
    {
        var newSolutions = new List<InstanceSolution>();

        var newHashCode = startingSolution.HashCode;
        var newPermutation = startingSolution.SolutionPermutation.ToArray();
        while (newHashCode != guidingSolution.HashCode)
        {
            AddAttributeToSolutionFromGuidingSolution(newPermutation, guidingSolution.SolutionPermutation);
            var newSolution = new InstanceSolution(instance, newPermutation.ToArray());
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
    
    
}
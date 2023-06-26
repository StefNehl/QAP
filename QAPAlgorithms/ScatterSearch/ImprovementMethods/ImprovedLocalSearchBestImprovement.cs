using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods;

public class ImprovedLocalSearchBestImprovement  : IImprovementMethod
{
    private QAPInstance _instance;

    public void InitMethod(QAPInstance instance)
    {
        _instance = instance;
    }
    
    public InstanceSolution ImproveSolution(InstanceSolution instanceSolution)
    {
        var permutation = instanceSolution.SolutionPermutation.ToArray();
        //Tuple (SolutionValue, startIndexForExchange)
        var solutionValues = new List<Tuple<long, int>>();
        var oldSolutionValue = instanceSolution.SolutionValue;

        for (int i = 0; i < permutation.Length - 1; i++)
        {
            var solutionDifference = InstanceHelpers.GetSolutionDifferenceAfterSwap(_instance, permutation, i, i + 1);
            var newSolutionValue = oldSolutionValue + solutionDifference;
            
            if (InstanceHelpers.IsBetterSolution(oldSolutionValue, newSolutionValue))
                solutionValues.Add(new Tuple<long, int>(newSolutionValue, i));
        }

        long minValue = long.MaxValue;
        var minValueIndex = -1;
        for (int i = 0; i < solutionValues.Count; i++)
        {
            if (solutionValues[i].Item1 < minValue)
            {
                minValue = solutionValues[i].Item1;
                minValueIndex = solutionValues[i].Item2;
            }
        }

        if (minValueIndex > -1)
        {
            (instanceSolution.SolutionPermutation[minValueIndex + 1], instanceSolution.SolutionPermutation[minValueIndex]) =
                (instanceSolution.SolutionPermutation[minValueIndex], instanceSolution.SolutionPermutation[minValueIndex + 1]);
            instanceSolution.SolutionValue = minValue;
            instanceSolution.RefreshHashCode();
        }

        return instanceSolution;
    }

    public void ImproveSolutions(List<InstanceSolution> instanceSolutions)
    {
        for (int i = 0; i < instanceSolutions.Count; i++)
            instanceSolutions[i] = ImproveSolution(instanceSolutions[i]);
    }


}
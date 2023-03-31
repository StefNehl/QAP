﻿using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods;

public class ImprovedLocalSearchBestImprovement  : IImprovementMethod
{
    private readonly QAPInstance _instance;
    public ImprovedLocalSearchBestImprovement(QAPInstance qAPInstance)
    {
        _instance = qAPInstance;
    }
    
    public void ImproveSolution(IInstanceSolution instanceSolution)
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
                minValueIndex = i;
            }
        }

        if (minValueIndex > -1)
        {
            (instanceSolution.SolutionPermutation[minValueIndex + 1], instanceSolution.SolutionPermutation[minValueIndex]) =
                (instanceSolution.SolutionPermutation[minValueIndex], instanceSolution.SolutionPermutation[minValueIndex + 1]);
            instanceSolution.RefreshSolutionValue(_instance);
        }
    } 
    
    public void ImproveSolutions(List<IInstanceSolution> instanceSolutions)
    {
        foreach (var solution in instanceSolutions)
            ImproveSolution(solution);
    }

    public async Task ImproveSolutionsInParallelAsync(List<IInstanceSolution> instanceSolutions, CancellationToken ct)
    {
        if (instanceSolutions.Count <= 5)
        {
            ImproveSolutions(instanceSolutions);
            return;
        }

        var tasksToRun = instanceSolutions.Select(s => Task.Factory.StartNew(() => ImproveSolution(s), ct));
        await Task.WhenAll(tasksToRun);
    }
}
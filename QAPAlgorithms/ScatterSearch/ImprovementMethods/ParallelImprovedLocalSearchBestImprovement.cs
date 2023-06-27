﻿using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods;

public class ParallelImprovedLocalSearchBestImprovement : ImprovedLocalSearchBestImprovement, IImprovementMethod
{
    public new void ImproveSolutions(List<InstanceSolution> instanceSolutions)
    {
        if (instanceSolutions.Count <= 5)
        {
            base.ImproveSolutions(instanceSolutions);
            return;
        }

        var taskList = new List<Task>();
        for (int i = 0; i < instanceSolutions.Count; i++)
        {
            var i1 = i;
            var newTask = Task.Factory.StartNew(() => instanceSolutions[i1] =  ImproveSolution(instanceSolutions[i1]));
            taskList.Add(newTask);
        }
        Task.WhenAll(taskList).Wait();
    }
}
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods;

public class ParallelImprovedLocalSearchFirstImprovement : ImprovedLocalSearchFirstImprovement, IImprovementMethod
{
    public new void ImproveSolutions(List<InstanceSolution> instanceSolutions)
    {
        if (instanceSolutions.Count <= 5)
        {
            ImproveSolutions(instanceSolutions);
            return;
        }

        var taskList = new List<Task>();
        for (int i = 0; i < instanceSolutions.Count; i++)
        {
            var i1 = i;
            var newTask = Task.Factory.StartNew(() => ImproveSolution(instanceSolutions[i1]));
            taskList.Add(newTask);
        }
        Task.WhenAll(taskList).Wait();
    }
}
using System.Collections.Concurrent;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class ParallelPathRelinking : PathRelinking, ISolutionGenerationMethod
{
    private readonly ConcurrentBag<InstanceSolution> _newSolutions;
    public ParallelPathRelinking()
    {
        _newSolutions = new ConcurrentBag<InstanceSolution>();
    }

    public new List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var taskList = new List<Task>();

        for(int i = 0; i < referenceSolutions.Count; i++)
        {
            for (int j = i; j < referenceSolutions.Count; j++)
            {
                var i1 = i;
                var j1 = j;
                var task = Task.Factory.StartNew(() =>
                {
                    GenerateTwoPath(i1, j1, referenceSolutions);
                });
                taskList.Add(task);
            }
        }

        Task.WhenAll(taskList).Wait();
        return _newSolutions.ToList();
    }

    private void GenerateTwoPath(int i, int j, IReadOnlyList<InstanceSolution> referenceSolutions)
    {
        AddRangeConcurrent(GeneratePathAndGetSolutions(referenceSolutions[i], referenceSolutions[j]));
        AddRangeConcurrent(GeneratePathAndGetSolutions(referenceSolutions[j], referenceSolutions[i]));
    }

    private void AddRangeConcurrent(List<InstanceSolution> solutionsToAdd)
    {
        foreach (var instanceSolution in solutionsToAdd)
        {
            _newSolutions.Add(instanceSolution);
        }
    }
    
}
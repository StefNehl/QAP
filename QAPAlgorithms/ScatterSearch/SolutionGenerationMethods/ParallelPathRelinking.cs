using System.Collections.Concurrent;
using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

public class ParallelPathRelinking : PathRelinking, ISolutionGenerationMethod
{
    private readonly ConcurrentDictionary<InstanceSolution, int> _newSolutions;
    public ParallelPathRelinking(IImprovementMethod improvementMethod, int improveEveryNSolutions = 100) : base(improvementMethod, improveEveryNSolutions)
    {
        _newSolutions = new ConcurrentDictionary<InstanceSolution, int>();
    }

    public new HashSet<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var taskList = new List<Task>();
        _newSolutions.Clear();

        for(int i = 0; i < referenceSolutions.Count; i++)
        {
            for (int j = (i + 1); j < referenceSolutions.Count; j++)
            {
                var i1 = i;
                var j1 = j;

                var task = Task.Factory.StartNew(() =>
                {
                    GenerateTwoPaths(i1, j1, referenceSolutions);
                });
                taskList.Add(task);
            }
        }

        Task.WhenAll(taskList).Wait();
        return _newSolutions.Keys.ToHashSet();
    }

    private void GenerateTwoPaths(int i, int j, IReadOnlyList<InstanceSolution> referenceSolutions)
    {
        AddRangeConcurrent(GeneratePathAndGetSolutions(referenceSolutions[i], referenceSolutions[j]));
        AddRangeConcurrent(GeneratePathAndGetSolutions(referenceSolutions[j], referenceSolutions[i]));
    }

    private void AddRangeConcurrent(List<InstanceSolution> solutionsToAdd)
    {
        foreach (var instanceSolution in solutionsToAdd)
        {
            _newSolutions.TryAdd(instanceSolution, 0);
        }
    }
    
}
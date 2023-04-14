using System.Collections.Concurrent;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.InitGenerationMethods;

public class ParallelRandomGeneratedPopulation : IGenerateInitPopulationMethod
{
    private readonly ConcurrentBag<InstanceSolution> _newSolutions = new ();
    
    private QAPInstance? _qApInstance;
    private int[]? _permutation;
        
    private readonly Random _randomGenerator;

    public ParallelRandomGeneratedPopulation(
        int? seed = null)
    {
        _randomGenerator = seed.HasValue ? new Random(Seed: seed.Value) : new Random();
    }

    public void InitMethod(QAPInstance instance)
    {
        _qApInstance = instance;
        _permutation = new int[_qApInstance.N];
    }

    public new List<InstanceSolution> GeneratePopulation(int populationSize)
    {
        var taskList = new List<Task>();
        _newSolutions.Clear();
        
        for(int i = 0; i < populationSize; i++)
        {
            var task = Task.Factory.StartNew(() =>
            {
                _newSolutions.Add(GenerateSolutionThreadSafe());
            });
            taskList.Add(task);
        }
        
        Task.WhenAll(taskList).Wait();
        return _newSolutions.ToList();
    }
    
    private InstanceSolution GenerateSolutionThreadSafe()
    {
        var listWithPossibilities = new List<int>();
        for (int i = 0; i < _permutation.Length; i++)
            listWithPossibilities.Add(i);
                
        for (int i = 0; i < _permutation.Length; i++)
        {
            var newRandomIndex = _randomGenerator.Next(listWithPossibilities.Count - 1);
            _permutation[i] = listWithPossibilities[newRandomIndex];
            listWithPossibilities.RemoveAt(newRandomIndex);
        }

        return new InstanceSolution(_qApInstance, _permutation.ToArray());
    }
    
}
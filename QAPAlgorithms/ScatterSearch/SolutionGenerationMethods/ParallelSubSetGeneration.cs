﻿using System.Collections.Concurrent;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods
{
    public class ParallelSubSetGeneration : ISolutionGenerationMethod
    {
        private readonly ICombinationMethod _combinationMethod;
        private readonly IImprovementMethod _improvementMethod;
        private readonly SubSetGenerationMethodType _subSetGenerationMethodType;

        private QAPInstance? _qapInstance;
        private int _typeCount;

        public ParallelSubSetGeneration(
            int typeCount,
            SubSetGenerationMethodType subSetGenerationMethodType,
            ICombinationMethod combinationMethod, 
            IImprovementMethod improvementMethod)
        {
            _combinationMethod = combinationMethod;
            _improvementMethod = improvementMethod;

            _typeCount = typeCount;
            _subSetGenerationMethodType = subSetGenerationMethodType;
        }

        public void InitMethod(QAPInstance instance)
        {
            _qapInstance = instance;
        }

        /// <summary>
        /// 18_P.27
        /// Hot Path https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md#prefer-asyncawait-over-directly-returning-task
        /// Directly return the task for performance
        /// </summary>
        /// <param name="referenceSolutions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<List<InstanceSolution>> GenerateType1SubSetAsync(List<InstanceSolution> referenceSolutions, CancellationToken ct = default)
        {
            var listForSubSets = new List<InstanceSolution>();
            return GetSolutionForSubSetsAsync(referenceSolutions, listForSubSets, 0, ct);
        }

        /// <summary>
        /// 18_P.27
        /// </summary>
        /// <param name="referenceSolutions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public  Task<List<InstanceSolution>> GenerateType2SubSetAsync(List<InstanceSolution> referenceSolutions, CancellationToken ct = default)
        {
            var listForSubSets = new List<InstanceSolution>
            {
                referenceSolutions.First()
            };

            return GetSolutionForSubSetsAsync(referenceSolutions, listForSubSets, 1, ct);
        }

        /// <summary>
        /// 18_P.28
        /// </summary>
        /// <param name="referenceSolutions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<List<InstanceSolution>> GenerateType3SubSetAsync(List<InstanceSolution> referenceSolutions, CancellationToken ct = default)
        {
            var listForSubSets = new List<InstanceSolution>
            {
                referenceSolutions.First(),
                referenceSolutions.ElementAt(1)
            };

            return GetSolutionForSubSetsAsync(referenceSolutions, listForSubSets, 2, ct);
        }

        /// <summary>
        /// 18_P.28
        /// </summary>
        /// <param name="referenceSolutions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<InstanceSolution>> GenerateType4SubSetAsync(List<InstanceSolution> referenceSolutions, CancellationToken ct = default)
        {
            var result = new List<InstanceSolution>();
            var listForSubSets = new List<InstanceSolution>();

            for(int i = 0; i < referenceSolutions.Count; i++)
            {
                listForSubSets.Add(referenceSolutions.ElementAt(i));
                if(i >= 4)
                {
                    var newTrialPermutations = _combinationMethod.CombineSolutions(listForSubSets);
                    var newTrialSolutions = CreateSolutions(newTrialPermutations);
                    await _improvementMethod.ImproveSolutionsInParallelAsync(newTrialSolutions, ct);
                    result.AddRange(newTrialSolutions);
                }
            }

            return result;
        }

        private async Task<List<InstanceSolution>> GetSolutionForSubSetsAsync(
            List<InstanceSolution> referenceSolutions,
            List<InstanceSolution> listForSubSets,
            int startIndex, 
            CancellationToken ct = default)
        {
            var result = new ConcurrentBag<InstanceSolution>();
            var tasksToFinish = new List<Task>();

            for (int i = startIndex; i < referenceSolutions.Count - 1; i++)
            {
                listForSubSets.Add(referenceSolutions.ElementAt(i));
                for (int j = i + 1; j < referenceSolutions.Count; j++)
                {
                    listForSubSets.Add(referenceSolutions.ElementAt(j));

                    var copyOfList = listForSubSets.ToList();
                    var newTask = Task.Factory.StartNew(async () =>
                    {
                        var newTrialSolutions = await CreateNewSolutionsFromSolutionsAsync(copyOfList, ct);
                        foreach (var solution in newTrialSolutions)
                            result.Add(solution);
                    }, ct);
                    tasksToFinish.Add(newTask);

                    listForSubSets.Remove(referenceSolutions.ElementAt(j));
                }
                listForSubSets.Remove(referenceSolutions.ElementAt(i));
            }

            await Task.WhenAll(tasksToFinish);
            return result.ToList();
        }

        private async Task<List<InstanceSolution>> CreateNewSolutionsFromSolutionsAsync(List<InstanceSolution> solutions, CancellationToken ct)
        {
            var newTrialPermutations = _combinationMethod.CombineSolutionsThreadSafe(solutions);
            var newTrialSolutions = CreateSolutions(newTrialPermutations);
            await _improvementMethod.ImproveSolutionsInParallelAsync(newTrialSolutions, ct);
            return newTrialSolutions;
        }

        private List<InstanceSolution> CreateSolutions(List<int[]> newPermutations)
        {
            var result = new List<InstanceSolution>();

            foreach (var permutation in newPermutations)
            {
                var newSolution = new InstanceSolution(_qapInstance, permutation);
                result.Add(newSolution);
            }

            return result;
        }

        public List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
        {
            return GetSolutionsAsync(referenceSolutions).Result;
        }

        private async Task<List<InstanceSolution>> GetSolutionsAsync(List<InstanceSolution> referenceSolutions)
        {
            var newSubSets = new List<InstanceSolution>();
            switch (_typeCount)
            {
                case 1:
                    newSubSets.AddRange(await GenerateType1SubSetAsync(referenceSolutions));
                    break;
                case 2:
                    newSubSets.AddRange(await GenerateType2SubSetAsync(referenceSolutions));
                    break;
                case 3:
                    newSubSets.AddRange(await GenerateType3SubSetAsync(referenceSolutions));
                    break;
                case 4:
                    newSubSets.AddRange(await GenerateType4SubSetAsync(referenceSolutions));
                    _typeCount = 0;
                    break;
            }
            
            if (_subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                _typeCount++;
            
            return newSubSets;
        }
    }
}

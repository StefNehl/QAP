using Domain.Models;
using QAPAlgorithms.Contracts;
using System.Collections.Concurrent;

namespace QAPAlgorithms.ScatterSearch
{
    public class ParallelSubSetGenerationMethod
    {
        private readonly ICombinationMethod _combinationMethod;
        private readonly IImprovementMethod _improvementMethod;
        private readonly QAPInstance _qapInstance;


        public ParallelSubSetGenerationMethod(QAPInstance qapInstance,
            ICombinationMethod combinationMethod, 
            IImprovementMethod improvementMethod)
        {
            _qapInstance = qapInstance;
            _combinationMethod = combinationMethod;
            _improvementMethod = improvementMethod;
        }

        /// <summary>
        /// 18_P.27
        /// Hot Path https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md#prefer-asyncawait-over-directly-returning-task
        /// Directly return the task for performance
        /// </summary>
        /// <param name="referenceSolutions"></param>
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
        /// <returns></returns>
        public async Task<List<InstanceSolution>> GenerateType4SubSetAsync(List<InstanceSolution> referenceSolutions, CancellationToken ct)
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
    }
}

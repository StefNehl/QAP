using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalSearchBestImprovement : IImprovementMethod
    {
        private readonly QAPInstance instance;
        public LocalSearchBestImprovement(QAPInstance qAPInstance)
        {
            instance = qAPInstance;
        }

        public void ImproveSolution(IInstanceSolution instanceSolution)
        {
            var permutation = instanceSolution.SolutionPermutation.ToArray();

            //Tuple (SolutionValue, startIndexForExchange)
            var solutionValues = new List<Tuple<long, int>>();
            var oldSolutionValue = instanceSolution.SolutionValue;

            for (int i = 0; i < permutation.Length - 1; i++)
            {
                int backUpFirstItem = permutation[i];
                int backUpSecondItem = permutation[i + 1];
                permutation[i] = backUpSecondItem;
                permutation[i + 1] = backUpFirstItem;

                var newSolutionValue = InstanceHelpers.GetSolutionValue(instance, permutation);
                if (InstanceHelpers.IsBetterSolution(oldSolutionValue, newSolutionValue))
                    solutionValues.Add(new Tuple<long, int>(newSolutionValue, i));

                permutation[i] = backUpFirstItem;
                permutation[i + 1] = backUpSecondItem;
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
                instanceSolution.RefreshSolutionValue(instance);
            }
        }

        public void ImproveSolutions(List<IInstanceSolution> instanceSolutions)
        {
            throw new Exception("Don't use this Method. Slower than parallel method");
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
        
        public async Task ImproveSolutionsInParallelAsync_WaitAll(List<IInstanceSolution> instanceSolutions, CancellationToken ct)
        {
            throw new Exception("Don't use this Method. Slower than parallel method");
            if (instanceSolutions.Count <= 10)
            {
                ImproveSolutions(instanceSolutions);
                return;
            }

            var tasksToRun = instanceSolutions.Select(s => Task.Factory.StartNew(() => ImproveSolution(s), ct));
            await Task.Run(() => Task.WaitAll(tasksToRun.ToArray()), ct);
        }
        
        public async Task ImproveSolutionsInParallelAsync_Parallel(List<IInstanceSolution> instanceSolutions, CancellationToken ct)
        {
            throw new Exception("Don't use this Method. Slower than parallel method");
            if (instanceSolutions.Count <= 10)
            {
                ImproveSolutions(instanceSolutions);
                return;
            }

            await Task.Run(() => Parallel.ForEach(instanceSolutions, ImproveSolution), ct);
        }
    }
}

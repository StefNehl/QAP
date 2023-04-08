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

        public InstanceSolution ImproveSolution(InstanceSolution instanceSolution)
        {
            //Tuple (SolutionValue, startIndexForExchange)
            var solutionValues = new List<Tuple<long, int>>();
            var oldSolutionValue = instanceSolution.SolutionValue;

            for (int i = 0; i < instanceSolution.SolutionPermutation.Length - 1; i++)
            {
                int backUpFirstItem = instanceSolution.SolutionPermutation[i];
                int backUpSecondItem = instanceSolution.SolutionPermutation[i + 1];
                instanceSolution.SolutionPermutation[i] = backUpSecondItem;
                instanceSolution.SolutionPermutation[i + 1] = backUpFirstItem;

                var newSolutionValue = InstanceHelpers.GetSolutionValue(instance, instanceSolution.SolutionPermutation);
                if (InstanceHelpers.IsBetterSolution(oldSolutionValue, newSolutionValue))
                    solutionValues.Add(new Tuple<long, int>(newSolutionValue, i));

                instanceSolution.SolutionPermutation[i] = backUpFirstItem;
                instanceSolution.SolutionPermutation[i + 1] = backUpSecondItem;
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
                instanceSolution.RefreshSolutionValue(instance);
            }

            return instanceSolution;
        }

        public void ImproveSolutions(List<InstanceSolution> instanceSolutions)
        {
            for (int i = 0; i < instanceSolutions.Count; i++)
                instanceSolutions[i] = ImproveSolution(instanceSolutions[i]);
        }

        public async Task ImproveSolutionsInParallelAsync(List<InstanceSolution> instanceSolutions, CancellationToken ct)
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
                var newTask = Task.Factory.StartNew(() => instanceSolutions[i1] = ImproveSolution(instanceSolutions[i1]), ct);
                taskList.Add(newTask);
            }
            await Task.WhenAll(taskList);
        }
        
        public async Task ImproveSolutionsInParallelAsync_WaitAll(List<InstanceSolution> instanceSolutions, CancellationToken ct)
        {
            throw new Exception("Don't use this Method. Slower than parallel method");
            // if (instanceSolutions.Count <= 10)
            // {
            //     ImproveSolutions(instanceSolutions);
            //     return;
            // }
            //
            // var tasksToRun = instanceSolutions.Select(s => Task.Factory.StartNew(() => ImproveSolution(s), ct));
            // await Task.Run(() => Task.WaitAll(tasksToRun.ToArray()), ct);
        }
        
        public async Task ImproveSolutionsInParallelAsync_Parallel(List<InstanceSolution> instanceSolutions, CancellationToken ct)
        {
            throw new Exception("Don't use this Method. Slower than parallel method");
            // if (instanceSolutions.Count <= 10)
            // {
            //     ImproveSolutions(instanceSolutions);
            //     return;
            // }
            //
            // await Task.Run(() => Parallel.ForEach(instanceSolutions, ImproveSolution), ct);
        }
    }
}

using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods
{
    /// <summary>
    /// Switches the values in the permutation pair wise. Updates the solution as soon as a better solution is found
    /// </summary>
    public class LocalSearchFirstImprovement : IImprovementMethod
    {
        private readonly QAPInstance _instance;
        public LocalSearchFirstImprovement(QAPInstance qAPInstance)
        {
            _instance = qAPInstance;
        }
        public void ImproveSolution(IInstanceSolution instanceSolution)
        {
            var permutation = instanceSolution.SolutionPermutation;
            var solutionValue = instanceSolution.SolutionValue;
            for (int i = 0; i < permutation.Length - 1; i++)
            {
                int backUpFirstItem = permutation[i];
                int backUpSecondItem = permutation[i + 1];
                permutation[i] = backUpSecondItem;
                permutation[i + 1] = backUpFirstItem;

                //ToDo
                //Improve new calculation of the Value Erenda Cela p.77
                instanceSolution.RefreshSolutionValue(_instance);
                var newSolutionValue = instanceSolution.SolutionValue;
                if (InstanceHelpers.IsBetterSolution(solutionValue, newSolutionValue))
                {
                    break;
                }
                permutation[i] = backUpFirstItem;
                permutation[i + 1] = backUpSecondItem;
            }

        }

        public void ImproveSolutions(List<IInstanceSolution> instanceSolutions)
        {
            foreach (var solution in instanceSolutions)
                ImproveSolution(solution);
        }

        public async Task ImproveSolutionsInParallelAsync(List<IInstanceSolution> instanceSolutions, CancellationToken ct)
        {
            if (instanceSolutions.Count <= 10)
            {
                ImproveSolutions(instanceSolutions);
                return;
            }

            await Parallel.ForEachAsync(instanceSolutions, async (i, ct) =>
            {
                ImproveSolution(i);
                await Task.CompletedTask;
            });

        }
    }
}

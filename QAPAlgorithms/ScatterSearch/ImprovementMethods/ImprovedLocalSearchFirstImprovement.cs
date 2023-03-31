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
    public class ImprovedLocalSearchFirstImprovement : IImprovementMethod
    {
        private readonly QAPInstance instance;
        public ImprovedLocalSearchFirstImprovement(QAPInstance qAPInstance)
        {
            instance = qAPInstance;
        }
        public void ImproveSolution(IInstanceSolution instanceSolution)
        {
            var permutation = instanceSolution.SolutionPermutation;
            var solutionValue = instanceSolution.SolutionValue;
            for (int i = 0; i < permutation.Length - 1; i++)
            {
                var solutionDifference = InstanceHelpers.GetSolutionDifferenceAfterSwap(instance, permutation, i, i + 1);
                var newSolutionValue = solutionValue + solutionDifference;
                if (!InstanceHelpers.IsBetterSolution(solutionValue, newSolutionValue))
                    break;
                
                (instanceSolution.SolutionPermutation[i + 1], instanceSolution.SolutionPermutation[i]) =
                    (instanceSolution.SolutionPermutation[i], instanceSolution.SolutionPermutation[i + 1]);
                instanceSolution.SolutionValue = newSolutionValue;
            }

        }

        public void ImproveSolutions(List<IInstanceSolution> instanceSolutions)
        {
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
    }
}

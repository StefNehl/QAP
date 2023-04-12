using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods
{
    /// <summary>
    /// Switches the values in the permutation pair wise. Updates the solution as soon as a better solution is found
    /// </summary>
    public class ImprovedLocalSearchFirstImprovement : IImprovementMethod
    {
        private QAPInstance? _instance;
        
        public void InitMethod(QAPInstance instance)
        {
            _instance = instance;
        }
        
        public InstanceSolution ImproveSolution(InstanceSolution instanceSolution)
        {
            var permutation = instanceSolution.SolutionPermutation;
            var solutionValue = instanceSolution.SolutionValue;
            for (int i = 0; i < permutation.Length - 1; i++)
            {
                var solutionDifference = InstanceHelpers.GetSolutionDifferenceAfterSwap(_instance, permutation, i, i + 1);
                var newSolutionValue = solutionValue + solutionDifference;
                if (!InstanceHelpers.IsBetterSolution(solutionValue, newSolutionValue))
                    break;
                
                (instanceSolution.SolutionPermutation[i + 1], instanceSolution.SolutionPermutation[i]) =
                    (instanceSolution.SolutionPermutation[i], instanceSolution.SolutionPermutation[i + 1]);
                instanceSolution.SolutionValue = newSolutionValue;
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
    }
}

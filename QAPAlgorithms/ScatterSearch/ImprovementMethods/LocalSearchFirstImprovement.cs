using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods
{
    /// <summary>
    /// Switches the values in the permutation pair wise. Updates the solution as soon as a better solution is found
    /// </summary>
    public class LocalSearchFirstImprovement : IImprovementMethod
    {
        private QAPInstance? _instance;
        
        public void InitMethod(QAPInstance instance)
        {
            _instance = instance;
        }
        
        public InstanceSolution ImproveSolution(InstanceSolution instanceSolution)
        {
            var solutionValue = instanceSolution.SolutionValue;
            for (int i = 0; i < instanceSolution.SolutionPermutation.Length - 1; i++)
            {
                int backUpFirstItem = instanceSolution.SolutionPermutation[i];
                int backUpSecondItem = instanceSolution.SolutionPermutation[i + 1];
                instanceSolution.SolutionPermutation[i] = backUpSecondItem;
                instanceSolution.SolutionPermutation[i + 1] = backUpFirstItem;
                
                instanceSolution.RefreshSolutionValue(_instance);
                var newSolutionValue = instanceSolution.SolutionValue;
                if (InstanceHelpers.IsBetterSolution(solutionValue, newSolutionValue))
                {
                    instanceSolution.RefreshHashCode();
                    break;
                }
                instanceSolution.SolutionPermutation[i] = backUpFirstItem;
                instanceSolution.SolutionPermutation[i + 1] = backUpSecondItem;
            }

            return instanceSolution;
        }
        


        public void ImproveSolutions(List<InstanceSolution> instanceSolutions)
        {
            for (int i = 0; i < instanceSolutions.Count; i++)
                instanceSolutions[i] = ImproveSolution(instanceSolutions[i]);
        }
    }
}

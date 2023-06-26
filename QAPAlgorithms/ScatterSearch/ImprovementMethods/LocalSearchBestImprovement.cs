using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.ImprovementMethods
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalSearchBestImprovement : IImprovementMethod
    {
        private QAPInstance? _instance;
        
        public void InitMethod(QAPInstance instance)
        {
            _instance = instance;
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

                var newSolutionValue = InstanceHelpers.GetSolutionValue(_instance, instanceSolution.SolutionPermutation);
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
                instanceSolution.RefreshSolutionValue(_instance);
                instanceSolution.RefreshHashCode();
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

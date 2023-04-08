using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch
{
    public class SubSetGenerationMethod
    {
        private readonly ICombinationMethod combinationMethod;
        private readonly IImprovementMethod improvementMethod;
        private readonly QAPInstance qapInstance;


        public SubSetGenerationMethod(QAPInstance qapInstance,
            ICombinationMethod combinationMethod,
            IImprovementMethod improvementMethod)
        {
            this.qapInstance = qapInstance;
            this.combinationMethod = combinationMethod;
            this.improvementMethod = improvementMethod;
        }

        /// <summary>
        /// 18_P.27
        /// Hot Path https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md#prefer-asyncawait-over-directly-returning-task
        /// Directly return the task for performance
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public List<InstanceSolution> GenerateType1SubSet(List<InstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<InstanceSolution>();
            return GetSolutionForSubSets(referenceSolutions, listForSubSets, 0);
        }

        /// <summary>
        /// 18_P.27
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public List<InstanceSolution> GenerateType2SubSet(List<InstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<InstanceSolution>
            {
                referenceSolutions.First()
            };

            return GetSolutionForSubSets(referenceSolutions, listForSubSets, 1);
        }

        /// <summary>
        /// 18_P.28
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public List<InstanceSolution> GenerateType3SubSet(List<InstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<InstanceSolution>
            {
                referenceSolutions.First(),
                referenceSolutions.ElementAt(1)
            };

            return GetSolutionForSubSets(referenceSolutions, listForSubSets, 2);
        }

        /// <summary>
        /// 18_P.28
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public List<InstanceSolution> GenerateType4SubSet(List<InstanceSolution> referenceSolutions)
        {
            var result = new List<InstanceSolution>();
            var listForSubSets = new List<InstanceSolution>();

            for (int i = 0; i < referenceSolutions.Count; i++)
            {
                listForSubSets.Add(referenceSolutions.ElementAt(i));
                if (i >= 4)
                {
                    var newTrialPermutations = combinationMethod.CombineSolutions(listForSubSets);
                    var newTrialSolutions = CreateSolutions(newTrialPermutations);
                    improvementMethod.ImproveSolutions(newTrialSolutions);
                    result.AddRange(newTrialSolutions);
                }
            }

            return result;
        }

        private List<InstanceSolution> GetSolutionForSubSets(
            List<InstanceSolution> referenceSolutions,
            List<InstanceSolution> listForSubSets,
            int startIndex)
        {
            var result = new List<InstanceSolution>();

            for (int i = startIndex; i < referenceSolutions.Count - 1; i++)
            {
                listForSubSets.Add(referenceSolutions.ElementAt(i));
                for (int j = i + 1; j < referenceSolutions.Count; j++)
                {
                    listForSubSets.Add(referenceSolutions.ElementAt(j));

                    var copyOfList = listForSubSets.ToList();

                    var newTrialSolutions = CreateNewSolutionsFromSolutions(copyOfList);
                    result.AddRange(newTrialSolutions);

                    foreach (var solution in newTrialSolutions)
                    {
                    }

                    listForSubSets.Remove(referenceSolutions.ElementAt(j));
                }
                listForSubSets.Remove(referenceSolutions.ElementAt(i));
            }

            return result.ToList();
        }

        private List<InstanceSolution> CreateNewSolutionsFromSolutions(List<InstanceSolution> solutions)
        {
            var newTrialPermutations = combinationMethod.CombineSolutionsThreadSafe(solutions);
            var newTrialSolutions = CreateSolutions(newTrialPermutations);
            improvementMethod.ImproveSolutions(newTrialSolutions);
            return newTrialSolutions;
        }

        private List<InstanceSolution> CreateSolutions(List<int[]> newPermutations)
        {
            var result = new List<InstanceSolution>();

            foreach (var permutation in newPermutations)
            {
                var newSolution = new InstanceSolution(qapInstance, permutation);
                result.Add(newSolution);
            }

            return result;
        }
    }
}

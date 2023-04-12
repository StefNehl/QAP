using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods
{
    public class SubSetGenerationMethod : ISolutionGenerationMethod
    {
        private readonly ICombinationMethod _combinationMethod;
        private readonly IImprovementMethod _improvementMethod;
        private readonly SubSetGenerationMethodType _subSetGenerationMethodType;

        private QAPInstance? _qapInstance;
        private int _typeCount;

        public SubSetGenerationMethod(
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
        /// <returns></returns>
        public List<InstanceSolution> GenerateType1SubSet(List<InstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<InstanceSolution>();
            return GetSolutionForSubSets(referenceSolutions, listForSubSets, 0);
        }

        /// <summary>
        /// 18_P.27
        /// </summary>
        /// <param name="referenceSolutions"></param>
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
        /// <param name="referenceSolutions"></param>
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
        /// <param name="referenceSolutions"></param>
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
                    var newTrialPermutations = _combinationMethod.CombineSolutions(listForSubSets);
                    var newTrialSolutions = CreateSolutions(newTrialPermutations);
                    _improvementMethod.ImproveSolutions(newTrialSolutions);
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

                    listForSubSets.Remove(referenceSolutions.ElementAt(j));
                }
                listForSubSets.Remove(referenceSolutions.ElementAt(i));
            }

            return result.ToList();
        }

        private List<InstanceSolution> CreateNewSolutionsFromSolutions(List<InstanceSolution> solutions)
        {
            var newTrialPermutations = _combinationMethod.CombineSolutionsThreadSafe(solutions);
            var newTrialSolutions = CreateSolutions(newTrialPermutations);
            _improvementMethod.ImproveSolutions(newTrialSolutions);
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
            var newSubSets = new List<InstanceSolution>();
            switch (_typeCount)
            {
                case 1:
                    newSubSets.AddRange(GenerateType1SubSet(referenceSolutions));
                    break;
                case 2:
                    newSubSets.AddRange(GenerateType2SubSet(referenceSolutions));
                    break;
                case 3:
                    newSubSets.AddRange(GenerateType3SubSet(referenceSolutions));
                    break;
                case 4:
                    newSubSets.AddRange(GenerateType4SubSet(referenceSolutions));
                    _typeCount = 0;
                    break;
            }

            if (_subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                _typeCount++;
            
            return newSubSets;
        }
    }
}

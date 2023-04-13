using System.Collections.Concurrent;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods
{
    public class ParallelSubSetGeneration : SubSetGeneration, ISolutionGenerationMethod
    {
        private readonly ICombinationMethod _combinationMethod;
        private readonly IImprovementMethod _improvementMethod;
        private readonly SubSetGenerationMethodType _subSetGenerationMethodType;

        private QAPInstance? _qapInstance;
        private int _typeCount;

        public ParallelSubSetGeneration(
            int typeCount,
            SubSetGenerationMethodType subSetGenerationMethodType,
            ICombinationMethod combinationMethod, 
            IImprovementMethod improvementMethod) : base(typeCount, subSetGenerationMethodType, combinationMethod, improvementMethod)
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

        protected override List<InstanceSolution> GetSolutionForSubSets(
            List<InstanceSolution> referenceSolutions,
            List<InstanceSolution> listForSubSets,
            int startIndex)
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
                    var newTask = Task.Factory.StartNew(() =>
                    {
                        var newTrialSolutions = CreateNewSolutionsFromSolutionsAsync(copyOfList);
                        foreach (var solution in newTrialSolutions)
                            result.Add(solution);
                    });
                    tasksToFinish.Add(newTask);

                    listForSubSets.Remove(referenceSolutions.ElementAt(j));
                }
                listForSubSets.Remove(referenceSolutions.ElementAt(i));
            }

            Task.WhenAll(tasksToFinish).Wait();
            return result.ToList();
        }

        private List<InstanceSolution> CreateNewSolutionsFromSolutionsAsync(List<InstanceSolution> solutions)
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

        public new List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
        {
            return GetSolutionsAsync(referenceSolutions);
        }

        private List<InstanceSolution> GetSolutionsAsync(List<InstanceSolution> referenceSolutions)
        {
            var newSubSets = new List<InstanceSolution>();
            switch (_typeCount)
            {
                case 1:
                    newSubSets.AddRange(GenerateType1SubSet(referenceSolutions));
                    break;
                case 2:
                    newSubSets.AddRange( GenerateType2SubSet(referenceSolutions));
                    break;
                case 3:
                    newSubSets.AddRange( GenerateType3SubSet(referenceSolutions));
                    break;
                case 4:
                    newSubSets.AddRange( GenerateType4SubSet(referenceSolutions));
                    _typeCount = 0;
                    break;
            }
            
            if (_subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                _typeCount++;
            
            return newSubSets;
        }
    }
}

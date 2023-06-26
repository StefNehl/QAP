using System.Collections.Concurrent;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods
{
    public class ParallelSubSetGeneration : SubSetGeneration, ISolutionGenerationMethod
    {
        private readonly ICombinationMethod _combinationMethod;
        private readonly IImprovementMethod _improvementMethod;

        private QAPInstance _qapInstance;

        public ParallelSubSetGeneration(
            int typeCount,
            SubSetGenerationMethodType subSetGenerationMethodType,
            ICombinationMethod combinationMethod, 
            IImprovementMethod improvementMethod) : base(typeCount, subSetGenerationMethodType, combinationMethod, improvementMethod)
        {
            _combinationMethod = combinationMethod;
            _improvementMethod = improvementMethod;

        }

        public new void InitMethod(QAPInstance instance)
        {
            base.InitMethod(instance);
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
                        var newTrialSolutions = CreateNewSolutionsFromSolutions(copyOfList);
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
    }
}

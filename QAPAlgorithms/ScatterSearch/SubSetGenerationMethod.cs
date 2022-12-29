using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public List<IInstanceSolution> GenerateType1SubSet(List<IInstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<IInstanceSolution>();
            return GetSolutionForSubSets(referenceSolutions, listForSubSets, 0);
        }

        /// <summary>
        /// 18_P.27
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public List<IInstanceSolution> GenerateType2SubSet(List<IInstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<IInstanceSolution>
            {
                referenceSolutions[0]
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
        public List<IInstanceSolution> GenerateType3SubSet(List<IInstanceSolution> referenceSolutions)
        {
            var listForSubSets = new List<IInstanceSolution>
            {
                referenceSolutions[0],
                referenceSolutions[1]
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
        public List<IInstanceSolution> GenerateType4SubSet(List<IInstanceSolution> referenceSolutions)
        {
            var result = new List<IInstanceSolution>();
            var listForSubSets = new List<IInstanceSolution>();

            for(int i = 0; i < referenceSolutions.Count; i++)
            {
                listForSubSets.Add(referenceSolutions[i]);
                if(i >= 4)
                {
                    var newTrialPermutations = combinationMethod.CombineSolutionsPairWise(listForSubSets);
                    var newTrialSolutions = CreateSolutions(newTrialPermutations);
                    improvementMethod.ImproveSolutionsInParallelAsync(newTrialSolutions, default);
                    result.AddRange(newTrialSolutions);
                }
            }

            return result;
        }

        private List<IInstanceSolution> GetSolutionForSubSets(
            List<IInstanceSolution> referenceSolutions,
            List<IInstanceSolution> listForSubSets,
            int startIndex)
        {
            var result = new List<IInstanceSolution>();

            for (int i = startIndex; i < referenceSolutions.Count - 1; i++)
            {
                listForSubSets.Add(referenceSolutions[i]);
                for (int j = i + 1; i < referenceSolutions.Count; j++)
                {
                    listForSubSets.Add(referenceSolutions[j]);
                    var newTrialPermutations = combinationMethod.CombineSolutionsPairWise(listForSubSets);
                    var newTrialSolutions = CreateSolutions(newTrialPermutations);
                    improvementMethod.ImproveSolutionsInParallelAsync(newTrialSolutions, default);
                    result.AddRange(newTrialSolutions);

                    listForSubSets.Remove(referenceSolutions[j]);
                }
                listForSubSets.Remove(referenceSolutions[i]);
            }

            return result;
        }

        private List<IInstanceSolution> CreateSolutions(List<int[]> newPermutations)
        {
            var result = new List<IInstanceSolution>();
            foreach(var permutation in newPermutations)
            {
                var newSolution = new InstanceSolution(qapInstance, permutation);
                result.Add(newSolution);
            }

            return result;
        }



    }
}

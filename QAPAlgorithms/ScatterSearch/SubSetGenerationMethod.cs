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
        private ICombinationMethod combinationMethod;
        public SubSetGenerationMethod(ICombinationMethod combinationMethod)
        {
            this.combinationMethod = combinationMethod;
        }
        /// <summary>
        /// Returns an array of new 2-elment with pairwise combination solution of the reference list. 
        /// 
        /// If the array can not be filled up every position with a new solution the position stays null
        /// 18_P.27
        /// </summary>
        /// <param name="instanceSolution"></param>
        /// <param name="sizeOfSubSet">size of the array</param>
        /// <param name="storedCombinations"></param>
        /// <returns></returns>
        public InstanceSolution[] GenerateType1SubSet(List<InstanceSolution> referenceSolutions, 
            int sizeOfSubSet)
        {
            var result = new InstanceSolution[sizeOfSubSet];

            for(int i = 0; i < referenceSolutions.Count - 1; i++)
            {
                var firstSolution = referenceSolutions[i];
                for(int j = i+1; i < referenceSolutions.Count; j++)
                {
                    var secondSolution = referenceSolutions[j];
                    combinationMethod.CombineSolutionsPairWise(new List<IInstanceSolution> { firstSolution, secondSolution });
                }
            }

            return result;
        }

       

    }
}

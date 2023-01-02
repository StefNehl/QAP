using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAP
{
    public class TestInstance
    {
        private ICombinationMethod combinationMethod;
        private IGenerateInitPopulationMethod generateInitPopulationMethod;
        private IImprovementMethod improvementMethod;
        private ScatterSearchStart scatterSearch;

        public TestInstance(ICombinationMethod combinationMethod, IGenerateInitPopulationMethod generateInitPopulationMethod, IImprovementMethod improvementMethod)
        {
            this.combinationMethod = combinationMethod;
            this.generateInitPopulationMethod= generateInitPopulationMethod;  
            this.improvementMethod= improvementMethod;
        }

        public TestResult StartTest(QAPInstance instance, 
            int populationSize, 
            int referenceSetSize, 
            int runTimeInSeconds, 
            int subSetGenerationTypes,
            SubSetGenerationMethodType subSetGenerationMethodType,
            bool displayProgressInConsole)
        {
            scatterSearch = new ScatterSearchStart(instance, improvementMethod, combinationMethod, generateInitPopulationMethod, populationSize, referenceSetSize);
            var result = scatterSearch.Solve(runTimeInSeconds, subSetGenerationTypes, subSetGenerationMethodType, displayProgressInConsole);

            var newTestResult = new TestResult(
                instance.InstanceName, 
                instance.N, 
                result.Item1.SolutionValue, 
                runTimeInSeconds, 
                result.Item1.SolutionPermutation, 
                combinationMethod.GetType().Name, 
                generateInitPopulationMethod.GetType().Name, 
                improvementMethod.GetType().Name);
            return newTestResult;
        }
    }
}

using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.ScatterSearch
{
    public class ScatterSearchStart
    {
        //16_Design of Heuristic Algorithms for Hard Optimization.pdf 215
        //14_Principles of Scatter Search P.3
        private readonly int populationSize; //size of the complete population (max = 20)
        private readonly int refrerenceSetSize; //size of the reference (elite) solutions (around 10) 
        private QAPInstance currentInstance;
        public List<int[]> Population { get; set; }
        public List<IInstanceSolution> ReferenceSet { get; set; }

        private readonly SubSetGenerationMethod subSetGenerationMethod;
        private readonly IGenerateInitPopulationMethod generateInitPopulationMethod;

        public ScatterSearchStart(QAPInstance instance,
            IImprovementMethod improvementMethod,
            ICombinationMethod combinationMethod,
            IGenerateInitPopulationMethod generateInitPopulationMethod,
            int populationSize = 10,
            int referenceSetSize = 5)
        {
            currentInstance = instance;
            this.populationSize = populationSize;
            this.refrerenceSetSize = referenceSetSize;

            this.subSetGenerationMethod = new SubSetGenerationMethod(instance, combinationMethod, improvementMethod);
            this.generateInitPopulationMethod = generateInitPopulationMethod;

            Population = new List<int[]>();
            ReferenceSet = new List<IInstanceSolution>();
        }

        public long IterationCount { get; set; }

        /// <summary>
        /// Returns the solved solution with the permutation and solution value and the number of iterations
        /// </summary>
        /// <param name="runTimeInSeconds"></param>
        /// <returns></returns>
        public Tuple<IInstanceSolution, long> Solve(int runTimeInSeconds, bool displayProgressInConsole = false)
        {
            var currentTime = DateTime.Now;
            var timeToEnd = currentTime.AddSeconds(runTimeInSeconds);

            ReferenceSet.Clear();
            Population.Clear();
            Population.AddRange(generateInitPopulationMethod.GeneratePopulation(populationSize, currentInstance.N));

            EliminateIdenticalSolutionsFromSet(Population);

            foreach(var permutation in Population) 
            {
                var newSolution = new InstanceSolution(currentInstance, permutation);
                ReferenceSetUpdate(newSolution);
            }

            var foundNewSolutions = true;
            IterationCount = 0;
            var thousendsCount = 0;

            var typeCount = 1;
            var newSubSets = new List<IInstanceSolution>();


            while (foundNewSolutions)
            {
                IterationCount++;
                thousendsCount++;
                //Check only every 1000 Iterations the Time
                
                if(thousendsCount == 1000)
                {
                    if (displayProgressInConsole)
                        Console.WriteLine(IterationCount);

                    thousendsCount = 0;
                    currentTime = DateTime.Now;
                    if (currentTime > timeToEnd)
                        break;
                }

                foundNewSolutions = false;
                newSubSets.Clear();

                switch (typeCount)
                {
                    case 1:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType1SubSet(ReferenceSet));
                        break;
                    case 2:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType2SubSet(ReferenceSet));
                        break;
                    case 3:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType3SubSet(ReferenceSet));
                        break;
                    case 4:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType4SubSet(ReferenceSet));
                        typeCount = 0;
                        break;
                }

                foreach(var subSet in newSubSets) 
                {
                    if (ReferenceSetUpdate(subSet))
                        foundNewSolutions = true;
                }

                typeCount++;

            }

            return new Tuple<IInstanceSolution, long>(ReferenceSet[0], IterationCount);
        }

        public Task<int> SolveAsync(QAPInstance instance, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public bool ReferenceSetUpdate(IInstanceSolution newSolution)
        {
            if (!ReferenceSet.Any())
            {
                ReferenceSet.Add(newSolution);
                return true;
            }

            var newSolutionValue = newSolution.SolutionValue;
            var bestSolution = ReferenceSet.First();
            if (newSolutionValue < bestSolution.SolutionValue)
                ReferenceSet.Insert(0, newSolution);
            else
            {
                foreach(var solution in ReferenceSet)
                {
                    if(newSolutionValue > solution.SolutionValue)
                    {
                        int indexAfter = ReferenceSet.IndexOf(solution) + 1;
                        if (indexAfter >= ReferenceSet.Count)
                        {
                            if (ReferenceSet.Count >= refrerenceSetSize)
                                return false;
                            ReferenceSet.Add(newSolution);
                        }
                        else
                            ReferenceSet.Insert(indexAfter, newSolution);
                        break;
                    }
                }
            }

            if (ReferenceSet.Count > refrerenceSetSize)
                ReferenceSet.RemoveAt(ReferenceSet.Count - 1);

            return true;
        }

        public void EliminateIdenticalSolutionsFromSet(List<int[]> solutionSet)
        {
            for(int i = 0; i < solutionSet.Count; i++) 
            {
                for(int j = i+1; j < solutionSet.Count; j++) 
                {
                    if (InstanceHelpers.IsEqual(solutionSet[i], solutionSet[j]))
                    {
                        solutionSet.RemoveAt(j);
                    }

                }
            }
        }

    }
}

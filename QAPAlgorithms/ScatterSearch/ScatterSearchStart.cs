﻿using Domain;
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

        private readonly IImprovementMethod improvementMethod;
        private readonly SubSetGenerationMethod subSetGenerationMethod;
        private readonly ICombinationMethod combinationMethod;

        public ScatterSearchStart(QAPInstance instance,
            IImprovementMethod improvementMethod,
            ICombinationMethod combinationMethod,
            int populationSize = 10,
            int referenceSetSize = 5)
        {
            currentInstance = instance;
            this.populationSize = populationSize;
            this.refrerenceSetSize = referenceSetSize;

            this.improvementMethod = improvementMethod;
            this.combinationMethod = combinationMethod;
            this.subSetGenerationMethod = new SubSetGenerationMethod(instance, combinationMethod, improvementMethod);

            Population = new List<int[]>();
            ReferenceSet = new List<IInstanceSolution>();
        }

        /// <summary>
        /// Returns the solved solution with the permutation and solution value and the number of iterations
        /// </summary>
        /// <param name="maxIterations"></param>
        /// <returns></returns>
        public Tuple<IInstanceSolution, int> Solve(int maxIterations)
        {
            ReferenceSet.Clear();
            GenerateInitialPopulation();
            EliminateIdenticalSolutionsFromSet(Population);

            foreach(var permutation in Population) 
            {
                var newSolution = new InstanceSolution(currentInstance, permutation);
                ReferenceSetUpdate(newSolution);
            }

            var foundNewSolutions = true;
            int count = 0;

            while(foundNewSolutions)
            {
                count++;

                foundNewSolutions = false;
                var newSubSets = subSetGenerationMethod.GenerateType1SubSet(ReferenceSet);

                foreach(var subSet in newSubSets) 
                {
                    if (ReferenceSetUpdate(subSet))
                        foundNewSolutions = true;
                }

                if (count == maxIterations)
                    break;
            }

            return new Tuple<IInstanceSolution, int>(ReferenceSet[0], count);
        }

        public Task<int> SolveAsync(QAPInstance instance, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //Generates Initial population 
        //Start with the first index and sets every index to the i 
        //Next solution starts with index + 1
        public void GenerateInitialPopulation(int nrOfIndexesToMovePerIteration = 1)
        {
            Population.Clear();

            for (int s = 0; s < populationSize; s++)
            {
                var newSolution = new int[currentInstance.N];
                for (int i = 0; i < currentInstance.N; i++)
                {
                    if (s == 0)
                        newSolution[i] = i;
                    else
                    {
                        int newIndex = i - nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = currentInstance.N + newIndex;
                        newSolution[i] = Population[s-1][newIndex];
                    }
                    
                }
                Population.Add(newSolution);
            }
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

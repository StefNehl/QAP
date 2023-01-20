﻿using Domain;
using Domain.Models;
using Microsoft.Win32.SafeHandles;
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
        private List<IInstanceSolution> population;
        private List<IInstanceSolution> referenceSet;

        private readonly IGenerateInitPopulationMethod generateInitPopulationMethod;
        private readonly IDiversificationMethod diversificationMethod;
        private readonly ICombinationMethod combinationMethod;
        private readonly IImprovementMethod improvementMethod;

        private DateTime startTime;
        private DateTime currentTime;
        private DateTime endTime;
        private int typeCount;
        private int displayCount;
        private bool foundNewSolutions;

        public ScatterSearchStart(QAPInstance instance,
            IGenerateInitPopulationMethod generateInitPopulationMethod,
            IDiversificationMethod diversificationMethod,
            ICombinationMethod combinationMethod,
            IImprovementMethod improvementMethod,
            int populationSize = 10,
            int referenceSetSize = 5)
        {
            currentInstance = instance;
            this.populationSize = populationSize;
            this.refrerenceSetSize = referenceSetSize;

            this.generateInitPopulationMethod = generateInitPopulationMethod;
            this.diversificationMethod = diversificationMethod;
            this.combinationMethod = combinationMethod;
            this.improvementMethod = improvementMethod;

            population = new List<IInstanceSolution>();
            referenceSet = new List<IInstanceSolution>();
        }

        public long IterationCount { get; set; }

        /// <summary>
        /// Returns the solved solution, the number of iterations and the runTime in seconds
        /// </summary>
        /// <param name="runTimeInSeconds"></param>
        /// <returns></returns>
        public Tuple<IInstanceSolution, long, long> Solve(
            int runTimeInSeconds,
            int subSetGenerationTypes = 4,
            SubSetGenerationMethodType subSetGenerationMethodType = SubSetGenerationMethodType.Cycle,
            bool displayProgressInConsole = false)
        {
            InitScattersearch(runTimeInSeconds, subSetGenerationTypes);
            var subSetGenerationMethod = new SubSetGenerationMethod(currentInstance, combinationMethod, improvementMethod);
            var newSubSets = new List<IInstanceSolution>();

            while (true)
            {
                IterationCount++;
                displayCount++;

                //Check only every 1000 Iterations the Time
                if(displayCount == 10)
                {
                    if (displayProgressInConsole)
                    {
                        Console.WriteLine($"Iteration: {IterationCount} Result: {GetBestSolution().SolutionValue}");
                    }

                    displayCount = 0;
                    currentTime = DateTime.Now;
                    if (currentTime > endTime)
                        break;
                }

                foundNewSolutions = false;
                newSubSets.Clear();

                switch (typeCount)
                {
                    case 1:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType1SubSet(referenceSet));
                        break;
                    case 2:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType2SubSet(referenceSet));
                        break;
                    case 3:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType3SubSet(referenceSet));
                        break;
                    case 4:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType4SubSet(referenceSet));
                        typeCount = 0;
                        break;
                }

                foreach (var subSet in newSubSets) 
                {
                    if (ReferenceSetUpdate(subSet))
                        foundNewSolutions = true;

                    if (referenceSet.Count > refrerenceSetSize)
                        throw new Exception("ReferenceSet Count is higher than allowed");
                }


                if(!foundNewSolutions)
                {
                    GenerateNewPopulation();
                    foundNewSolutions = true;
                }

                if (subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                    typeCount++;
            }

            return new Tuple<IInstanceSolution, long, long>(referenceSet.First(), IterationCount, (long)(currentTime - startTime).TotalSeconds);
        }

        public async Task<Tuple<IInstanceSolution, long, long>> SolveAsync(
                        int runTimeInSeconds,
                        int subSetGenerationTypes = 4,
                        SubSetGenerationMethodType subSetGenerationMethodType = SubSetGenerationMethodType.Cycle,
                        bool displayProgressInConsole = false,
                        CancellationToken cancellationToken = default)
        {

            InitScattersearch(runTimeInSeconds, subSetGenerationTypes);
            var newSubSets = new List<IInstanceSolution>();
            var subSetGenerationMethod = new ParallelSubSetGenerationMethod(currentInstance, combinationMethod, improvementMethod);

            while (true)
            {
                IterationCount++;
                displayCount++;

                //Check only every 1000 Iterations the Time
                if (displayCount == 10)
                {
                    if (displayProgressInConsole)
                    {
                        Console.WriteLine($"Iteration: {IterationCount} Result: {GetBestSolution().SolutionValue}");
                    }

                    displayCount = 0;
                    currentTime = DateTime.Now;
                    if (currentTime > endTime)
                        break;
                }

                foundNewSolutions = false;
                newSubSets.Clear();

                switch (typeCount)
                {
                    case 1:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType1SubSetAsync(referenceSet, cancellationToken));
                        break;
                    case 2:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType2SubSetAsync(referenceSet, cancellationToken));
                        break;
                    case 3:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType3SubSetAsync(referenceSet, cancellationToken));
                        break;
                    case 4:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType4SubSetAsync(referenceSet, cancellationToken));
                        typeCount = 0;
                        break;
                }

                foreach (var subSet in newSubSets)
                {
                    if (ReferenceSetUpdate(subSet))
                        foundNewSolutions = true;

                    if (referenceSet.Count > refrerenceSetSize)
                        throw new Exception("ReferenceSet Count is higher than allowed");
                }


                if (!foundNewSolutions)
                {
                    GenerateNewPopulation();
                    foundNewSolutions = true;
                }

                if (subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                    typeCount++;
            }

            return new Tuple<IInstanceSolution, long, long>(referenceSet.First(), IterationCount, (long)(currentTime - startTime).TotalSeconds);
        }

        public bool ReferenceSetUpdate(IInstanceSolution newSolution)
        {
            if (!referenceSet.Any())
            {
                referenceSet.Add(newSolution);
                return true;
            }

            if (IsSolutionAlreadyInReferenceSet(newSolution))
                return false;

            var worstItem = referenceSet.Last();
            if(referenceSet.Count == refrerenceSetSize)
            {
                if (!InstanceHelpers.IsBetterSolution(worstItem.SolutionValue, newSolution.SolutionValue))
                    return false;
            }

            for(int i = 0; i < referenceSet.Count; i++)
            {
                if (InstanceHelpers.IsBetterSolution(referenceSet[i].SolutionValue, newSolution.SolutionValue))
                {
                    referenceSet.Insert(i, newSolution);

                    if (referenceSet.Count > refrerenceSetSize)
                        referenceSet.Remove(worstItem);

                    return true;
                }
            }

            referenceSet.Add(newSolution);
            return true;
        }

        private void InitScattersearch(int runTimeInSeconds, int subSetGenerationTypes)
        {
            startTime = DateTime.Now;
            currentTime = DateTime.Now;
            endTime = startTime.AddSeconds(runTimeInSeconds);

            typeCount = subSetGenerationTypes;

            referenceSet.Clear();
            population.Clear();
            population.AddRange(generateInitPopulationMethod.GeneratePopulation());

            EliminateIdenticalSolutionsFromSet(population);

            improvementMethod.ImproveSolutions(population);

            foreach (var solution in population)
            {
                ReferenceSetUpdate(solution);
            }

            diversificationMethod.ApplyDiversificationMethod(referenceSet, population, this);
            IterationCount = 0;
            displayCount = 0;
        }

        private void GenerateNewPopulation()
        {
            population = generateInitPopulationMethod.GeneratePopulation();
            diversificationMethod.ApplyDiversificationMethod(referenceSet, population, this);
        }

        private bool IsSolutionAlreadyInReferenceSet(IInstanceSolution instanceSolution)
        {
            foreach (var solution in referenceSet)
                if (solution.HashCode == instanceSolution.HashCode)
                    return true;

            return false;
        }

        public IInstanceSolution GetBestSolution()
        {
            return referenceSet.First();
        }

        public int GetReferenceSetCount()
        {
            return referenceSet.Count;
        }

        public void EliminateIdenticalSolutionsFromSet(List<IInstanceSolution> solutionSet)
        {
            for(int i = 0; i < solutionSet.Count; i++) 
            {
                for(int j = i+1; j < solutionSet.Count; j++) 
                {
                    if (InstanceHelpers.IsEqual(solutionSet[i].SolutionPermutation, solutionSet[j].SolutionPermutation))
                    {
                        solutionSet.RemoveAt(j);
                    }
                }
            }
        }
    }

    public enum SubSetGenerationMethodType
    {
        Cycle,
        UseOnlyOne
    }
}

using Domain;
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
        private int _populationSize; //size of the complete population (max = 20)
        private int _referenceSetSize; //size of the reference (elite) solutions (around 10) 
        private QAPInstance currentInstance;
        private List<InstanceSolution> population;
        private List<InstanceSolution> referenceSet;

        private readonly IGenerateInitPopulationMethod generateInitPopulationMethod;
        private readonly IDiversificationMethod diversificationMethod;
        private readonly ICombinationMethod combinationMethod;
        private readonly IImprovementMethod improvementMethod;
        private readonly int subSetGenerationTypes;
        private readonly SubSetGenerationMethodType subSetGenerationMethodType;

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
            int subSetGenerationTypes = 4,
            SubSetGenerationMethodType subSetGenerationMethodType = SubSetGenerationMethodType.Cycle,
            int populationSize = 10,
            int referenceSetSize = 5)
        {
            currentInstance = instance;
            this.generateInitPopulationMethod = generateInitPopulationMethod;
            this.diversificationMethod = diversificationMethod;
            this.combinationMethod = combinationMethod;
            this.improvementMethod = improvementMethod;
            this.subSetGenerationTypes = subSetGenerationTypes;
            this.subSetGenerationMethodType = subSetGenerationMethodType;

            _populationSize = populationSize;
            _referenceSetSize = referenceSetSize;
            population = new List<InstanceSolution>(this._populationSize);
            referenceSet = new List<InstanceSolution>(this._referenceSetSize);
        }

        public long IterationCount { get; set; }


        /// <summary>
        /// Returns the solved solution, the number of iterations and the runTime in seconds
        /// </summary>
        /// <param name="runTimeInSeconds"></param>
        /// <param name="populationSize"></param>
        /// <param name="referenceSetSize"></param>
        /// <param name="displayProgressInConsole"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Tuple<InstanceSolution, long, long> Solve(
            int runTimeInSeconds,

            bool displayProgressInConsole = false)
        {
            InitScatterSearch(runTimeInSeconds);
            var subSetGenerationMethod = new SubSetGenerationMethod(currentInstance, combinationMethod, improvementMethod);
            var newSubSets = new List<InstanceSolution>();

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

                    if (referenceSet.Count > _referenceSetSize)
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

            return new Tuple<InstanceSolution, long, long>(referenceSet.First(), IterationCount, (long)(currentTime - startTime).TotalSeconds);
        }

        public async Task<Tuple<InstanceSolution, long, long>> SolveAsync(
                        int runTimeInSeconds,
                        bool displayProgressInConsole = false,
                        CancellationToken cancellationToken = default)
        {

            InitScatterSearch(runTimeInSeconds);
            var newSubSets = new List<InstanceSolution>();
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

                    if (referenceSet.Count > _referenceSetSize)
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

            return new Tuple<InstanceSolution, long, long>(referenceSet.First(), IterationCount, (long)(currentTime - startTime).TotalSeconds);
        }

        public bool ReferenceSetUpdate(InstanceSolution newSolution)
        {
            if (!referenceSet.Any())
            {
                referenceSet.Add(newSolution);
                return true;
            }

            if (IsSolutionAlreadyInReferenceSet(newSolution))
                return false;

            var worstItem = referenceSet.Last();
            if(referenceSet.Count == _referenceSetSize)
            {
                if (!InstanceHelpers.IsBetterSolution(worstItem.SolutionValue, newSolution.SolutionValue))
                    return false;
            }

            for(int i = 0; i < referenceSet.Count; i++)
            {
                if (InstanceHelpers.IsBetterSolution(referenceSet[i].SolutionValue, newSolution.SolutionValue))
                {
                    referenceSet.Insert(i, newSolution);

                    if (referenceSet.Count > _referenceSetSize)
                        referenceSet.Remove(worstItem);

                    return true;
                }
            }

            referenceSet.Add(newSolution);
            return true;
        }

        private void InitScatterSearch(int runTimeInSeconds)
        {
            startTime = DateTime.Now;
            currentTime = DateTime.Now;
            endTime = startTime.AddSeconds(runTimeInSeconds);

            typeCount = subSetGenerationTypes;

            referenceSet.Clear();
            population.Clear();
            population.AddRange(generateInitPopulationMethod.GeneratePopulation(_populationSize));

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
            population = generateInitPopulationMethod.GeneratePopulation(_populationSize);
            diversificationMethod.ApplyDiversificationMethod(referenceSet, population, this);
        }

        private bool IsSolutionAlreadyInReferenceSet(InstanceSolution instanceSolution)
        {
            foreach (var solution in referenceSet)
                if (solution.HashCode == instanceSolution.HashCode)
                    return true;

            return false;
        }

        public InstanceSolution GetBestSolution()
        {
            return referenceSet.First();
        }

        public int GetReferenceSetCount()
        {
            return referenceSet.Count;
        }

        public void EliminateIdenticalSolutionsFromSet(List<InstanceSolution> solutionSet)
        {
            int index = 0;
            while (index < solutionSet.Count)
            {
                var solutionToCheck = solutionSet[index];
                for (int i = index + 1; i < solutionSet.Count; i++)
                {
                    var solution = solutionSet[i];
                    if (!solution.Equals(solutionToCheck))
                        continue;

                    solutionSet.RemoveAt(i);
                    i--;
                }

                index++;
            }
        }
    }

    public enum SubSetGenerationMethodType
    {
        Cycle,
        UseOnlyOne
    }
}

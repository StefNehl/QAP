﻿using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch
{
    public class ScatterSearch
    {
        //16_Design of Heuristic Algorithms for Hard Optimization.pdf 215
        //14_Principles of Scatter Search P.3
        private readonly int _populationSize; //size of the complete population (max = 20)
        private readonly int _referenceSetSize; //size of the reference (elite) solutions (around 10) 
        private readonly List<InstanceSolution> _population;
        private readonly List<InstanceSolution> _referenceSet;

        private readonly IGenerateInitPopulationMethod _generateInitPopulationMethod;
        private readonly IDiversificationMethod _diversificationMethod;
        private readonly ICombinationMethod _combinationMethod;
        private readonly IImprovementMethod _improvementMethod;
        private readonly ISolutionGenerationMethod _solutionGenerationMethod;

        private DateTime _startTime;
        private DateTime _currentTime;
        private DateTime _endTime;
        private int _displayCount;
        private bool _foundNewSolutions;
        private long _iterationCount;
        
        
        
        public ScatterSearch(IGenerateInitPopulationMethod generateInitPopulationMethod,
            IDiversificationMethod diversificationMethod,
            ICombinationMethod combinationMethod,
            IImprovementMethod improvementMethod,
            ISolutionGenerationMethod solutionGenerationMethod,
            int populationSize = 10,
            int referenceSetSize = 5)
        {
            _generateInitPopulationMethod = generateInitPopulationMethod;
            _diversificationMethod = diversificationMethod;
            _combinationMethod = combinationMethod;
            _improvementMethod = improvementMethod;
            _solutionGenerationMethod = solutionGenerationMethod;

            _populationSize = populationSize;
            _referenceSetSize = referenceSetSize;
            _population = new List<InstanceSolution>(_populationSize);
            _referenceSet = new List<InstanceSolution>(_referenceSetSize);
        }



        /// <summary>
        /// Returns the solved solution, the number of iterations and the runTime in seconds
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="runTimeInSeconds"></param>
        /// <param name="displayProgressInConsole"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Tuple<InstanceSolution, long, long> Solve(
            QAPInstance instance,
            int runTimeInSeconds,
            bool displayProgressInConsole = false)
        {
            InitScatterSearch(runTimeInSeconds, instance);
            GenerateNewPopulation();

            foreach (var instanceSolution in _population)
            {
                ReferenceSetUpdate(instanceSolution);
            }
            
            var newSubSets = new List<InstanceSolution>();

            while (true)
            {
                _iterationCount++;
                _displayCount++;

                //Check only every 1000 Iterations the Time
                if(_displayCount == 1000)
                {
                    if (displayProgressInConsole)
                    {
                        Console.WriteLine($"Iteration: {_iterationCount} Result: {GetBestSolution().SolutionValue}");
                    }

                    _displayCount = 0;
                    _currentTime = DateTime.Now;
                    if (_currentTime > _endTime)
                        break;
                }

                _foundNewSolutions = false;
                newSubSets.Clear();
                newSubSets.AddRange(_solutionGenerationMethod.GetSolutions(_referenceSet));

                foreach (var subSet in newSubSets) 
                {
                    if (ReferenceSetUpdate(subSet))
                        _foundNewSolutions = true;

                    if (_referenceSet.Count > _referenceSetSize)
                        throw new Exception("ReferenceSet Count is higher than allowed");
                }


                if(!_foundNewSolutions)
                {
                    GenerateNewPopulation();
                    _foundNewSolutions = true;
                }
            }

            return new Tuple<InstanceSolution, long, long>(_referenceSet.First(), _iterationCount, (long)(_currentTime - _startTime).TotalSeconds);
        }

        public bool ReferenceSetUpdate(InstanceSolution newSolution)
        {
            if (!_referenceSet.Any())
            {
                _referenceSet.Add(newSolution);
                return true;
            }

            if (IsSolutionAlreadyInReferenceSet(newSolution))
                return false;

            var worstItem = _referenceSet.Last();
            if(_referenceSet.Count == _referenceSetSize)
            {
                if (!InstanceHelpers.IsBetterSolution(worstItem.SolutionValue, newSolution.SolutionValue))
                    return false;
            }

            for(int i = 0; i < _referenceSet.Count; i++)
            {
                if (InstanceHelpers.IsBetterSolution(_referenceSet[i].SolutionValue, newSolution.SolutionValue))
                {
                    _referenceSet.Insert(i, newSolution);

                    if (_referenceSet.Count > _referenceSetSize)
                        _referenceSet.Remove(worstItem);

                    return true;
                }
            }

            _referenceSet.Add(newSolution);
            return true;
        }

        private void InitScatterSearch(int runTimeInSeconds, QAPInstance instance)
        {
            _generateInitPopulationMethod.InitMethod(instance);
            _diversificationMethod.InitMethod(instance);
            _combinationMethod.InitMethod(instance);
            _improvementMethod.InitMethod(instance);
            _solutionGenerationMethod.InitMethod(instance);
            
            _startTime = DateTime.Now;
            _currentTime = DateTime.Now;
            _endTime = _startTime.AddSeconds(runTimeInSeconds);

            _referenceSet.Clear();
            _population.Clear();

            _iterationCount = 0;
            _displayCount = 0;
        }

        private void GenerateNewPopulation()
        {
            _population.Clear();
            _population.AddRange(_generateInitPopulationMethod.GeneratePopulation(_populationSize));
            EliminateIdenticalSolutionsFromSet(_population);
            _improvementMethod.ImproveSolutions(_population);
            _diversificationMethod.ApplyDiversificationMethod(_referenceSet, _population, this);
        }

        private bool IsSolutionAlreadyInReferenceSet(InstanceSolution instanceSolution)
        {
            foreach (var solution in _referenceSet)
                if (solution.HashCode == instanceSolution.HashCode)
                    return true;

            return false;
        }

        public InstanceSolution GetBestSolution()
        {
            return _referenceSet.First();
        }

        public int GetReferenceSetCount()
        {
            return _referenceSet.Count;
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

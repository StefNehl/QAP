using Domain;
using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch
{
    public class ScatterSearchStart
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
        private readonly int _subSetGenerationTypes;
        private readonly SubSetGenerationMethodType _subSetGenerationMethodType;

        private DateTime _startTime;
        private DateTime _currentTime;
        private DateTime _endTime;
        private int _typeCount;
        private int _displayCount;
        private bool _foundNewSolutions;
        private long _iterationCount;


        public ScatterSearchStart(IGenerateInitPopulationMethod generateInitPopulationMethod,
            IDiversificationMethod diversificationMethod,
            ICombinationMethod combinationMethod,
            IImprovementMethod improvementMethod,
            int subSetGenerationTypes = 4,
            SubSetGenerationMethodType subSetGenerationMethodType = SubSetGenerationMethodType.Cycle,
            int populationSize = 10,
            int referenceSetSize = 5)
        {
            _generateInitPopulationMethod = generateInitPopulationMethod;
            _diversificationMethod = diversificationMethod;
            _combinationMethod = combinationMethod;
            _improvementMethod = improvementMethod;
            _subSetGenerationTypes = subSetGenerationTypes;
            _subSetGenerationMethodType = subSetGenerationMethodType;

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
            InitScatterSearch(runTimeInSeconds);
            var subSetGenerationMethod = new SubSetGenerationMethod(instance, _combinationMethod, _improvementMethod);
            var newSubSets = new List<InstanceSolution>();

            while (true)
            {
                _iterationCount++;
                _displayCount++;

                //Check only every 1000 Iterations the Time
                if(_displayCount == 10)
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

                switch (_typeCount)
                {
                    case 1:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType1SubSet(_referenceSet));
                        break;
                    case 2:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType2SubSet(_referenceSet));
                        break;
                    case 3:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType3SubSet(_referenceSet));
                        break;
                    case 4:
                        newSubSets.AddRange(subSetGenerationMethod.GenerateType4SubSet(_referenceSet));
                        _typeCount = 0;
                        break;
                }

                //newSubSets.AddRange(PathRelinking.GeneratePathAndGetSolutions());

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

                if (_subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                    _typeCount++;
            }

            return new Tuple<InstanceSolution, long, long>(_referenceSet.First(), _iterationCount, (long)(_currentTime - _startTime).TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="runTimeInSeconds"></param>
        /// <param name="displayProgressInConsole"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tuple<InstanceSolution, long, long>> SolveAsync(
            QAPInstance instance,
            int runTimeInSeconds,
            bool displayProgressInConsole = false,
            CancellationToken cancellationToken = default)
        {
            InitScatterSearch(runTimeInSeconds);
            var newSubSets = new List<InstanceSolution>();
            var subSetGenerationMethod = new ParallelSubSetGenerationMethod(instance, _combinationMethod, _improvementMethod);

            while (true)
            {
                _iterationCount++;
                _displayCount++;

                //Check only every 1000 Iterations the Time
                if (_displayCount == 10)
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
                
                

                switch (_typeCount)
                {
                    case 1:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType1SubSetAsync(_referenceSet, cancellationToken));
                        break;
                    case 2:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType2SubSetAsync(_referenceSet, cancellationToken));
                        break;
                    case 3:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType3SubSetAsync(_referenceSet, cancellationToken));
                        break;
                    case 4:
                        newSubSets.AddRange(await subSetGenerationMethod.GenerateType4SubSetAsync(_referenceSet, cancellationToken));
                        _typeCount = 0;
                        break;
                }
                
                foreach (var subSet in newSubSets)
                {
                    if (ReferenceSetUpdate(subSet))
                        _foundNewSolutions = true;

                    if (_referenceSet.Count > _referenceSetSize)
                        throw new Exception("ReferenceSet Count is higher than allowed");
                }


                if (!_foundNewSolutions)
                {
                    GenerateNewPopulation();
                    _foundNewSolutions = true;
                }

                if (_subSetGenerationMethodType == SubSetGenerationMethodType.Cycle)
                    _typeCount++;
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

        private void InitScatterSearch(int runTimeInSeconds)
        {
            _startTime = DateTime.Now;
            _currentTime = DateTime.Now;
            _endTime = _startTime.AddSeconds(runTimeInSeconds);

            _typeCount = _subSetGenerationTypes;

            _referenceSet.Clear();
            _population.Clear();
            _population.AddRange(_generateInitPopulationMethod.GeneratePopulation(_populationSize));

            EliminateIdenticalSolutionsFromSet(_population);

            _improvementMethod.ImproveSolutions(_population);

            foreach (var solution in _population)
            {
                ReferenceSetUpdate(solution);
            }

            _diversificationMethod.ApplyDiversificationMethod(_referenceSet, _population, this);
            _iterationCount = 0;
            _displayCount = 0;
        }

        private void GenerateNewPopulation()
        {
            _population.Clear();
            _population.AddRange(_generateInitPopulationMethod.GeneratePopulation(_populationSize));
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

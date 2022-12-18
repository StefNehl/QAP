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
        private readonly int populationSize = 10; //size of the complete population (max = 20)
        private readonly int refrerenceSetSize = 5; //size of the reference (elite) solutions (around 10) 
        private Instance currentInstance;
        public List<int[]> Population { get; set; }
        public List<InstanceSolution> ReferenceSet { get; set; } 


        public ScatterSearchStart(Instance instance, int? populationSize = null)
        {
            currentInstance = instance;
            if(populationSize != null)
                this.populationSize = populationSize.Value;

            Population = new List<int[]>();
            ReferenceSet = new List<InstanceSolution>();
        }

        public InstanceSolution Solve()
        {
            
            var resultPermutation = new int[currentInstance.N];
            ReferenceSet.Clear();
            GenerateInitialPopulation();

            var foundNewSolutions = true;

            while(foundNewSolutions)
            {
                var newSubSets = GenerateNewSubSets(2, 10);
                foundNewSolutions = false;

                for(int i = 0; i < newSubSets.Length; i++)
                {
                    var subSet = newSubSets[i,0];
                }
            }


            return new InstanceSolution(currentInstance, resultPermutation);
        }

        //
        private int[,] GenerateNewSubSets(int sizeOfSubsets, int nrOfSubsets)
        {
            var subSets = new int[sizeOfSubsets, nrOfSubsets];

            return subSets;
        }

        public Task<int> SolveAsync(Instance instance, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //Generates Initial population 
        //Start with the first index and sets every index to the i 
        //Next solution starts with index + 1
        public void GenerateInitialPopulation(int nrOfIndexesToMovePerIteration = 1)
        {
            Population.Clear();

            int newIndexValue;
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

        public bool ReferenceSetUpdate(int[] newSolution)
        {
            if (!ReferenceSet.Any())
            {
                ReferenceSet.Add(new InstanceSolution(currentInstance,newSolution));
                return true;
            }

            var newSolutionValue = InstanceHelpers.GetInstanceValueWithSolution(currentInstance, newSolution);
            var bestSolution = ReferenceSet.First();
            if (newSolutionValue < bestSolution.SolutionValue)
                ReferenceSet.Insert(0, new InstanceSolution(currentInstance, newSolution));

            foreach(var solution in ReferenceSet)
            {
                if(newSolutionValue > solution.SolutionValue)
                {
                    int indexAfter = ReferenceSet.IndexOf(solution) + 1;
                    if (indexAfter > ReferenceSet.Count)
                        ReferenceSet.Add(new InstanceSolution(currentInstance, newSolution));
                    else
                        ReferenceSet.Insert(indexAfter, new InstanceSolution(currentInstance, newSolution));
                    break;
                }
            }

            if (ReferenceSet.Count > refrerenceSetSize)
                ReferenceSet.RemoveRange(refrerenceSetSize - 1, ReferenceSet.Count);

            return false;
        }

        private void RepairAndImproveSolution()
        {
            //ToDo Improve solution with shortest distance 
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

        private void IdentifyTheBestSolutions()
        {

        }

        private void IdentifyMostDifferentSolutions()
        {

        }

        private void CombineSolutions()
        {

        }

        private void JoinPotentialSolutions()
        {

        }
    }
}

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
        private readonly int mu = 10; //size of the complete population (max = 20)
        private readonly int bigE = 5; //size of the elite solutions (around 10) 
        private Instance currentInstance;
        public int[,] Population { get; private set; }

        public ScatterSearchStart(Instance instance, int? sizeOfCompletePopulationMu = null)
        {
            currentInstance = instance;

            if(sizeOfCompletePopulationMu != null)
                mu = sizeOfCompletePopulationMu.Value;  

            Population = new int[mu, instance.N];
        }

        public InstanceSolution Solve()
        {
            
            var resultValue = int.MaxValue;
            var resultPermutation = new int[currentInstance.N];
            

            GenerateInitialPopulation();

            return new InstanceSolution(currentInstance, resultPermutation);
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
            int newIndexValue;
            for (int s = 0; s < mu; s++)
            {
                for (int i = 0; i < currentInstance.N; i++)
                {
                    if (s == 0)
                        newIndexValue = i;
                    else
                    {
                        int newIndex = i - nrOfIndexesToMovePerIteration;
                        if (newIndex < 0)
                            newIndex = currentInstance.N + newIndex;
                        newIndexValue = Population[s - 1, newIndex];
                    }

                    Population[s, i] = newIndexValue;
                }
            }
        }

        private void RepairAndImproveSolution()
        {
            //ToDo Improve solution with shortest distance 
        }

        private void EliminateIdenticalSolutions()
        {
            
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class InstanceSolution
    {
        public int[] Permutation { get; }
        public Instance Instance { get; }
        public long SolutionValue { get; }  

        public InstanceSolution(Instance instance, int[] permutation)
        {
            Permutation = permutation;
            Instance = instance;
            SolutionValue = InstanceHelpers.GetInstanceValueWithSolution(instance, permutation);
        }

    }
}

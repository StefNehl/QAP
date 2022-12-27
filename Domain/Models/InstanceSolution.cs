using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class InstanceSolution : IInstanceSolution
    {
        public int[] SolutionPermutation { get; }
        public long SolutionValue { get; }
        public long HashCode { get; }

        public InstanceSolution(Instance instance, int[] permutation)
        {
            SolutionPermutation = permutation;
            SolutionValue = InstanceHelpers.GetInstanceValueWithSolution(instance, permutation);
            HashCode = InstanceHelpers.GenerateHashCode(permutation);
        }

    }
}

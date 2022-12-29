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
        public long SolutionValue { get; private set; }
        public long HashCode { get; }

        public InstanceSolution(QAPInstance instance, int[] permutation)
        {
            SolutionPermutation = permutation;
            SolutionValue = InstanceHelpers.GetSolutionValue(instance, permutation);
            HashCode = InstanceHelpers.GenerateHashCode(permutation);
        }

        public void RefreshSolutionValue(QAPInstance instance)
        {
            SolutionValue = InstanceHelpers.GetSolutionValue(instance, this.SolutionPermutation);
        }
    }
}

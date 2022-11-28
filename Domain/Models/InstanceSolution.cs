using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record InstanceSolution(Instance Instance, int[] Permutation)
    {
        public long SolutionValue
        {
            get
            {
                return Instance.GetInstanceValue(Permutation);
            }
        }
    }
}

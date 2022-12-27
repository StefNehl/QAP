using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StoredCombination
    {
        public StoredCombination(long hashCodeAOfPermutation, long hashCodeBOfPermutation)
        {
            HashCodeAOfPermutation = hashCodeAOfPermutation;
            HashCodeBOfPermutation = hashCodeBOfPermutation;
        }

        public long HashCodeAOfPermutation { get; }
        public long HashCodeBOfPermutation { get; }

        public bool IsEquals(InstanceSolution a, InstanceSolution b)
        {
            if (a.HashCode.Equals(HashCodeAOfPermutation) &&
                b.HashCode.Equals(HashCodeBOfPermutation))
                return true;

            if (b.HashCode.Equals(HashCodeAOfPermutation) &&
                a.HashCode.Equals(HashCodeBOfPermutation))
                return true;

            return false;
        }
    }
}

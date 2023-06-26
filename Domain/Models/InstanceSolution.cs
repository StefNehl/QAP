using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public struct InstanceSolution
    {
        public int[] SolutionPermutation { get; }
        public long SolutionValue { get; set; }
        public long HashCode { get; private set; }

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

        public void RefreshHashCode()
        {
            HashCode = InstanceHelpers.GenerateHashCode(SolutionPermutation);
        }

        public string DisplayInConsole()
        {
            var resultString = string.Empty;

            Console.WriteLine();
            Console.WriteLine($"Solution Value: {SolutionValue}");
            Console.Write("[ ");
            for(int i = 0; i < SolutionPermutation.Length; i++)
            {
                Console.Write(SolutionPermutation[i] + " ");
            }
            Console.Write("]");
            Console.WriteLine();

            return resultString;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not InstanceSolution)
                return false;
            if (HashCode == ((InstanceSolution)obj).HashCode)
                return true;

            return false;
        }
    }
}

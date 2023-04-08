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
    }
}

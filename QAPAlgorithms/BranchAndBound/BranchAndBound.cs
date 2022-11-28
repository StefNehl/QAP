using Domain.Models;
using QAPAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.BranchAndBound
{
    public class BranchAndBound : IAlgorithm
    {
        public int Solve(Instance instance)
        {
            throw new NotImplementedException();
        }

        public Task<int> SolveAsync(Instance instance, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.Contracts
{
    public interface IAlgorithm
    {
        Task<int> SolveAsync(Instance instance, CancellationToken cancellationToken);
        int Solve(Instance instance);
    }
}

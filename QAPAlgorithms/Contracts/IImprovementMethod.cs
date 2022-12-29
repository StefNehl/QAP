using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.Contracts
{
    public interface IImprovementMethod
    {
        void ImproveSolution(IInstanceSolution instanceSolution);

        Task ImproveSolutionsInParallelAsync(List<IInstanceSolution> instanceSolutions, CancellationToken ct);
    }
}

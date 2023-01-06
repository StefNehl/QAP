using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.Contracts
{
    public interface ICombinationMethod
    {
        List<int[]> CombineSolutions(List<IInstanceSolution> solutions);
        List<int[]> CombineSolutionsThreadSafe(List<IInstanceSolution> solutions, CancellationToken ct);
    }
}

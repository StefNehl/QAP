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
        List<int[]> CombineSolutions(List<InstanceSolution> solutions);
        List<int[]> CombineSolutionsThreadSafe(List<InstanceSolution> solutions);
    }
}

using Domain.Models;
using QAPAlgorithms.ScatterSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.Contracts
{
    public interface IDiversificationMethod
    {
        void ApplyDiversificationMethod(List<IInstanceSolution> referenceSet, List<int[]> population, ScatterSearchStart scatterSearchStart);
    }
}

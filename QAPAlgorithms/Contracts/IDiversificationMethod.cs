using Domain.Models;
using QAPAlgorithms.ScatterSearch;

namespace QAPAlgorithms.Contracts
{
    public interface IDiversificationMethod
    {
        void ApplyDiversificationMethod(List<InstanceSolution> referenceSet, List<InstanceSolution> population, ScatterSearch.ScatterSearch scatterSearch);
    }
}

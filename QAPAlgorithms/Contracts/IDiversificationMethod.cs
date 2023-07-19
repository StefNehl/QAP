using Domain.Models;
using QAPAlgorithms.ScatterSearch;

namespace QAPAlgorithms.Contracts
{
    public interface IDiversificationMethod
    {
        void InitMethod(QAPInstance instance);
        void ApplyDiversificationMethod(List<InstanceSolution> referenceSet, 
            List<InstanceSolution> population, 
            ScatterSearch.ScatterSearch scatterSearch,
            double percentageOfSolutionToRemove = 0.5);
    }
}

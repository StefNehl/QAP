using Domain.Models;

namespace QAPAlgorithms.Contracts
{
    public interface ICombinationMethod
    {
        List<int[]> CombineSolutions(List<InstanceSolution> solutions);
        List<int[]> CombineSolutionsThreadSafe(List<InstanceSolution> solutions);
    }
}

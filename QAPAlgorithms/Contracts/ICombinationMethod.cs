using Domain.Models;

namespace QAPAlgorithms.Contracts
{
    public interface ICombinationMethod
    {
        void InitMethod(QAPInstance instance);
        List<int[]> CombineSolutions(List<InstanceSolution> solutions);
        List<int[]> CombineSolutionsThreadSafe(List<InstanceSolution> solutions);
    }
}

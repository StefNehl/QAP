using Domain.Models;

namespace QAPAlgorithms.Contracts
{
    public interface IImprovementMethod
    {
        InstanceSolution ImproveSolution(InstanceSolution instanceSolution);

        void ImproveSolutions(List<InstanceSolution> instanceSolutions);

        Task ImproveSolutionsInParallelAsync(List<InstanceSolution> instanceSolutions, CancellationToken ct = default);
    }
}

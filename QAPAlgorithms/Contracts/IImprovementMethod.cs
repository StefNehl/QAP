using Domain.Models;

namespace QAPAlgorithms.Contracts
{
    public interface IImprovementMethod
    {
        void ImproveSolution(IInstanceSolution instanceSolution);

        void ImproveSolutions(List<IInstanceSolution> instanceSolutions);

        Task ImproveSolutionsInParallelAsync(List<IInstanceSolution> instanceSolutions, CancellationToken ct = default);
    }
}

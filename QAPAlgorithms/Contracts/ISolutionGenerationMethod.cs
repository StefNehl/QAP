using Domain.Models;

namespace QAPAlgorithms.Contracts;

public interface ISolutionGenerationMethod
{
    void InitMethod(QAPInstance instance);
    HashSet<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions);
}
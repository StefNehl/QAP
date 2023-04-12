using Domain.Models;

namespace QAPAlgorithms.Contracts;

public interface ISolutionGenerationMethod
{
    void InitMethod(QAPInstance instance);
    List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions);
}
using Domain.Models;

namespace QAPAlgorithms.Contracts;

public interface ISolutionGenerationMethod
{
    List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions);
}
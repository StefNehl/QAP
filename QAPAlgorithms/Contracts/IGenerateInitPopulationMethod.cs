using Domain.Models;

namespace QAPAlgorithms.Contracts
{
    public interface IGenerateInitPopulationMethod
    {
        List<InstanceSolution> GeneratePopulation(int populationSize);
    }
}

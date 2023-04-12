using Domain.Models;

namespace QAPAlgorithms.Contracts
{
    public interface IGenerateInitPopulationMethod
    {
        void InitMethod(QAPInstance instance);
        List<InstanceSolution> GeneratePopulation(int populationSize);
    }
}

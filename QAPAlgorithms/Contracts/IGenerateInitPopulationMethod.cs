using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPAlgorithms.Contracts
{
    public interface IGenerateInitPopulationMethod
    {
        List<int[]> GeneratePopulation(int populationSize, int permutationSize);
    }
}

using Domain.Models;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;

namespace QAP;

public static class TestParallelPopulationGeneration
{
    public static async Task Test()
    {
        var qapReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var instance = await qapReader.ReadFileAsync("QAPLIB", "chr12a.dat");
        
        var populationGeneration = new ParallelRandomGeneratedPopulation();
        populationGeneration.InitMethod(instance);
        
        for (int i = 0; i < 1000000000; i++)
        {
            Console.WriteLine("Test: " + i);
            var permutations = populationGeneration.GeneratePopulation(200);

            foreach (var permutation in permutations)
            {
                permutation.DisplayInConsole();
            }
        }
    }
}
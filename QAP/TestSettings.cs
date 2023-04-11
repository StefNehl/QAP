using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;

namespace QAP;

public record TestSettings(
    QAPInstance Instance, 
    int PopulationSize,
    int ReferenceSetSize,
    int RunTimeInSeconds,
    ICombinationMethod CombinationMethod,
    IGenerateInitPopulationMethod GenerateInitPopulationMethod,
    IImprovementMethod ImprovementMethod,
    IDiversificationMethod DiversificationMethod,
    ISolutionGenerationMethod SolutionGenerationMethod,
    bool DisplayProgressInConsole = false);

using Domain.Models;
using QAPAlgorithms.Contracts;
using QAPAlgorithms.ScatterSearch;

namespace QAP;

public record TestSettings(
    QAPInstance Instance, 
    int PopulationSize,
    int ReferenceSetSize,
    int RunTimeInSeconds,
    int SubSetGenerationTypes,
    SubSetGenerationMethodType SubSetGenerationMethodType,
    ICombinationMethod CombinationMethod,
    IGenerateInitPopulationMethod GenerateInitPopulationMethod,
    IImprovementMethod ImprovementMethod,
    IDiversificationMethod DiversificationMethod,
    bool DisplayProgressInConsole = false);

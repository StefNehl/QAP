using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

// BenchmarkRunner.Run<GenerateInitialPopulationBenchmarks>();
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();
// BenchmarkRunner.Run<ImprovementBenchmarks>();
BenchmarkRunner.Run<SolutionGenerationBenchmarks>();

// var test = new SolutionGenerationBenchmarks();
// test.GetNrOfSolutions();
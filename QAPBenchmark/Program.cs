using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

BenchmarkRunner.Run<GenerateInitialPopulationBenchmarks>();
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();
// BenchmarkRunner.Run<ImprovementBenchmarks>();
// BenchmarkRunner.Run<SolutionGenerationBenchmarks>();

// var test = new GenerateInitialPopulationBenchmarks();
// test.Setup(); 
// test.StepWisePopulationGeneration();
// test.RandomGeneratedPopulation();
// test.ParallelRandomGeneratedPopulation();

using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

// BenchmarkRunner.Run<GenerateInitialPopulationBenchmarks>();
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();
// BenchmarkRunner.Run<ImprovementBenchmarks>();

BenchmarkRunner.Run<SolutionGenerationBenchmarks>();

// var test = new SolutionGenerationBenchmarks();
// await test.Setup(); 
// test.SubSetGen();
// test.SubSetGen_Parallel();
// test.PathRelinking();
// test.PathRelinking_Parallel();
// test.Combination();
// test.Combination_Parallel();

using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

// BenchmarkRunner.Run<GenerateInitialPopulationBenchmarks>();
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();
// BenchmarkRunner.Run<ImprovementBenchmarks>();
// BenchmarkRunner.Run<SubSetGenerationBenchmark>();
// BenchmarkRunner.Run<PathRelinkingBenchmarks>();
BenchmarkRunner.Run<PathRelinkingSubSetGenerationCombinedBenchmarks>();

// var test = new SubSetGenerationBenchmark();
// test.Setup(); 
// test.SubSetGen();
// test.SubSetGen_Parallel();

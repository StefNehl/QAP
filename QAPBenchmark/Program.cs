using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

//BenchmarkRunner.Run<GenerateInitialPopulationBenchmarks>();
//BenchmarkRunner.Run<InstanceHelpersBenchmarks>();
BenchmarkRunner.Run<ImprovementBenchmarks>();

//BenchmarkRunner.Run<SubSetGenerationBenchmark>();
//BenchmarkRunner.Run<PathRelinkingBenchmarks>();
//BenchmarkRunner.Run<PathRelinkingSubSetGenerationCombinedBenchmarks>();

// var test = new PathRelinkingBenchmarks();
// await test.Setup(); 
// test.PathRelinking();
// test.PathRelinking_Parallel();

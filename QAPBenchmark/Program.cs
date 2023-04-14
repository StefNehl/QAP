using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

BenchmarkRunner.Run<GenerateInitialPopulationBenchmarks>();
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();
// BenchmarkRunner.Run<ImprovementBenchmarks>();
// BenchmarkRunner.Run<SolutionGenerationBenchmarks>();

// var test = new ImprovementBenchmarks();
// test.Setup(); 
// test.LocalSearchBestImprovement_ImproveSolutions();
// test.ImprovedLocalSearchBestImprovement_ImproveSolutions();
// test.LocalSearchBestImprovement_ImproveSolutions_Parallel();
// test.LocalSearchFirstImprovement_ImproveSolutions();
// test.ImprovedLocalSearchFirstImprovement_ImproveSolutions();
// test.LocalSearchFirstImprovement_ImproveSolutions_Parallel();

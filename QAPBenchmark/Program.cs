// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

//BenchmarkRunner.Run(typeof(GenerateInitialPopulationBenchmarks).Assembly);
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();

BenchmarkRunner.Run<ImprovementBestSolutionParallelBenchmarks>();


// var test = new ImprovementParallelBenchmarks();
// test.Setup();
// await test.ImprovedLocalSearchBestImprovement_ImproveSolutions_With50Solutions_Parallel_ParallelForEach();

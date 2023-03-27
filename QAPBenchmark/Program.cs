// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

//BenchmarkRunner.Run(typeof(GenerateInitialPopulationBenchmarks).Assembly);
// BenchmarkRunner.Run<InstanceHelpersBenchmarks>();

BenchmarkRunner.Run<ImprovementParallelBenchmarks>();


//var test = new ImprovementBenchmarks();
//test.Setup();
//test.LocalSearchFirstImprovement_ImproveSolutionsParallel_With50Solutions();

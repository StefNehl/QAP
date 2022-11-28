// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using QAPBenchmark.ScatterSearchBenchmarks;

BenchmarkRunner.Run(typeof(GenerateInitialPopulationBenchmarks).Assembly);

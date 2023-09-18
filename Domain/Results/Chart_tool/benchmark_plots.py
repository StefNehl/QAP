from generate_time_plots_for_benchmark import *


def generate_plots():

    memory_label = "Allocated Memory[B]"

    generate_plot('..\\Benchmarks\\GenerateInitialPopulation_Time_Results.CSV',
                  "Initial Generation Methods Time Consumption",
                  "MeanUs",
                  "Time[us]",
                  "NrOfSolutions",
                  "Nr of Solutions generated",
                  bbox_to_anchor=(0.55, 0.31))

    generate_plot("..\\Benchmarks\\GenerateInitialPopulation_Memory_Results.CSV",
                  "Initial Generation Methods Memory Consumption",
                  "AllocatedB",
                  memory_label,
                  "NrOfSolutions",
                  "Nr of Solutions generated",
                  transform_values_against_overlapping=[2000, 2000, 10000],
                  bbox_to_anchor=(0.55, 0.31))

    generate_plot("..\\Benchmarks\\ImprovementBenchmarks_First_Time_Results.csv",
                  "First Improvement Methods Time Consumption",
                  "MeanNs",
                  "Time[ns]",
                  "NrOfSolutions",
                  "Nr of Calls",
                  transform_values_against_overlapping=[0, 0, 40000000],
                  bbox_to_anchor=(0.55, 0.045))

    generate_plot("..\\Benchmarks\\ImprovementBenchmarks_First_Memory_Results.csv",
                  "First Improvement Methods Memory Consumption",
                  "AllocatedB",
                  memory_label,
                  "NrOfSolutions",
                  "Nr of Calls",
                  transform_values_against_overlapping=[500, 500, 0],
                  bbox_to_anchor=(0.55, 0.045))

    generate_plot("..\\Benchmarks\\ImprovementBenchmarks_Best_Time_Results.csv",
                  "Best Improvement Methods Time Consumption",
                  "MeanNs",
                  "Time[ns]",
                  "NrOfSolutions",
                  "Nr of Calls",
                  transform_values_against_overlapping=[4000, 40000, 40000000],
                  bbox_to_anchor=(0.55, 0.045))

    generate_plot("..\\Benchmarks\\ImprovementBenchmarks_Best_Memory_Results.csv",
                  "Best Improvement Methods Memory Consumption",
                  "AllocatedB",
                  memory_label,
                  "NrOfSolutions",
                  "Nr of Calls",
                  transform_values_against_overlapping=[200, 300, 2000],
                  bbox_to_anchor=(0.55, 0.045))

    generate_plot("..\\Benchmarks\\SolutionGeneration_Results_Time.csv",
                  "Solutions Generation Method Time Consumption",
                  "Mean",
                  "Time[us]",
                  "NrOfCalls",
                  "Nr of Calls",
                  transform_values_against_overlapping=[200, 8000, 5000000],
                  bbox_to_anchor=(0.55, 0.045))

    generate_plot("..\\Benchmarks\\SolutionGeneration_Results_Memory.csv",
                  "Solutions Generation Method Memory Consumption",
                  "Allocated",
                  "Allocated Memory[KB]",
                  "NrOfCalls",
                  "Nr of Calls",
                  transform_values_against_overlapping=[300, 3000, 200000],
                  bbox_to_anchor=(0.55, 0.045))

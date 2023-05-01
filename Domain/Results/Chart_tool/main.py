from generate_time_plots_for_benchmark import *

memory_label = "Allocated Memory[B]"

generate_plot('..\\Benchmarks\\GenerateInitialPopulation_Time_Results.CSV',
              "Initial Generation Methods Time Consumption",
              "MeanUs",
              "Time[us]",
              "NrOfSolutions",
              "Nr of Solutions",
              bbox_to_anchor=(0.55, 0.31))

generate_plot("..\\Benchmarks\\GenerateInitialPopulation_Memory_Results.CSV",
              "Initial Generation Methods Memory Consumption",
              "AllocatedB",
              memory_label,
              "NrOfSolutions",
              "Nr of Solutions",
              use_different_alpha=True,
              bbox_to_anchor=(0.55, 0.31))

generate_plot("..\\Benchmarks\\ImprovementBenchmarks_Time_Results.csv",
              "Improvement Methods Time Consumption",
              "MeanNs",
              "Time[ns]",
              "NrOfSolutions",
              "Nr of Solutions",
              bbox_to_anchor=(0.55, 0.045))

generate_plot("..\\Benchmarks\\ImprovementBenchmarks_Memory_Results.csv",
              "Improvement Methods Memory Consumption",
              "AllocatedB",
              memory_label,
              "NrOfSolutions",
              "Nr of Solutions",
              bbox_to_anchor=(0.55, 0.045))

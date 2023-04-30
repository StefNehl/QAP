import pandas as pd
import matplotlib.pyplot as plt

from generate_time_plots_for_benchmark import *

generate_time_plot('..\\Benchmarks\\GenerateInitialPopulation_Time_Results.CSV',
                   "Initial Generation Methods",
                   "Mean[us]",
                   "Time[us]",
                   "NrOfSolutions",
                   "Nr of Solutions")

generate_memory_plot("..\\Benchmarks\\GenerateInitialPopulation_Memory_Results.CSV",
                     "Initial Generation Methods",
                     "Allocated[B]",
                     "Allocated[B]",
                     "NrOfSolutions",
                     "Nr of Solutions")

generate_time_plot("..\\Benchmarks\\ImprovementBenchmarks_Time_Results.csv",
                   "Improvement Methods",
                   "Mean[ns]",
                   "Time[ns]",
                   "NrOfSolutions",
                   "Nr of Solutions")

generate_time_plot("..\\Benchmarks\\ImprovementBenchmarks_Memory_Results.csv",
                   "Improvement Methods",
                   "Allocated[B]",
                   "Allocated[B]",
                   "NrOfSolutions",
                   "Nr of Solutions")

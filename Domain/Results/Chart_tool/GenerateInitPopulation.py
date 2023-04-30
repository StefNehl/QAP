import pandas as pd
import matplotlib.pyplot as plt

from generate_time_plots_for_benchmark import *

generate_time_plot('..\\Benchmarks\\GenerateInitialPopulation_Time_Results.CSV',
                   "Initial Generation Methods")

generate_memory_plot("..\\Benchmarks\\GenerateInitialPopulation_Memory_Results.CSV",
                     "Initial Generation Methods")

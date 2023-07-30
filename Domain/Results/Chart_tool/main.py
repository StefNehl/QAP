import benchmark_plots
from generate_optimization_result_plot import generate_plot
from prepare_csv_file import prepare_csv

# benchmark_plots.generate_plots()

prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/06-47-52_30-07-2023.csv")

prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-17-11_24-07-2023.csv")
generate_plot("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-17-11_24-07-2023_prepared.csv",
              x_axis_name="Reference Size",
              x_axis_label="Reference Set Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/10-15-57_24-07-2023.csv")
generate_plot("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/10-15-57_24-07-2023_prepared.csv",
              x_axis_name="Reference Size",
              x_axis_label="Reference Set Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-14-27_26-07-2023.csv")
generate_plot("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-14-27_26-07-2023_prepared.csv",
              x_axis_name="Population Size",
              x_axis_label="Population Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/23-30-16_26-07-2023.csv")
generate_plot("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/23-30-16_26-07-2023_prepared.csv",
              x_axis_name="Population Size",
              x_axis_label="Population Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")
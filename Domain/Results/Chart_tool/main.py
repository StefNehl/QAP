import benchmark_plots
from generate_optimization_result_plot import generate_plot
from prepare_csv_file import prepare_csv, merge_csv_files_to_compare_diff

# benchmark_plots.generate_plots()

# new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-17-11_24-07-2023.csv")
# generate_plot(new_file_path,
#               x_axis_name="Reference Size",
#               x_axis_label="Reference Set Size",
#               y_axis_name="Difference",
#               y_axis_label="Difference[%]")
#
# new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/10-15-57_24-07-2023.csv")
# generate_plot(new_file_path,
#               x_axis_name="Reference Size",
#               x_axis_label="Reference Set Size",
#               y_axis_name="Difference",
#               y_axis_label="Difference[%]")
#
# new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-14-27_26-07-2023.csv")
# generate_plot(new_file_path,
#               x_axis_name="Population Size",
#               x_axis_label="Population Size",
#               y_axis_name="Difference",
#               y_axis_label="Difference[%]")
#
# new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/23-30-16_26-07-2023.csv")
# generate_plot(new_file_path,
#               x_axis_name="Population Size",
#               x_axis_label="Population Size",
#               y_axis_name="Difference",
#               y_axis_label="Difference[%]")

result_without_dynamic = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/06-47-52_30-07-2023.csv")
result_with_dynamic = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/17-42-46_30-07-2023.csv")
result_with_dynamic_limit = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/17-42-46_30-07-2023.csv")

merge_csv_files_to_compare_diff(
    [result_without_dynamic, result_with_dynamic, result_with_dynamic_limit]
    )

new_file_path = result_with_dynamic_limit[:-4]
new_file_path = new_file_path + "_compare_combinations.png"
generate_plot(result_with_dynamic_limit,
              x_axis_name="Instance Name",
              x_axis_label="Instance Name",
              y_axis_name="Difference",
              y_axis_label="Difference[%]",
              show_header=False,
              x_axis_rotation=45,
              new_file_path=new_file_path)

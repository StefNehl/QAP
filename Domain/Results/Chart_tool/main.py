import benchmark_plots
from generate_optimization_result_plot import generate_plot
from prepare_csv_file import prepare_csv, merge_csv_files_to_compare_diff, calculate_mean_and_median_for_csv, \
    reduce_csv_to_optimization_scenario_instances, merge_csv_files_for_different_tests

# benchmark_plots.generate_plots()

new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-17-11_24-07-2023.csv")
generate_plot(new_file_path,
              x_axis_name="Reference Size",
              x_axis_label="Reference Set Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/10-15-57_24-07-2023.csv")
generate_plot(new_file_path,
              x_axis_name="Reference Size",
              x_axis_label="Reference Set Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/14-14-27_26-07-2023.csv")
generate_plot(new_file_path,
              x_axis_name="Population Size",
              x_axis_label="Population Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

new_file_path = prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/23-30-16_26-07-2023.csv")
generate_plot(new_file_path,
              x_axis_name="Population Size",
              x_axis_label="Population Size",
              y_axis_name="Difference",
              y_axis_label="Difference[%]")

result_without_dynamic = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/06-47-52_30-07-2023.csv")
result_with_dynamic = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/17-42-46_30-07-2023.csv")
result_with_dynamic_limit = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/01-10-43_31-07-2023.csv")

merge_csv_files_to_compare_diff(
    [result_without_dynamic, result_with_dynamic, result_with_dynamic_limit]
    )

mean_final_results = calculate_mean_and_median_for_csv(result_with_dynamic_limit)
generate_plot(mean_final_results,
              x_axis_name="Instance Name",
              x_axis_label="Instance Name",
              y_axis_name="Mean",
              y_axis_label="Geometric Mean[%]",
              show_header=False,
              x_axis_rotation=45)

final_results = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/final_result_without_optimization.csv")

mean_final_results = calculate_mean_and_median_for_csv(final_results)
generate_plot(mean_final_results,
              x_axis_name="Instance Name",
              x_axis_label="Instance Name",
              y_axis_name="Mean",
              y_axis_label="Geometric Mean[%]",
              show_header=False,
              x_axis_rotation=45)


mean_final_results_without_optimization = reduce_csv_to_optimization_scenario_instances(mean_final_results)
final_results_with_deduction = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results"
                "/final_result_decreased_values_for_parameter.csv")
mean_final_results_with_deduction = calculate_mean_and_median_for_csv(final_results_with_deduction)

final_results_with_increased = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results"
                "/final_result_increased_values_for_parameter.csv")
mean_final_results_with_increased = calculate_mean_and_median_for_csv(final_results_with_increased)

file_paths = \
    {
        mean_final_results_without_optimization: "Without Optimization",
        mean_final_results_with_deduction: "With Decreased",
        mean_final_results_with_increased: "With Increased",
    }

optimization_comparison = merge_csv_files_for_different_tests(file_paths)

generate_plot(optimization_comparison,
              x_axis_name="Instance Name",
              x_axis_label="Instance Name",
              y_axis_name="Mean",
              y_axis_label="Geometric Mean[%]",
              show_header=False,
              x_axis_rotation=45,
              second_group_string="Test Name")

final_results_with_optimized_parameters = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results"
                "/final_results_with_optimization.csv")
final_results_with_optimized_parameters = \
    calculate_mean_and_median_for_csv(final_results_with_optimized_parameters)

generate_plot(final_results_with_optimized_parameters,
              x_axis_name="Instance Name",
              x_axis_label="Instance Name",
              y_axis_name="Mean",
              y_axis_label="Geometric Mean[%]",
              show_header=False,
              x_axis_rotation=45)

final_results_with_optimized_parameters_unknown_solutions = \
    prepare_csv("C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results"
                "/final_results_with_optimization_unkown_solutions.csv")
final_results_with_optimized_parameters_unknown_solutions = \
    calculate_mean_and_median_for_csv(final_results_with_optimized_parameters_unknown_solutions)

generate_plot(final_results_with_optimized_parameters_unknown_solutions,
              x_axis_name="Instance Name",
              x_axis_label="Instance Name",
              y_axis_name="Mean",
              y_axis_label="Geometric Mean[%]",
              show_header=False,
              x_axis_rotation=45)


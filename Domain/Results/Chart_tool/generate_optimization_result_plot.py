from textwrap import wrap

import pandas as pd
import matplotlib.pyplot as plt
from pandas.core.groupby import DataFrameGroupBy

from generate_time_plots_for_benchmark import generate_plot

def generate_plot():
    # file_path = "..\\OptimizationAnalysis\\02-18-12_21-07-2023.csv"

    # file_path = "..\\OptimizationAnalysis\\10-15-57_24-07-2023.csv"
    file_path = "..\\OptimizationAnalysis\\14-17-11_24-07-2023.csv"
    data = pd.read_csv(file_path, delimiter=';')

    header_name = "Instance: " + data["Instance Name"]
    # y_axis_name = "Found Optimum"
    # y_axis_label = y_axis_name

    x_axis_name = "Reference Size"
    x_axis_label = "Reference Set Size"

    # first_groups = data.groupby('Combination Method')

    y_axis_name = "Difference"
    y_axis_label = "Optimum Difference"
    #
    # x_axis_name = "N"
    # x_axis_label = "Instance Size"

    first_groups = data.groupby('Combination Method')

    plt.figure(figsize=(8, 6))


    for first_group in first_groups:

        plt.xlabel(x_axis_label)
        plt.ylabel(y_axis_label)

        second_groups = first_group[1].groupby("Improvement Method")
        for second_group in second_groups:

            y = second_group[1][y_axis_name]
            x = second_group[1][x_axis_name]

            new_alpha = 1
            # if use_different_alpha:
            #     new_alpha -= (alpha_count / 10)

            plt.plot(x, y, label=first_group[0] + " " + second_group[0], alpha=new_alpha)
            # alpha_count += 2

    fig = plt.gcf()

    lines = []
    labels = []

    chart_line, chart_label = fig.axes[0].get_legend_handles_labels()
    lines.extend(chart_line)
    labels.extend(chart_label)

    labels = ['\n'.join(wrap(label, 35)) for label in labels]


    fig.legend(lines,
               labels)





    plt.show()



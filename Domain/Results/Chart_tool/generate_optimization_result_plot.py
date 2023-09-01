from textwrap import wrap

import pandas as pd
import matplotlib.pyplot as plt
from pandas.core.groupby import DataFrameGroupBy

from generate_time_plots_for_benchmark import generate_plot


def generate_plot(file_path: str,
                  y_axis_name: str,
                  y_axis_label: str,
                  x_axis_name: str,
                  x_axis_label: str,
                  x_axis_rotation: int = 0,
                  show_header: bool = True,
                  sort_after_instance_size: bool = False,
                  new_file_path: str = None,
                  second_group_string: str = None):
    data = pd.read_csv(file_path, delimiter=';')

    header_name = "Instance: " + data["Instance Name"][0]
    first_groups = data.groupby('Test Setting')

    plt.figure(figsize=(8, 6))

    def plot_group(group, name):
        plt.xlabel(x_axis_label)
        plt.ylabel(y_axis_label)

        if sort_after_instance_size:
            group[1].sort_values("N", inplace=True)

        y = group[1][y_axis_name]
        x = group[1][x_axis_name]

        plt.plot(x, y, label=name + str(group[0]))
        ax = plt.gca()
        ax.tick_params(axis='x', labelrotation=x_axis_rotation)

    for first_group in first_groups:
        if second_group_string is not None:
            second_groups = first_group[1].groupby(second_group_string)

            for second_group in second_groups:
                plot_group(second_group, "Test Setting " + str(first_group[0]) + " ")
        else:
            plot_group(first_group, "Test Setting")

    fig = plt.gcf()
    lines = []
    labels = []

    chart_line, chart_label = fig.axes[0].get_legend_handles_labels()
    lines.extend(chart_line)
    labels.extend(chart_label)

    labels = ['\n'.join(wrap(label, 35)) for label in labels]

    fig.legend(lines,
               labels)


    if show_header:
        plt.title(header_name)

    plt.tight_layout()
    plt.show()
    final_file_path = file_path[:-4] + ".png"

    if new_file_path is not None:
        final_file_path = new_file_path
    fig.savefig(final_file_path, dpi=200)



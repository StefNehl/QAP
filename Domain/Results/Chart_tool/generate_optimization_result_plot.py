from textwrap import wrap

import pandas as pd
import matplotlib.pyplot as plt
from pandas.core.groupby import DataFrameGroupBy

from generate_time_plots_for_benchmark import generate_plot


def generate_plot(file_path: str,
                  y_axis_name: str,
                  y_axis_label: str,
                  x_axis_name: str,
                  x_axis_label: str):
    data = pd.read_csv(file_path, delimiter=';')

    header_name = "Instance: " + data["Instance Name"][0]
    first_groups = data.groupby('Test Setting')

    plt.figure(figsize=(8, 6))

    for first_group in first_groups:
        plt.xlabel(x_axis_label)
        plt.ylabel(y_axis_label)

        y = first_group[1][y_axis_name]
        x = first_group[1][x_axis_name]

        plt.plot(x, y, label="Test Setting " + str(first_group[0]))

    fig = plt.gcf()
    lines = []
    labels = []

    chart_line, chart_label = fig.axes[0].get_legend_handles_labels()
    lines.extend(chart_line)
    labels.extend(chart_label)

    labels = ['\n'.join(wrap(label, 35)) for label in labels]

    fig.legend(lines,
               labels)

    plt.title(header_name)
    plt.show()
    new_file_path = file_path[:-4] + ".png"
    fig.savefig(new_file_path, dpi=200)



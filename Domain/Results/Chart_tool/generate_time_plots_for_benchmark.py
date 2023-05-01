import pandas as pd
import matplotlib.pyplot as plt
from pandas.core.groupby import DataFrameGroupBy


def generate_plot(file_path: str,
                  plot_name: str,
                  y_axis_name: str,
                  y_axis_label: str,
                  x_axis_name: str,
                  x_axis_label: str):
    data = pd.read_csv(file_path)
    first_groups = data.groupby("SolutionName")

    for g in first_groups:
        print(g)

    for first_group in first_groups:
        second_groups = first_group[1].groupby("Method")

        show_plot_for_group(first_group,
                            second_groups,
                            plot_name,
                            y_axis_name,
                            y_axis_label,
                            x_axis_name,
                            x_axis_label)


def show_plot_for_group(first_group,
                        second_groups: DataFrameGroupBy,
                        plot_name: str,
                        y_axis_name: str,
                        y_axis_label: str,
                        x_axis_name: str,
                        x_axis_label: str):
    for second_group in second_groups:
        y = second_group[1][y_axis_name]
        x = second_group[1][x_axis_name]
        plt.plot(x, y, linewidth=1, label=second_group[0])

    title = plot_name + " " + first_group[0]
    plt.title(title)
    plt.xlabel(x_axis_label)
    plt.ylabel(y_axis_label)
    plt.tight_layout()
    plt.legend()
    fig = plt.gcf()
    plt.show()
    fig.savefig("..\\Benchmarks\\Plots\\" + title + ".png")

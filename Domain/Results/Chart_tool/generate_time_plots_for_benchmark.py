import pandas as pd
import matplotlib.pyplot as plt


def generate_time_plot(file_path: str,
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

        for second_group in second_groups:
            y = second_group[1][y_axis_name]
            x = second_group[1][x_axis_name]
            plt.plot(x, y, linewidth=1, label=second_group[0])

        plt.title(plot_name + " " + first_group[0])
        plt.xlabel(x_axis_label)
        plt.ylabel(y_axis_label)
        plt.tight_layout()
        plt.legend()
        plt.show()


def generate_memory_plot(file_path: str,
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

        for second_group in second_groups:
            y = second_group[1][y_axis_name]
            x = second_group[1][x_axis_name]
            plt.plot(x, y, linewidth=1, label=second_group[0])

        plt.title(plot_name + " " + first_group[0])
        plt.xlabel(x_axis_label)
        plt.ylabel(y_axis_label)
        plt.tight_layout()
        plt.legend()
        plt.show()

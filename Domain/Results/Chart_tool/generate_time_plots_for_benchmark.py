from textwrap import wrap

import pandas as pd
import matplotlib.pyplot as plt
from matplotlib.transforms import offset_copy
from pandas.core.groupby import DataFrameGroupBy


def generate_plot(file_path: str,
                  plot_name: str,
                  y_axis_name: str,
                  y_axis_label: str,
                  x_axis_name: str,
                  x_axis_label: str,
                  transform_values_against_overlapping=None,
                  bbox_to_anchor=(0.5, 0.5)):
    if transform_values_against_overlapping is None:
        transform_values_against_overlapping = []
    data = pd.read_csv(file_path)
    first_groups = data.groupby("SolutionName")

    for g in first_groups:
        print(g)

    create_sub_plot_for_group(first_groups,
                              plot_name,
                              y_axis_name,
                              y_axis_label,
                              x_axis_name,
                              x_axis_label,
                              transform_values_against_overlapping,
                              bbox_to_anchor)

    # for first_group in first_groups:
    #     second_groups = first_group[1].groupby("Method")
    #
    #     show_plot_for_group(first_group,
    #                         second_groups,
    #                         plot_name,
    #                         y_axis_name,
    #                         y_axis_label,
    #                         x_axis_name,
    #                         x_axis_label,
    #                         use_different_alpha)


def show_plot_for_group(first_group: DataFrameGroupBy,
                        second_groups: DataFrameGroupBy,
                        plot_name: str,
                        y_axis_name: str,
                        y_axis_label: str,
                        x_axis_name: str,
                        x_axis_label: str,
                        use_different_alpha: bool):
    count = 2
    for second_group in second_groups:
        y = second_group[1][y_axis_name]
        x = second_group[1][x_axis_name]

        new_alpha = 1
        if use_different_alpha:
            new_alpha -= (count / 10)
        plt.plot(x, y, label=second_group[0], alpha=new_alpha)
        count += 2

    title = plot_name + " " + first_group[0]
    plt.title(title)
    plt.xlabel(x_axis_label)
    plt.ylabel(y_axis_label)
    plt.tight_layout()
    plt.legend()
    fig = plt.gcf()
    plt.show()
    fig.savefig("..\\Benchmarks\\Plots\\" + title + ".png", dpi=200)


def create_sub_plot_for_group(first_groups: DataFrameGroupBy,
                              plot_name: str,
                              y_axis_name: str,
                              y_axis_label: str,
                              x_axis_name: str,
                              x_axis_label: str,
                              transform_values_against_overlapping,
                              bbox_to_anchor=(0.5, 0.5)):

    plt.figure(figsize=(8, 6))
    plt.suptitle(plot_name)
    fig = plt.gcf()

    plot_count = 1
    group_count = 0

    for first_group in first_groups:
        second_groups = first_group[1].groupby("Method")

        plt.subplot(2, 2, plot_count)
        plt.title(first_group[0])
        plt.xlabel(x_axis_label)
        plt.ylabel(y_axis_label)
        plot_count += 1

        transform_add_value = 0

        for second_group in second_groups:
            y = second_group[1][y_axis_name]
            x = second_group[1][x_axis_name]

            plt.plot(x, y + transform_add_value, label=second_group[0])

            if len(transform_values_against_overlapping) != 0:
                transform_add_value += (
                    transform_values_against_overlapping)[group_count]

        group_count += 1

    lines = []
    labels = []

    chart_line, chart_label = fig.axes[0].get_legend_handles_labels()
    lines.extend(chart_line)
    labels.extend(chart_label)

    labels = ['\n'.join(wrap(label, 35)) for label in labels]


    fig.legend(lines,
               labels,
               bbox_to_anchor)
    plt.tight_layout()

    plt.show()
    fig.savefig("..\\Benchmarks\\Plots\\" + plot_name + ".png", dpi=200)

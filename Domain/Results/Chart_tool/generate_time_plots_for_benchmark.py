import pandas as pd
import matplotlib.pyplot as plt


def generate_time_plot(file_path: str, plot_name: str):

    data = pd.read_csv(file_path)

    first_groups = data.groupby("SolutionName")

    for g in first_groups:
        print(g)

    for first_group in first_groups:
        second_groups = first_group[1].groupby("Method")

        for second_group in second_groups:
            y = second_group[1]["Mean[us]"]
            x = second_group[1]["NrOfSolutions"]
            plt.plot(x, y, linewidth=1, label=second_group[0])

        plt.title(plot_name + " " + first_group[0])
        plt.xlabel("Nr of solutions")
        plt.ylabel("Time [us]")
        plt.tight_layout()
        plt.legend()
        plt.show()


def generate_memory_plot(file_path: str, plot_name: str):
    data = pd.read_csv(file_path)
    first_groups = data.groupby("SolutionName")

    for g in first_groups:
        print(g)

    for first_group in first_groups:
        second_groups = first_group[1].groupby("Method")

        for second_group in second_groups:
            y = second_group[1]["Allocated[B]"]
            x = second_group[1]["NrOfSolutions"]
            plt.plot(x, y, linewidth=1, label=second_group[0])

        plt.title(plot_name + " " + first_group[0])
        plt.xlabel("Nr of solutions")
        plt.ylabel("Allocation [B]")
        plt.tight_layout()
        plt.legend()
        plt.show()

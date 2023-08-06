import copy

import pandas as pd
import numpy as np


def prepare_csv(file_path: str):
    data = pd.read_csv(file_path, delimiter=';')
    prepared_data_strings: [str] = [get_column_names()]

    for row in data.values:
        new_row: list = copy.deepcopy(row).tolist()
        new_row.pop(10)
        new_row.pop(10)

        if row[10] == "DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions" and \
                row[11] == "ParallelImprovedLocalSearchBestImprovement":
            new_row.append(1)

        if row[10] == "ExhaustingPairwiseCombination" and \
                row[11] == "ParallelImprovedLocalSearchBestImprovement":
            new_row.append(2)

        if row[10] == "DeletionPartsOfTheFirstSolutionAndFillWithPartsOfTheOtherSolutions" and \
                row[11] == "ParallelImprovedLocalSearchFirstImprovement":
            new_row.append(3)

        if row[10] == "ExhaustingPairwiseCombination" and \
                row[11] == "ParallelImprovedLocalSearchFirstImprovement":
            new_row.append(4)

        diff = round(calculate_gmean([(row[4] / row[5])]), 2)
        # diff = round(((row[4] / row[5]) - 1) * 100, 2)
        new_row[6] = diff

        prepared_data_strings.append(new_row)

    csv_array = np.asarray(prepared_data_strings)
    new_file_path = file_path[:-4]
    new_file_path = new_file_path + "_prepared.csv"
    np.savetxt(new_file_path, csv_array, fmt="%s", delimiter=";")
    return new_file_path


def merge_csv_files_to_compare_diff(file_paths: list[str]):
    data = None
    diff_counter = 1
    default_column_name = "Difference"
    for path in file_paths:
        new_data = pd.read_csv(path, delimiter=';')

        # print("mean:" + str(new_data[default_column_name].mean()))
        # print("median:" + str(new_data[default_column_name].median()))
        adjusted_value_for_gmean = [(v + 100) / 100 for v in new_data[default_column_name].values]
        print("geo mean:" + str(calculate_gmean(adjusted_value_for_gmean)))

        if data is None:
            data = copy.deepcopy(new_data)
            diff_counter
            continue

        data[default_column_name + "_" + str(diff_counter)] = new_data[default_column_name]
        diff_counter += 1

    new_file_path = file_paths[0][:-4]
    new_file_path = new_file_path + "_merged.csv"
    data.to_csv(new_file_path, sep=";", index=False)


def calculate_mean_and_median_for_csv(file_path: str):
    data = pd.read_csv(file_path, delimiter=";")
    mean_data = []

    group_by_instances = data.groupby("Instance Name")
    for instance in group_by_instances:
        group_by_test_setting = instance[1].groupby("Test Setting")

        for test_setting in group_by_test_setting:
            adjusted_values = [(v + 100) / 100 for v in test_setting[1]["Difference"].values]
            mean = calculate_gmean(adjusted_values)
            median = test_setting[1]["Difference"].median()
            mean_data.append([
                instance[0],
                test_setting[0],
                round(mean, 2),
                round(median, 2)
            ])

    mean_data_frame = pd.DataFrame(mean_data)
    mean_data_frame.columns = ["Instance Name", "Test Setting", "Mean", "Median"]
    new_file_path = file_path[:-4]
    new_file_path = new_file_path + "_mean_median.csv"
    mean_data_frame.to_csv(new_file_path, sep=";", index=False)
    return new_file_path


def reduce_csv_to_optimization_scenario_instances(file_path: str):
    scenario_names = [
        "chr15b.dat",
        "chr25a.dat",
        "esc128.dat",
        "tai256c.dat"
    ]
    data = pd.read_csv(file_path, sep=";")
    filtered_data = data[data["Instance Name"].isin(scenario_names)]

    new_file_path = file_path[:-4]
    new_file_path = new_file_path + "_optimization.csv"
    filtered_data.to_csv(new_file_path, sep=";", index=False)
    return new_file_path


def merge_csv_files_for_different_tests(file_paths: {str, str}):
    data = None

    for file_path, test_name in file_paths.items():
        new_data = pd.read_csv(file_path, sep=";")
        new_column = []
        for row in new_data.values:
            new_column.append(test_name)

        new_data["Test Name"] = new_column

        if data is None:
            data = new_data
            continue

        data = pd.concat([data, new_data])

    new_file_path = "C:/Users/stefa/OneDrive/Documents/_Private/MasterArbeit/Results/optimization_comparison.csv"
    data.to_csv(new_file_path, sep=";", index=False)
    return new_file_path


def calculate_gmean(values: list) -> float:
    result = 1
    for i in values:
        if i < 0:
            continue
        result = result * i

    return round((result ** (1 / len(values))) - 1, 4) * 100


def get_column_names() -> list:
    column_names = [
        "Instance Name",
        "N",
        "Reference Size",
        "Population Size",
        "Found Optimum",
        "Known Optimum",
        "Difference",
        "Time",
        "Iterations",
        "Permutations",
        "Test Setting"
    ]

    return column_names

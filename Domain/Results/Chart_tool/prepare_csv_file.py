import copy

import pandas as pd
import numpy as np

def prepare_csv(file_path:str):
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

        diff = round(((row[4] / row[5]) - 1) * 100, 2)
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

        print("mean:" + str(new_data[default_column_name].mean()))
        print("median:" + str(new_data[default_column_name].median()))
        # print("geo mean:" + str(calculate_gmean(new_data[default_column_name].values)))

        if data is None:
            data = copy.deepcopy(new_data)
            diff_counter
            continue

        data[default_column_name + "_" + str(diff_counter)] = new_data[default_column_name]
        diff_counter += 1

    new_file_path = file_paths[0][:-4]
    new_file_path = new_file_path + "_merged.csv"
    data.to_csv(new_file_path, sep=";", index=False)


def calculate_gmean(values: list) -> float:
    result = 1
    for i in values:
        if i < 0:
            continue
        result = result * i

    return result**(1/len(values))

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

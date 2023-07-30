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

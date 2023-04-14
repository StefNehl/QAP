﻿using Domain.Models;
using QAPAlgorithms.Contracts;

namespace QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

/*
|                 Method | SolutionName | NrOfCalls |              Mean |            Error |           StdDev |        Gen0 |        Gen1 |        Gen2 |      Allocated |
|----------------------- |------------- |---------- |------------------:|-----------------:|-----------------:|------------:|------------:|------------:|---------------:|
|              SubSetGen |   chr12a.dat |        10 |          44.41 us |         0.862 us |         1.059 us |      1.7700 |           - |           - |       89.67 KB |
|     SubSetGen_Parallel |   chr12a.dat |        10 |         123.82 us |         1.593 us |         1.490 us |      2.9297 |      1.2207 |           - |      143.85 KB |
|          PathRelinking |   chr12a.dat |        10 |       3,742.69 us |        74.810 us |        91.873 us |     42.9688 |      7.8125 |           - |     2255.86 KB |
| PathRelinking_Parallel |   chr12a.dat |        10 |       1,043.00 us |        14.848 us |        13.889 us |     52.7344 |      7.8125 |           - |     2644.61 KB |
|            Combination |   chr12a.dat |        10 |       3,942.85 us |        77.161 us |        75.782 us |     54.6875 |     11.7188 |           - |     2780.76 KB |
|   Combination_Parallel |   chr12a.dat |        10 |       1,130.51 us |        19.660 us |        18.390 us |     62.5000 |     15.6250 |           - |     3091.52 KB |
|              SubSetGen |   chr12a.dat |       100 |         455.52 us |         9.072 us |         9.317 us |     18.0664 |           - |           - |      896.68 KB |
|     SubSetGen_Parallel |   chr12a.dat |       100 |       1,242.76 us |         8.150 us |         7.624 us |     29.2969 |     13.6719 |           - |     1438.48 KB |
|          PathRelinking |   chr12a.dat |       100 |      37,042.58 us |       727.580 us |       680.579 us |    428.5714 |     71.4286 |           - |     23702.4 KB |
| PathRelinking_Parallel |   chr12a.dat |       100 |      10,533.39 us |       140.546 us |       124.591 us |    562.5000 |     78.1250 |           - |    27840.62 KB |
|            Combination |   chr12a.dat |       100 |      38,121.24 us |       392.336 us |       366.991 us |    500.0000 |     71.4286 |           - |    26807.67 KB |
|   Combination_Parallel |   chr12a.dat |       100 |      10,721.99 us |        67.158 us |        62.820 us |    625.0000 |    156.2500 |           - |    30527.47 KB |
|              SubSetGen |   chr12a.dat |       200 |         864.34 us |         4.521 us |         4.008 us |     36.1328 |           - |           - |     1793.36 KB |
|     SubSetGen_Parallel |   chr12a.dat |       200 |       2,438.44 us |        29.800 us |        27.875 us |     58.5938 |     27.3438 |           - |     2876.96 KB |
|          PathRelinking |   chr12a.dat |       200 |      70,983.72 us |       746.599 us |       698.369 us |    875.0000 |    125.0000 |           - |    46323.53 KB |
| PathRelinking_Parallel |   chr12a.dat |       200 |      20,169.69 us |       168.598 us |       157.707 us |   1062.5000 |    156.2500 |           - |    52826.56 KB |
|            Combination |   chr12a.dat |       200 |      77,834.18 us |       817.107 us |       764.323 us |   1142.8571 |    285.7143 |           - |    56393.47 KB |
|   Combination_Parallel |   chr12a.dat |       200 |      21,299.57 us |        21.509 us |        17.961 us |   1218.7500 |    312.5000 |           - |    59567.46 KB |
|              SubSetGen |   chr25a.dat |        10 |          43.06 us |         0.347 us |         0.325 us |      1.7700 |           - |           - |       89.67 KB |
|     SubSetGen_Parallel |   chr25a.dat |        10 |         115.83 us |         2.284 us |         2.444 us |      2.9297 |      1.3428 |           - |      143.85 KB |
|          PathRelinking |   chr25a.dat |        10 |      27,484.25 us |       226.712 us |       212.066 us |    156.2500 |     62.5000 |           - |     9181.59 KB |
| PathRelinking_Parallel |   chr25a.dat |        10 |       4,295.68 us |         4.966 us |         4.402 us |    187.5000 |     70.3125 |           - |     9365.09 KB |
|            Combination |   chr25a.dat |        10 |      27,258.52 us |        41.606 us |        32.483 us |    187.5000 |     93.7500 |           - |     9763.83 KB |
|   Combination_Parallel |   chr25a.dat |        10 |       4,494.96 us |         5.312 us |         4.436 us |    203.1250 |    101.5625 |           - |     9912.53 KB |
|              SubSetGen |   chr25a.dat |       100 |         430.80 us |         2.603 us |         2.435 us |     18.0664 |           - |           - |      896.68 KB |
|     SubSetGen_Parallel |   chr25a.dat |       100 |       1,210.67 us |         8.437 us |         6.587 us |     29.2969 |     13.6719 |           - |     1438.48 KB |
|          PathRelinking |   chr25a.dat |       100 |     268,223.80 us |     1,456.730 us |     1,362.626 us |   1500.0000 |    500.0000 |           - |    91242.57 KB |
| PathRelinking_Parallel |   chr25a.dat |       100 |      42,517.64 us |        63.816 us |        49.823 us |   1833.3333 |    833.3333 |           - |    92828.19 KB |
|            Combination |   chr25a.dat |       100 |     273,194.86 us |     2,035.972 us |     1,904.450 us |   1500.0000 |    500.0000 |           - |    97584.57 KB |
|   Combination_Parallel |   chr25a.dat |       100 |      45,052.54 us |       249.192 us |       220.902 us |   2000.0000 |   1000.0000 |           - |    99072.14 KB |
|              SubSetGen |   chr25a.dat |       200 |         862.95 us |         4.450 us |         4.163 us |     36.1328 |           - |           - |     1793.36 KB |
|     SubSetGen_Parallel |   chr25a.dat |       200 |       2,444.93 us |        41.364 us |        38.692 us |     58.5938 |     27.3438 |           - |     2876.96 KB |
|          PathRelinking |   chr25a.dat |       200 |     539,477.66 us |     4,015.413 us |     3,756.019 us |   3000.0000 |   1000.0000 |           - |   180874.21 KB |
| PathRelinking_Parallel |   chr25a.dat |       200 |      87,761.15 us |        62.334 us |        48.667 us |   3833.3333 |   1666.6667 |           - |      188522 KB |
|            Combination |   chr25a.dat |       200 |     539,132.51 us |     7,094.362 us |     5,924.114 us |   3000.0000 |   1000.0000 |           - |   192722.26 KB |
|   Combination_Parallel |   chr25a.dat |       200 |      92,153.94 us |       380.115 us |       336.962 us |   4000.0000 |   2000.0000 |           - |   200038.03 KB |
|              SubSetGen |  tai256c.dat |        10 |          42.70 us |         0.218 us |         0.204 us |      1.7700 |           - |           - |       89.67 KB |
|     SubSetGen_Parallel |  tai256c.dat |        10 |         123.11 us |         1.645 us |         1.539 us |      2.9297 |      1.3428 |           - |      143.85 KB |
|          PathRelinking |  tai256c.dat |        10 |  23,754,607.86 us |   316,341.396 us |   295,905.938 us |  16000.0000 |  15000.0000 |   5000.0000 |   593599.88 KB |
| PathRelinking_Parallel |  tai256c.dat |        10 |   3,279,408.51 us |    56,568.854 us |    52,914.541 us |  17000.0000 |  16000.0000 |   5000.0000 |   598679.28 KB |
|            Combination |  tai256c.dat |        10 |  23,314,773.61 us |    43,217.373 us |    38,311.075 us |  15000.0000 |  14000.0000 |   5000.0000 |   598489.15 KB |
|   Combination_Parallel |  tai256c.dat |        10 |   3,191,051.72 us |    17,362.684 us |    16,241.065 us |  17000.0000 |  16000.0000 |   5000.0000 |    603177.7 KB |
|              SubSetGen |  tai256c.dat |       100 |         427.43 us |         2.344 us |         1.957 us |     18.0664 |           - |           - |      896.68 KB |
|     SubSetGen_Parallel |  tai256c.dat |       100 |       1,230.54 us |         7.339 us |         6.864 us |     29.2969 |     13.6719 |           - |     1438.48 KB |
|          PathRelinking |  tai256c.dat |       100 | 231,082,146.85 us |   161,078.488 us |   150,672.918 us | 169000.0000 | 168000.0000 |  57000.0000 |  5928708.95 KB |
| PathRelinking_Parallel |  tai256c.dat |       100 |  32,216,436.01 us |   138,352.428 us |   115,530.544 us | 193000.0000 | 192000.0000 |  72000.0000 |  5974806.55 KB |
|            Combination |  tai256c.dat |       100 | 237,359,035.28 us | 1,256,767.791 us | 1,114,091.912 us | 171000.0000 | 170000.0000 |  63000.0000 |  5977330.97 KB |
|   Combination_Parallel |  tai256c.dat |       100 |  32,215,206.16 us |   193,295.213 us |   180,808.462 us | 193000.0000 | 192000.0000 |  74000.0000 |  6042367.91 KB |
|              SubSetGen |  tai256c.dat |       200 |         843.95 us |         4.732 us |         4.195 us |     36.1328 |           - |           - |     1793.36 KB |
|     SubSetGen_Parallel |  tai256c.dat |       200 |       2,471.22 us |        19.993 us |        18.701 us |     58.5938 |     27.3438 |           - |     2876.96 KB |
|          PathRelinking |  tai256c.dat |       200 | 460,727,184.39 us |   206,126.265 us |   192,810.636 us | 335000.0000 | 334000.0000 | 116000.0000 |  11856178.6 KB |
| PathRelinking_Parallel |  tai256c.dat |       200 |  63,820,733.37 us |   454,510.420 us |   425,149.329 us | 389000.0000 | 388000.0000 | 147000.0000 | 11973411.65 KB |
|            Combination |  tai256c.dat |       200 | 465,081,337.17 us |   867,644.234 us |   769,144.014 us | 356000.0000 | 355000.0000 | 126000.0000 | 11964597.85 KB |
|   Combination_Parallel |  tai256c.dat |       200 |  64,212,406.93 us |   441,516.904 us |   412,995.186 us | 386000.0000 | 385000.0000 | 149000.0000 | 12081911.74 KB |
 */

public class PathRelinking : ISolutionGenerationMethod
{
    private QAPInstance? _qapInstance;
    private readonly IImprovementMethod _improvementMethod;
    private readonly int _improveEveryNSolutions;

    private int _improvementCount;

    
    public PathRelinking(IImprovementMethod improvementMethod, int improveEveryNSolutions = 100)
    {
        _improvementMethod = improvementMethod;
        _improveEveryNSolutions = improveEveryNSolutions;
    }

    public void InitMethod(QAPInstance instance)
    {
        _improvementCount = 0;
        _qapInstance = instance;
    }
    
    public List<InstanceSolution> GeneratePathAndGetSolutions(InstanceSolution startingSolution, InstanceSolution guidingSolution)
    {
        var newSolutions = new List<InstanceSolution>();

        var newHashCode = startingSolution.HashCode;
        var newPermutation = startingSolution.SolutionPermutation.ToArray();
        while (newHashCode != guidingSolution.HashCode)
        {
            AddAttributeToSolutionFromGuidingSolution(newPermutation, guidingSolution.SolutionPermutation);
            var newSolution = new InstanceSolution(_qapInstance, newPermutation.ToArray());
            newHashCode = newSolution.HashCode;
            newSolutions.Add(newSolution);
        }
        
        if (_improvementCount == _improveEveryNSolutions)
            _improvementMethod.ImproveSolutions(newSolutions);
        _improvementCount++;
        
        return newSolutions;
    }

    private static void AddAttributeToSolutionFromGuidingSolution(int[] permutation, int[] guidingPermutation)
    {
        var notCorrectIndices = new List<int>();
        for (int i = 0; i < permutation.Length; i++)
        {
            if (permutation[i] == guidingPermutation[i])
                continue;
            
            notCorrectIndices.Add(i);
        }

        var indexForSwap = notCorrectIndices[0];
        var correctValueForIndex = guidingPermutation[indexForSwap];
        var indexOfCorrectValueInPermutation = -1;

        foreach (var notCorrectIndex in notCorrectIndices)
        {
            if (correctValueForIndex != permutation[notCorrectIndex])
                continue;
                
            indexOfCorrectValueInPermutation = notCorrectIndex;
            break;
        }
        
        (permutation[indexForSwap], permutation[indexOfCorrectValueInPermutation]) = (permutation[indexOfCorrectValueInPermutation], permutation[indexForSwap]);
    }


    public List<InstanceSolution> GetSolutions(List<InstanceSolution> referenceSolutions)
    {
        var newSolutions = new List<InstanceSolution>();
        
        for(int i = 0; i < referenceSolutions.Count; i++)
        {
            for (int j = i; j < referenceSolutions.Count; j++)
            {
                newSolutions.AddRange(GeneratePathAndGetSolutions(referenceSolutions[i], referenceSolutions[j]));
                newSolutions.AddRange(GeneratePathAndGetSolutions(referenceSolutions[j], referenceSolutions[i]));
            }
        }

        return newSolutions;
    }
}
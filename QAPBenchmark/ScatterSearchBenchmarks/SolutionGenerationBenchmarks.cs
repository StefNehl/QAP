using BenchmarkDotNet.Attributes;
using Domain.Models;
using QAPAlgorithms.ScatterSearch;
using QAPAlgorithms.ScatterSearch.CombinationMethods;
using QAPAlgorithms.ScatterSearch.ImprovementMethods;
using QAPAlgorithms.ScatterSearch.InitGenerationMethods;
using QAPAlgorithms.ScatterSearch.SolutionGenerationMethods;

namespace QAPBenchmark.ScatterSearchBenchmarks;

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

After Hashset change

|                 Method | SolutionName | NrOfCalls |              Mean |            Error |           StdDev |            Median |        Gen0 |        Gen1 |        Gen2 |      Allocated |
|----------------------- |------------- |---------- |------------------:|-----------------:|-----------------:|------------------:|------------:|------------:|------------:|---------------:|
|              SubSetGen |   chr12a.dat |        10 |          72.05 us |         1.390 us |         1.487 us |          72.09 us |      2.1973 |           - |           - |      110.64 KB |
|     SubSetGen_Parallel |   chr12a.dat |        10 |         130.40 us |         1.079 us |         0.957 us |         130.21 us |      3.1738 |      1.4648 |           - |      164.83 KB |
|          PathRelinking |   chr12a.dat |        10 |       4,046.79 us |        50.267 us |        44.561 us |       4,031.76 us |     46.8750 |      7.8125 |           - |     2678.05 KB |
| PathRelinking_Parallel |   chr12a.dat |        10 |       2,304.75 us |        45.865 us |        61.229 us |       2,306.82 us |     70.3125 |     15.6250 |           - |     3376.34 KB |
|            Combination |   chr12a.dat |        10 |       4,090.47 us |        80.719 us |       107.757 us |       4,023.63 us |     46.8750 |      7.8125 |           - |     2643.31 KB |
|   Combination_Parallel |   chr12a.dat |        10 |       2,581.41 us |        51.455 us |        83.090 us |       2,579.90 us |     74.2188 |     19.5313 |           - |      3659.3 KB |
|              SubSetGen |   chr12a.dat |       100 |         734.38 us |         3.268 us |         2.897 us |         734.44 us |     22.4609 |           - |           - |     1106.45 KB |
|     SubSetGen_Parallel |   chr12a.dat |       100 |       1,320.53 us |        13.533 us |        12.658 us |       1,322.17 us |     33.2031 |     15.6250 |           - |     1648.25 KB |
|          PathRelinking |   chr12a.dat |       100 |      41,755.46 us |       524.222 us |       437.749 us |      41,839.84 us |    538.4615 |     76.9231 |           - |    26903.97 KB |
| PathRelinking_Parallel |   chr12a.dat |       100 |      24,627.81 us |       480.728 us |       572.273 us |      24,837.60 us |    718.7500 |    156.2500 |           - |    34935.96 KB |
|            Combination |   chr12a.dat |       100 |      41,746.75 us |       464.625 us |       411.878 us |      41,802.84 us |    538.4615 |     76.9231 |           - |    26907.29 KB |
|   Combination_Parallel |   chr12a.dat |       100 |      24,985.56 us |       402.727 us |       590.313 us |      24,862.08 us |    812.5000 |    250.0000 |           - |    39595.46 KB |
|              SubSetGen |   chr12a.dat |       200 |       1,549.33 us |        30.855 us |        27.352 us |       1,560.13 us |     44.9219 |           - |           - |     2212.89 KB |
|     SubSetGen_Parallel |   chr12a.dat |       200 |       2,599.64 us |        24.950 us |        23.339 us |       2,596.71 us |     66.4063 |     31.2500 |           - |      3296.5 KB |
|          PathRelinking |   chr12a.dat |       200 |      81,222.62 us |     1,247.951 us |     1,106.276 us |      81,296.94 us |   1000.0000 |    142.8571 |           - |    53089.17 KB |
| PathRelinking_Parallel |   chr12a.dat |       200 |      49,875.13 us |       982.778 us |     1,092.355 us |      49,895.89 us |   1400.0000 |    300.0000 |           - |    70081.79 KB |
|            Combination |   chr12a.dat |       200 |      89,398.34 us |     1,348.082 us |     1,195.040 us |      89,812.43 us |   1333.3333 |    166.6667 |           - |    69055.21 KB |
|   Combination_Parallel |   chr12a.dat |       200 |      55,291.36 us |     1,103.251 us |     1,582.250 us |      54,949.25 us |   1800.0000 |    500.0000 |           - |    85794.27 KB |
|              SubSetGen |   chr25a.dat |        10 |          78.47 us |         1.323 us |         1.237 us |          78.82 us |      2.5635 |           - |           - |      130.41 KB |
|     SubSetGen_Parallel |   chr25a.dat |        10 |         130.90 us |         1.570 us |         1.468 us |         131.18 us |      3.6621 |      1.7090 |           - |      184.59 KB |
|          PathRelinking |   chr25a.dat |        10 |      29,233.31 us |       107.162 us |       100.240 us |      29,216.06 us |    187.5000 |     62.5000 |           - |     9345.96 KB |
| PathRelinking_Parallel |   chr25a.dat |        10 |       6,398.73 us |       119.385 us |       111.673 us |       6,355.22 us |    250.0000 |    132.8125 |           - |    12099.31 KB |
|            Combination |   chr25a.dat |        10 |      29,802.31 us |       137.435 us |       128.557 us |      29,808.40 us |    187.5000 |     62.5000 |           - |    10624.81 KB |
|   Combination_Parallel |   chr25a.dat |        10 |       6,886.61 us |        77.729 us |        72.708 us |       6,892.12 us |    257.8125 |    164.0625 |           - |    12716.97 KB |
|              SubSetGen |   chr25a.dat |       100 |         808.73 us |         4.144 us |         3.460 us |         808.83 us |     26.3672 |           - |           - |      1304.1 KB |
|     SubSetGen_Parallel |   chr25a.dat |       100 |       1,313.74 us |        13.635 us |        12.087 us |       1,314.48 us |     37.1094 |     17.5781 |           - |      1845.9 KB |
|          PathRelinking |   chr25a.dat |       100 |     284,779.19 us |     5,107.640 us |     4,777.690 us |     286,905.70 us |   1500.0000 |    500.0000 |           - |    93169.92 KB |
| PathRelinking_Parallel |   chr25a.dat |       100 |      61,681.20 us |     1,010.859 us |       945.558 us |      61,496.27 us |   2333.3333 |   1111.1111 |           - |    115387.9 KB |
|            Combination |   chr25a.dat |       100 |     292,749.01 us |     5,317.903 us |     4,974.370 us |     295,446.95 us |   2000.0000 |   1000.0000 |           - |   107459.18 KB |
|   Combination_Parallel |   chr25a.dat |       100 |      68,708.94 us |       280.010 us |       218.613 us |      68,644.29 us |   2750.0000 |   1500.0000 |           - |   137288.96 KB |
|              SubSetGen |   chr25a.dat |       200 |       1,545.12 us |        21.554 us |        20.162 us |       1,547.67 us |     52.7344 |           - |           - |     2608.21 KB |
|     SubSetGen_Parallel |   chr25a.dat |       200 |       2,634.24 us |        27.632 us |        25.847 us |       2,646.76 us |     74.2188 |     35.1563 |           - |      3691.8 KB |
|          PathRelinking |   chr25a.dat |       200 |     580,610.90 us |    11,116.308 us |    10,917.701 us |     587,826.80 us |   3000.0000 |   1000.0000 |           - |    187716.4 KB |
| PathRelinking_Parallel |   chr25a.dat |       200 |     123,330.38 us |       917.169 us |       857.921 us |     123,200.70 us |   4800.0000 |   2800.0000 |           - |   238816.41 KB |
|            Combination |   chr25a.dat |       200 |     604,948.46 us |    11,551.083 us |    10,804.890 us |     610,648.80 us |   4000.0000 |   1000.0000 |           - |   219088.66 KB |
|   Combination_Parallel |   chr25a.dat |       200 |     138,503.77 us |     2,691.555 us |     3,773.184 us |     137,236.83 us |   5250.0000 |   2500.0000 |           - |   261525.22 KB |
|              SubSetGen |  tai256c.dat |        10 |          89.01 us |         1.774 us |         3.781 us |          90.60 us |           - |           - |           - |      139.87 KB |
|     SubSetGen_Parallel |  tai256c.dat |        10 |         133.99 us |         0.897 us |         0.839 us |         134.26 us |      3.6621 |      1.7090 |           - |      188.42 KB |
|          PathRelinking |  tai256c.dat |        10 |  23,723,679.37 us |    75,646.365 us |    63,168.141 us |  23,720,253.40 us |  16000.0000 |  15000.0000 |   7000.0000 |   602214.84 KB |
| PathRelinking_Parallel |  tai256c.dat |        10 |   3,262,366.45 us |    19,991.838 us |    18,700.378 us |   3,261,248.70 us |  16000.0000 |  15000.0000 |   5000.0000 |   634851.97 KB |
|            Combination |  tai256c.dat |        10 |  23,673,114.67 us |   128,034.867 us |   119,763.894 us |  23,710,502.30 us |  20000.0000 |  18000.0000 |   9000.0000 |   627118.73 KB |
|   Combination_Parallel |  tai256c.dat |        10 |   3,329,925.94 us |    33,392.827 us |    29,601.871 us |   3,327,832.75 us |  19000.0000 |  18000.0000 |   7000.0000 |   661172.32 KB |
|              SubSetGen |  tai256c.dat |       100 |         822.71 us |         6.329 us |         4.941 us |         822.50 us |           - |           - |           - |     1343.48 KB |
|     SubSetGen_Parallel |  tai256c.dat |       100 |       1,314.55 us |        20.053 us |        18.757 us |       1,307.18 us |     37.1094 |     17.5781 |           - |     1884.18 KB |
|          PathRelinking |  tai256c.dat |       100 | 242,230,381.86 us | 4,318,920.546 us | 4,039,920.958 us | 239,836,950.30 us | 174000.0000 | 173000.0000 |  75000.0000 |  6014619.37 KB |
| PathRelinking_Parallel |  tai256c.dat |       100 |  33,219,590.65 us |   246,825.535 us |   230,880.759 us |  33,153,858.30 us | 197000.0000 | 196000.0000 |  78000.0000 |  6361155.79 KB |
|            Combination |  tai256c.dat |       100 | 237,153,855.93 us | 1,087,403.917 us | 1,017,158.299 us | 237,202,597.10 us | 212000.0000 | 210000.0000 | 103000.0000 |  6272515.12 KB |
|   Combination_Parallel |  tai256c.dat |       100 |  33,709,536.06 us |   206,063.177 us |   192,751.623 us |  33,761,832.90 us | 222000.0000 | 213000.0000 |  94000.0000 |  6611875.97 KB |
|              SubSetGen |  tai256c.dat |       200 |       1,644.71 us |        13.116 us |        10.240 us |       1,649.05 us |           - |           - |           - |     2685.87 KB |
|     SubSetGen_Parallel |  tai256c.dat |       200 |       2,627.90 us |        17.933 us |        15.897 us |       2,626.30 us |     74.2188 |     35.1563 |           - |     3768.36 KB |
|          PathRelinking |  tai256c.dat |       200 | 471,215,399.19 us | 1,971,261.847 us | 1,843,919.554 us | 470,792,754.70 us | 352000.0000 | 351000.0000 | 152000.0000 | 12047757.06 KB |
| PathRelinking_Parallel |  tai256c.dat |       200 |  66,055,507.39 us |   240,197.929 us |   224,681.291 us |  66,044,859.60 us | 395000.0000 | 394000.0000 | 151000.0000 | 12684343.16 KB |
|            Combination |  tai256c.dat |       200 | 473,935,657.57 us | 1,699,298.525 us | 1,589,524.894 us | 472,872,225.70 us | 420000.0000 | 418000.0000 | 201000.0000 | 12562743.62 KB |
|   Combination_Parallel |  tai256c.dat |       200 |  66,654,552.59 us |   164,110.692 us |   153,509.243 us |  66,643,760.40 us | 449000.0000 | 436000.0000 | 197000.0000 | 13201136.42 KB |


 */

[MemoryDiagnoser]
public class SolutionGenerationBenchmarks
{
    private PathRelinking _pathRelinking;
    private ParallelPathRelinking _parallelPathRelinking;
    
    private ParallelSubSetGeneration _parallelSubSetGeneration;
    private SubSetGeneration _subSetGeneration;
    
    private PathRelinkingSubSetGenerationCombined _pathRelinkingSubSetGenerationCombined;
    private ParallelPathRelinkingSubSetGenerationCombined _parallelPathRelinkingSubSetGenerationCombined;
    
    
    private List<InstanceSolution> _referenceSet;
    
    [Params(10, 100, 200)]
    public int NrOfCalls { get; set; } = 1;

    [Params("chr12a.dat", "chr25a.dat", "tai256c.dat")]
    public string SolutionName = "tai256c.dat";


    [GlobalSetup] 
    public async Task Setup()
    {
        await Init(SolutionName);
    }

    private async Task Init(string instanceName)
    {
        var instanceReader = QAPInstanceReader.QAPInstanceReader.GetInstance();
        var instance = await instanceReader.ReadFileAsync("QAPLIB", instanceName);
        
        var generationMethod = new RandomGeneratedPopulation();
        generationMethod.InitMethod(instance);

        var solutions = generationMethod.GeneratePopulation(10);
        _referenceSet = new List<InstanceSolution>();
        _referenceSet.AddRange(solutions);
        
        var improvementMethod = new ImprovedLocalSearchBestImprovement();
        improvementMethod.InitMethod(instance);
        var combinationMethod = new ExhaustingPairwiseCombination();
        combinationMethod.InitMethod(instance);
        
        _subSetGeneration = new SubSetGeneration( 1, SubSetGenerationMethodType.Cycle, combinationMethod, improvementMethod);
        _subSetGeneration.InitMethod(instance);
        _parallelSubSetGeneration = new ParallelSubSetGeneration( 1, SubSetGenerationMethodType.Cycle, combinationMethod, improvementMethod);
        _parallelSubSetGeneration.InitMethod(instance);
        
        _pathRelinking = new PathRelinking(improvementMethod);
        _pathRelinking.InitMethod(instance);
        _parallelPathRelinking = new ParallelPathRelinking(improvementMethod);
        _parallelPathRelinking.InitMethod(instance);
        
        _pathRelinkingSubSetGenerationCombined = new PathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _pathRelinkingSubSetGenerationCombined.InitMethod(instance);
        _parallelPathRelinkingSubSetGenerationCombined = new ParallelPathRelinkingSubSetGenerationCombined(1, SubSetGenerationMethodType.Cycle, improvementMethod, combinationMethod);
        _parallelPathRelinkingSubSetGenerationCombined.InitMethod(instance);
    }
    
    [Benchmark]
    public void SubSetGen()
    {
        for (int i = 0; i < NrOfCalls; i++)
        {
            var solutions = _subSetGeneration.GetSolutions(_referenceSet);
        }
    }
    
    [Benchmark]
    public void SubSetGen_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
        {
            _parallelSubSetGeneration.GetSolutions(_referenceSet);
        }
    }
    
    [Benchmark]
    public void PathRelinking()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _pathRelinking.GetSolutions(_referenceSet);
    }
        
    [Benchmark]
    public void PathRelinking_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _parallelPathRelinking.GetSolutions(_referenceSet);
    }
    
    [Benchmark]
    public void Combination()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _pathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet);
    }
        
    [Benchmark]
    public void Combination_Parallel()
    {
        for(int i = 0; i < NrOfCalls; i++)
            _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet);
    }

    public void GetNrOfSolutions()
    {
        var instanceNames = new []
        {
            "chr12a.dat", 
            "chr25a.dat", 
            "tai256c.dat"
        };

        foreach (var instanceName in instanceNames)
        {
            Console.WriteLine(instanceName);
            Init(instanceName).Wait();
            Console.WriteLine("Subset generation: " + _subSetGeneration.GetSolutions(_referenceSet).Count);
        
            Init(instanceName).Wait();
            Console.WriteLine("Parallel subset generation: " + _parallelSubSetGeneration.GetSolutions(_referenceSet).Count);
        
            Init(instanceName).Wait();
            Console.WriteLine("Path relinking: " + _pathRelinking.GetSolutions(_referenceSet).Count);
        
            Init(instanceName).Wait();
            Console.WriteLine("Parallel Path relinking: " + _parallelPathRelinking.GetSolutions(_referenceSet).Count);
        
            Init(instanceName).Wait();
            Console.WriteLine("Combined: " + _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet).Count);
        
            Init(instanceName).Wait();
            Console.WriteLine("Parallel Combined: " + _parallelPathRelinkingSubSetGenerationCombined.GetSolutions(_referenceSet).Count);
        }

        
    }
}
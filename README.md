Introduced by Koopmans and Beckmann in 1957, the Quadratic Assignment Problem (QAP)
represents one of the pivotal challenges in combinatorial optimization, serving as the mathematical backbone for numerous applications. Classified as NP-hard, finding optimal solutions
for instances exceeding size 20 has proven elusive.

Furthermore, the traditional algorithm Branch and Bound demonstrates its inefficiency for this problem, primarily due to the absence of effective bounding strategies for the QAP. Many difficult problems of the combinatorial optimization, the QAP included, led to the introduction of Meta-heuristic over the last few decades. Among them, is Scatter Search from the family of evolutionary algorithms. Meta-heuristics serve as overarching frameworks designed primarily to counter the limitations of improvement methods, permitting sub-optimal moves to surpass
local peaks. Notably, the flexible structure of Scatter Search enables the exploration of various strategies during its execution. Its basic form of Scatter Search contains a Diversification Method, an Improvement Method, a Reference Set Update Method, a Subset Generation Method
and a Solution Combination Method.

This thesis analysed different algorithms for the methods used in Scatter Search. Moreover, an implementation of Path Relinking as an additional Solution Generation Method was implemented. The idea behind Path Relinking is to explore trajectories which connect high-quality
solutions. The implementation was done in C# and followed the Scatter Search architecture introduced by Nebro A. et al. in 2008. In addition to the implementation of the framework, parallelized algorithms were also implemented.

For pre-selecting the combinations of different algorithms, benchmarks were performed to compare the algorithms and verify the efficiency of a parallel computation. 
The Scatter Search framework underwent testing across multiple instances of varying sizes. The test batch comprised 10 instances with known optimal objective values and another 10 for which only a lower bound is known. The initial 10 instances were used for the final algorithm selection and assisted with parameter tuning. Every test was executed with a designated runtime of 10 minutes. In the first test, the Scatter Search algorithm delivered results which were between 0 and 35%
worse than the optimal objective value. However, some instances had results over 100% percent worse than the optimal objective value. Therefore, some improvements were made to the
Scatter Search framework. One was a dynamic adjustment to the reference set size if no good
permutations were found after the Combination Method. This improved the results of those
instances which performed poorly before. After the improvement, the best two combinations of algorithms were chosen and further parameter tuning was performed.

After testing every instance with the Scatter Search framework, the results were between
0 and 30%, with exceptions, above the optimum or lower bound, respectively, within a time
frame of 10 minutes. However, the modularity of the framework allows a combination of various
algorithms, which allows an adjustment to specific problems.

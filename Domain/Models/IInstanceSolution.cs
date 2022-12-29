namespace Domain.Models
{
    public interface IInstanceSolution
    {
        long HashCode { get; }
        int[] SolutionPermutation { get; }
        long SolutionValue { get; }
        void RefreshSolutionValue(QAPInstance instance);
        

    }
}
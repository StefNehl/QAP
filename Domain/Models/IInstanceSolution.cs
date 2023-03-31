namespace Domain.Models
{
    public interface IInstanceSolution
    {
        long HashCode { get; }
        int[] SolutionPermutation { get; }
        long SolutionValue { get; set; }
        void RefreshSolutionValue(QAPInstance instance);
        string DisplayInConsole();

    }
}
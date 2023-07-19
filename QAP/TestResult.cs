using System.Text;

namespace QAP
{
    public record TestResult(
        TestSetting TestSetting, 
        long FoundOptimum, 
        long KnownOptimum,
        long Time, 
        long Iterations,
        int[] Solution)
    {
        public double OptimumDifference { get; } = 1 - ((double)KnownOptimum / FoundOptimum);
        
        public string ToCSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TestSetting.Instance.InstanceName);
            sb.Append(';');
            sb.Append(TestSetting.Instance.N);
            sb.Append(';');
            sb.Append(FoundOptimum);
            sb.Append(';');
            sb.Append(Time);
            sb.Append(';');

            var arrayString = new StringBuilder();
            arrayString.Append('[');
            for(int i = 0; i < Solution.Length; i++)
            {
                arrayString.Append(Solution[i]);

                if(i < Solution.Length - 1) 
                    arrayString.Append(", ");
            }
            arrayString.Append(']');

            sb.Append(arrayString);
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TestSetting.Instance.InstanceName);
            sb.Append(';');
            sb.Append(TestSetting.Instance.N);
            sb.Append(';');
            sb.Append(FoundOptimum);
            sb.Append(';');
            sb.Append(Time);
            sb.Append(';');
            sb.Append(Iterations);
            sb.Append(';');

            var arrayString = new StringBuilder();
            arrayString.Append('[');
            for (int i = 0; i < Solution.Length; i++)
            {
                arrayString.Append(Solution[i]);

                if (i < Solution.Length - 1)
                    arrayString.Append(", ");
            }
            arrayString.Append(']');

            sb.Append(arrayString.ToString());
            sb.Append(';');
            sb.Append(TestSetting.CombinationMethod.GetType().Name);
            sb.Append(';');
            sb.Append(TestSetting.GenerateInitPopulationMethod.GetType().Name);
            sb.Append(';');
            sb.Append(TestSetting.ImprovementMethod.GetType().Name);
            return sb.ToString();
        }

        public string ToStringColumnNames()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Instance Name");
            sb.Append(';');
            sb.Append('N');
            sb.Append(';');
            sb.Append("Solution Value");
            sb.Append(';');
            sb.Append("Time[s]");
            sb.Append(';');
            sb.Append("Solutions Tried");
            sb.Append(';');
            sb.Append("Permutation");
            sb.Append(';');
            sb.Append("Combination Method");
            sb.Append(';');
            sb.Append("Init Pop Generation");
            sb.Append(';');
            sb.Append("ImprovementMethod");
            return sb.ToString();
        }

        public string ToStringForConsole()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Instance Name: {TestSetting.Instance.InstanceName}");
            sb.AppendLine($"N: {TestSetting.Instance.N}");
            sb.AppendLine($"Found Optimum: {FoundOptimum}");
            sb.AppendLine($"Known Optimum: {KnownOptimum}");
            sb.AppendLine($"Geometric mean: {OptimumDifference}%");
            sb.AppendLine($"Time[s]: {Time}");
            sb.AppendLine($"Iterations: {Iterations}");

            var arrayString = new StringBuilder();
            arrayString.Append('[');
            for (int i = 0; i < Solution.Length; i++)
            {
                arrayString.Append(Solution[i]);

                if (i < Solution.Length - 1)
                    arrayString.Append(", ");
            }
            arrayString.Append(']');

            sb.AppendLine($"Permutation: {arrayString.ToString()}");
            sb.AppendLine($"Combination Method: {TestSetting.CombinationMethod.GetType().Name}");
            sb.AppendLine($"Init Pop Generation: {TestSetting.GenerateInitPopulationMethod.GetType().Name}");
            sb.AppendLine($"Improvement Method: {TestSetting.ImprovementMethod.GetType().Name}");

            return sb.ToString();
        }
    }
}

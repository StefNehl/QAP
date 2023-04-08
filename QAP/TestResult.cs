using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAP
{
    public record TestResult(
        TestSettings TestSettings, 
        long SolutionValue, 
        long Time, 
        long Iterations,
        int[] Solution)
    {
        public string ToCSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TestSettings.Instance.InstanceName);
            sb.Append(";");
            sb.Append(TestSettings.Instance.N);
            sb.Append(";");
            sb.Append(SolutionValue);
            sb.Append(";");
            sb.Append(Time);
            sb.Append(";");

            var arrayString = new StringBuilder();
            arrayString.Append("[");
            for(int i = 0; i < Solution.Length; i++)
            {
                arrayString.Append(Solution[i]);

                if(i < Solution.Length - 1) 
                    arrayString.Append(", ");
            }
            arrayString.Append("]");

            sb.Append(arrayString.ToString());
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TestSettings.Instance.InstanceName);
            sb.Append(";");
            sb.Append(TestSettings.Instance.N);
            sb.Append(";");
            sb.Append(SolutionValue);
            sb.Append(";");
            sb.Append(Time);
            sb.Append(";");
            sb.Append(Iterations);
            sb.Append(";");

            var arrayString = new StringBuilder();
            arrayString.Append("[");
            for (int i = 0; i < Solution.Length; i++)
            {
                arrayString.Append(Solution[i]);

                if (i < Solution.Length - 1)
                    arrayString.Append(", ");
            }
            arrayString.Append("]");

            sb.Append(arrayString.ToString());
            sb.Append(";");
            sb.Append(TestSettings.CombinationMethod.GetType().Name);
            sb.Append(";");
            sb.Append(TestSettings.GenerateInitPopulationMethod.GetType().Name);
            sb.Append(";");
            sb.Append(TestSettings.ImprovementMethod.GetType().Name);
            return sb.ToString();
        }

        public string ToStringColumnNames()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Instance Name");
            sb.Append(";");
            sb.Append("N");
            sb.Append(";");
            sb.Append("Solution Value");
            sb.Append(";");
            sb.Append("Time[s]");
            sb.Append(";");
            sb.Append("Solutions Tried");
            sb.Append(";");
            sb.Append("Permutation");
            sb.Append(";");
            sb.Append("Combination Method");
            sb.Append(";");
            sb.Append("Init Pop Generation");
            sb.Append(";");
            sb.Append("ImprovementMethod");
            return sb.ToString();
        }

        public string ToStringForConsole()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Instance Name: {TestSettings.Instance.InstanceName}");
            sb.AppendLine($"N: {TestSettings.Instance.N}");
            sb.AppendLine($"Solution Value: {SolutionValue}");
            sb.AppendLine($"Time[s]: {Time}");
            sb.AppendLine($"Iterations: {Iterations}");

            var arrayString = new StringBuilder();
            arrayString.Append("[");
            for (int i = 0; i < Solution.Length; i++)
            {
                arrayString.Append(Solution[i]);

                if (i < Solution.Length - 1)
                    arrayString.Append(", ");
            }
            arrayString.Append("]");

            sb.AppendLine($"Permutation: {arrayString.ToString()}");
            sb.AppendLine($"Combination Method: {TestSettings.CombinationMethod.GetType().Name}");
            sb.AppendLine($"Init Pop Generation: {TestSettings.GenerateInitPopulationMethod.GetType().Name}");
            sb.AppendLine($"Improvement Method: {TestSettings.ImprovementMethod.GetType().Name}");



            return sb.ToString();
        }
    }
}

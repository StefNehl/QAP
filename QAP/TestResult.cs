using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAP
{
    public record TestResult(
        string InstanceName, 
        int N, 
        long SolutionValue, 
        long Time, 
        int[] Solution,
        string CombinationMethodName,
        string InitPopulationGenerationMethodName, 
        string ImprovementMethodName)
    {
        public string ToCSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(InstanceName);
            sb.Append(";");
            sb.Append(N);
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
            sb.Append(InstanceName);
            sb.Append(";");
            sb.Append(N);
            sb.Append(";");
            sb.Append(SolutionValue);
            sb.Append(";");
            sb.Append(Time);
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
            sb.Append(CombinationMethodName);
            sb.Append(";");
            sb.Append(InitPopulationGenerationMethodName);
            sb.Append(";");
            sb.Append(ImprovementMethodName);
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
            sb.Append("Permutation");
            sb.Append(";");
            sb.Append("Combination Method");
            sb.Append(";");
            sb.Append("Init Pop Generation");
            sb.Append(";");
            sb.Append("ImprovementMethod");
            return sb.ToString();
        }
    }
}

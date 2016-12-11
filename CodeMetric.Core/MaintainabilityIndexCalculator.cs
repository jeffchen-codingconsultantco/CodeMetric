using System;
using CodeMetric.Core.Halstead;

namespace CodeMetric.Core
{
    public class MaintainabilityIndexCalculator
    {
        public static double CalculateMaintainablityIndex(double cyclomaticComplexity, double linesOfCode, IHalsteadMetrics halsteadMetrics)
        {
            if(linesOfCode.Equals(0.0)
               || halsteadMetrics.NumberOfOperands.Equals(0)
               || halsteadMetrics.NumberOfOperators.Equals(0))
            {
                return 100.0;
            }

            var num = Math.Log(halsteadMetrics.GetVolume());
            var mi = (171 - (5.2 * num)
                      - (0.23 * cyclomaticComplexity)
                      - (16.2 * Math.Log(linesOfCode))
                     ) * 100 / 171;

            return Math.Max(0.0, mi);
        }
    }
}

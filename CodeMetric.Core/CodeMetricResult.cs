namespace CodeMetric.Core
{
    public class CodeMetricResult
    {
        public int CyclomaticComplexity { get; set; }
        public double LineOfCode { get; set; }
        public double MaintainabilityIndex { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetric.Core.Halstead;
using Microsoft.CodeAnalysis;

namespace CodeMetric.Core
{
    public class CodeMetricCalculator
    {
        private LineOfCodeCalculator _locCal;
        private CyclomaticComplexityCalculator _ccCal;
        private MaintainabilityIndexCalculator _miCal;
        
        public CodeMetricCalculator()
        {
            _locCal = new LineOfCodeCalculator();
            _ccCal = new CyclomaticComplexityCalculator();
            _miCal = new MaintainabilityIndexCalculator();
        }

        public CodeMetricResult Calculate(SyntaxNode node)
        {
            var loc = _locCal.Calculate(node);
            var cc = _ccCal.Calculate(node);
            var halsteadAnalyzer = new HalsteadAnalyzer();
            var halsteadMetrics = halsteadAnalyzer.Calculate(node);
            var mi = MaintainabilityIndexCalculator.CalculateMaintainablityIndex(cc, loc, halsteadMetrics);

            return new CodeMetricResult
                       {
                           LineOfCode = loc,
                           CyclomaticComplexity = cc,
                           MaintainabilityIndex = mi
                       };
        }
    }
}

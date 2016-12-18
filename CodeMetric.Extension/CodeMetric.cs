using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CodeMetric.Core;
using CodeMetric.Core.Halstead;
using CodeMetric.Core.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace CodeMetric.Extension
{
    class CodeMetric
    {
        private readonly CodeMetricBarControl _root;
        private readonly IWpfTextView _view;
        private readonly IAdornmentLayer _adornmentLayer;

        public CodeMetric(IWpfTextView view)
        {
            _view = view;
            _root = new CodeMetricBarControl();

            _adornmentLayer = view.GetAdornmentLayer("CodeMetric");

            //_view.ViewportHeightChanged += OnViewSizeChanged;
            //_view.ViewportWidthChanged += OnViewSizeChanged;

            ILayoutChangeProvider layoutChangeProvider = new LayoutChangeProvider();
            CodeMetricTypeProvider.Add<ILayoutChangeProvider>(typeof(ILayoutChangeProvider), layoutChangeProvider);
            _view.LayoutChanged += (sender, args) =>
                                   {
                                       layoutChangeProvider.OnLayoutChanged(sender, args);
                                   };
        }

        private void OnViewSizeChanged(object sender, EventArgs eventArgs)
        {
            _adornmentLayer.RemoveAdornment(_root);

            Canvas.SetLeft(_root, _view.ViewportLeft + 300);
            Canvas.SetTop(_root, _view.ViewportTop + 30);

            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _root, null);
        }

        public void UpdateMetric()
        {
            SnapshotPoint caretPosition = _view.Caret.Position.BufferPosition;
            Document doc = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            var ancestorsAndSelf = doc.GetSyntaxRootAsync().Result.FindToken(caretPosition).Parent.AncestorsAndSelf();
            SyntaxNode targetNode = ancestorsAndSelf.FirstOrDefault(i => i is MethodDeclarationSyntax);
            
            if(targetNode != null)
            {
                var charBounds = _view.GetTextViewLineContainingBufferPosition(caretPosition)
                                      .GetCharacterBounds(caretPosition);
                
                Canvas.SetTop(_root, charBounds.Bottom);
                Canvas.SetRight(_root, charBounds.Right + 100);
                
                //LineOfCode
                var locCalculator = new LineOfCodeCalculator();
                var loc = locCalculator.Calculate(targetNode);
                _root.LblLineOfCode.Content = loc;

                //CyclomaticComplexity
                var cyclomaticCalculaotr = new CyclomaticComplexityCalculator();
                var cyclomaticCounter = cyclomaticCalculaotr.Calculate(targetNode);
                _root.LblCyclomaticComplexity.Content = cyclomaticCounter;

                //MaintainabilityIndex
                var halsteadAnalyzer = new HalsteadAnalyzer();
                var halsteadMetrics = halsteadAnalyzer.Calculate(targetNode);
                var mi = MaintainabilityIndexCalculator.CalculateMaintainablityIndex(cyclomaticCounter, loc, halsteadMetrics);
                _root.LblMaintainabilityIndex.Content = mi.ToString("###");

                //Add into SyntaxNodeCache
                var metricMsg = $"LOC: {loc}, CC: {cyclomaticCounter}, MI: {mi:###}";
                SyntaxNodeCache.Change(targetNode, metricMsg);
            }
            else
            {
                //Reset
                _root.LblLineOfCode.Content = 0;
                _root.LblCyclomaticComplexity.Content = 0;
                _root.LblMaintainabilityIndex.Content = 0;
            }
        }
    }
}

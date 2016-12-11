using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CodeMetric.Core;
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
        private readonly LineOfCodeCalculator _locCalculator;


        public CodeMetric(IWpfTextView view)
        {
            _view = view;
            _root = new CodeMetricBarControl();

            _adornmentLayer = view.GetAdornmentLayer("CodeMetric");

            _locCalculator = new LineOfCodeCalculator();

            _view.ViewportHeightChanged += OnViewSizeChanged;
            _view.ViewportWidthChanged += OnViewSizeChanged;
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
            var ran = new Random();
            
            SnapshotPoint caretPosition = _view.Caret.Position.BufferPosition;
            Document doc = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            var ancestorsAndSelf = doc.GetSyntaxRootAsync().Result.FindToken(caretPosition).Parent.AncestorsAndSelf();
            var root = ancestorsAndSelf.FirstOrDefault(i => i is MethodDeclarationSyntax);

            if(root != null)
            {
                var charBounds = _view.GetTextViewLineContainingBufferPosition(caretPosition)
                                      .GetCharacterBounds(caretPosition);
                
                Canvas.SetTop(_root, charBounds.Top);
                Canvas.SetRight(_root, charBounds.Right + 30);

                //LOC
                _root.LblLineOfCode.Content = _locCalculator.Calculate(root);


                _root.LblCyclomaticComplexity.Content = ran.Next(100, 200);
                _root.LblMaintainabilityIndex.Content = ran.Next(1, 100);
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

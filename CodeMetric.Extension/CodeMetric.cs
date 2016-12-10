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

namespace CodeMetric.Extension
{
    class CodeMetric
    {
        private CodeMetricBarControl _root;
        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;
        
        public CodeMetric(IWpfTextView view)
        {
            _view = view;
            _root = new CodeMetricBarControl();

            _adornmentLayer = view.GetAdornmentLayer("CodeMetric");

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

            /*
Microsoft.VisualStudio.Text.Editor.IWpfTextView textView = GetTextView();
Microsoft.VisualStudio.Text.SnapshotPoint caretPosition = textView.Caret.Position.BufferPosition;
Microsoft.CodeAnalysis.Document document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax invocationExpressionNode = 
document.GetSyntaxRootAsync().Result.FindToken(caretPosition).Parent.AncestorsAndSelf().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax>().FirstOrDefault();
             */

            //SnapshotPoint caretPosition = _view.Caret.Position.BufferPosition;
            //Document doc = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            //InvocationExpressionSyntax invocationExpressionNode = doc.GetSyntaxRootAsync().Result.FindToken(caretPosition).Parent.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().FirstOrDefault();
            
            var codeStr = _view.TextSnapshot.GetText();
            var syntaxTree = CSharpSyntaxTree.ParseText(codeStr);
            
            var root = syntaxTree.GetRoot();
            
            var locCalculator = new LineOfCodeCalculator();
            var loc = locCalculator.Calculate(root);
            _root.LblLineOfCode.Content = loc;

            _root.LblCyclomaticComplexity.Content = ran.Next(100, 200);
            _root.LblMaintainabilityIndex.Content = ran.Next(1, 100);

        }
    }
}

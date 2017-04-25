using System.ComponentModel.Composition;
using CodeMetric.Core.Shared;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace CodeMetric.Extension
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class CodeMetricBarFactory : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("CodeMetric")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        public void TextViewCreated(IWpfTextView textView)
        {
            textView.LayoutChanged += (sender, args) =>
                                      {
                                          CodeMetricPackage.Current.LayoutChangeProvider.OnLayoutChanged(sender, args);
                                      };
            textView.Properties.GetOrCreateSingletonProperty(() => CodeMetricPackage.Current.LayoutChangeProvider);
        }
    }
}

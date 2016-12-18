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
            ILayoutChangeProvider layoutChangeProvider = new LayoutChangeProvider();
            CodeMetricTypeProvider.Add<ILayoutChangeProvider>(typeof(ILayoutChangeProvider), layoutChangeProvider);
            textView.LayoutChanged += (sender, args) =>
                                      {
                                          layoutChangeProvider.OnLayoutChanged(sender, args);
                                      };
            textView.Properties.GetOrCreateSingletonProperty(() => layoutChangeProvider);
        }
    }
}

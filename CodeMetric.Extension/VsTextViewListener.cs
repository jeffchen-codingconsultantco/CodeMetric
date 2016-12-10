using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace CodeMetric.Extension
{
    [Export(typeof(IVsTextViewCreationListener))]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [ContentType("text")]
    internal class VsTextViewListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService = null;
        

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if(textView == null)
                return;

            CodeMetric adornment = textView.Properties.GetProperty<CodeMetric>(typeof(CodeMetric));
            textView.Properties.GetOrCreateSingletonProperty(() => new TypeCharFilter(textViewAdapter, textView, adornment));
        }
    }


    internal sealed class TypeCharFilter : IOleCommandTarget
    {
        private ITextView _textView;
        private readonly CodeMetric _adornmentCodeMetric;
        private readonly IOleCommandTarget _nextCommandHandler;


        public TypeCharFilter(IVsTextView textViewAdapter, ITextView textView, CodeMetric adornment)
        {
            this._textView = textView;
            this._adornmentCodeMetric = adornment;

            textViewAdapter.AddCommandFilter(this, out _nextCommandHandler);

        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            int hr = VSConstants.S_OK;
            _adornmentCodeMetric.UpdateMetric();

            hr = _nextCommandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            return hr;

        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return _nextCommandHandler.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}

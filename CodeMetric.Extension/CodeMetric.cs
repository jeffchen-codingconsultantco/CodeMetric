using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

            Canvas.SetLeft(_root, _view.ViewportRight - 80);
            Canvas.SetTop(_root, _view.ViewportTop + 15);

            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _root, null);
        }
    }
}

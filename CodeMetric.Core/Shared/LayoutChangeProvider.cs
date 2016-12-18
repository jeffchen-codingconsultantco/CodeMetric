using System;
using System.ComponentModel.Composition;

namespace CodeMetric.Core.Shared
{
    [Export(typeof(ILayoutChangeProvider))]
    public class LayoutChangeProvider : ILayoutChangeProvider
    {
        public event EventHandler LayoutChanged;

        public void OnLayoutChanged(object sender, EventArgs args)
        {
            LayoutChanged?.Invoke(sender, args);
        }
    }
}
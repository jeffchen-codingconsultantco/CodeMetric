using System;

namespace CodeMetric.Core.Shared
{
    public interface ILayoutChangeProvider
    {
        event EventHandler LayoutChanged;
        void OnLayoutChanged(object sender, EventArgs args);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetric.Core.Shared
{
    public interface ILayoutChangeProvider
    {
        event EventHandler LayoutChanged;
        void OnLayoutChanged(object sender, EventArgs args);
    }
}

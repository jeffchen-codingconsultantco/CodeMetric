using System.ComponentModel.Composition;
using Microsoft.VisualStudio.CodeSense.Editor;
using Microsoft.VisualStudio.Language.Intellisense;

namespace CodeMetric.v14.VisualStudio.CodeLens
{
    [DataPointViewModelProvider(typeof(CurrentUsersDataPoint))]
    public class CurrentUsersDataPointViewModelProvider : DataPointViewModelProvider<CurrentUsersDataPointViewModel>
    {
        [Import]
        private readonly CurrentUsersDataPointUpdater DataPointUpdater = null;
        protected override CurrentUsersDataPointViewModel GetViewModel(ICodeLensDataPoint dataPoint)
        {
            var dataPointModel = new CurrentUsersDataPointViewModel(DataPointUpdater, dataPoint);
            return dataPointModel;
        }
    }
}

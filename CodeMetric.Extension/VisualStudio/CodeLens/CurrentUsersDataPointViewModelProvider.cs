using Microsoft.VisualStudio.CodeSense.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using System.ComponentModel.Composition;

namespace TeamCoding.VisualStudio.CodeLens
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

using Microsoft.VisualStudio.CodeSense.Roslyn;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CodeMetric.Core;
using CodeMetric.Core.Shared;


namespace TeamCoding.VisualStudio.CodeLens
{
    [Export(typeof(CurrentUsersDataPointUpdater))]
    public class CurrentUsersDataPointUpdater : IDisposable
    {
        // Can't use an import here since this is loaded dynamically it doesn't have access to the main project's MEF exports
        private readonly ILayoutChangeProvider LayoutChangeProvider = CodeMetricTypeProvider.Get<ILayoutChangeProvider>();
        private readonly List<CurrentUsersDataPointViewModel> DataPointModels = new List<CurrentUsersDataPointViewModel>();

        private Dictionary<ICodeElementDescriptor, string> CodeElementDescriptorToDataPointString = new Dictionary<ICodeElementDescriptor, string>();
        private bool disposedValue = false; // To detect redundant calls
        public CurrentUsersDataPointUpdater(): base()
        {
            LayoutChangeProvider.LayoutChanged += LayoutChangeProviderOnLayoutChanged;
        }

        private void LayoutChangeProviderOnLayoutChanged(object sender, EventArgs eventArgs)
        {
            foreach(var dataPointModel in DataPointModels)
            {
                var codeElementDescriptor = ((CurrentUsersDataPoint)dataPointModel.DataPoint).CodeElementDescriptor;
                if(dataPointModel.IsDisposed)
                {
                    CodeElementDescriptorToDataPointString.Remove(codeElementDescriptor);
                }
                else
                {
                    bool shouldRefresh = false;
                    
                    var cmc = new CodeMetricCalculator();
                    var result = cmc.Calculate(codeElementDescriptor.SyntaxNode);
                    var newText = $"LOC: {result.LineOfCode}, CC: {result.CyclomaticComplexity}, MI: {result.MaintainabilityIndex:###}";

                    if(dataPointModel.Descriptor != newText ||
                        !dataPointModel.IsVisible && !string.IsNullOrEmpty(newText))
                    {
                        shouldRefresh = true;
                    }

                    if(shouldRefresh && dataPointModel.RefreshCommand.CanExecute(null))
                    {
                        CodeElementDescriptorToDataPointString[codeElementDescriptor] = newText;
                        dataPointModel.RefreshCommand.Execute(null);
                    }
                }
            }
        }

        public void AddDataPointModel(CurrentUsersDataPointViewModel dataPointModel)
        {
            DataPointModels.Add(dataPointModel);
        }
        public void RemoveDataPointModel(CurrentUsersDataPointViewModel dataPointModel)
        {
            DataPointModels.Remove(dataPointModel);
        }

        public Task<string> GetTextForDataPoint(ICodeElementDescriptor codeElementDescriptor)
        {
            if(CodeElementDescriptorToDataPointString.ContainsKey(codeElementDescriptor))
            {
                return Task.FromResult(CodeElementDescriptorToDataPointString[codeElementDescriptor]);
            }

            var cmc = new CodeMetricCalculator();
            var result = cmc.Calculate(codeElementDescriptor.SyntaxNode);

            var metricMsg = $"LOC: {result.LineOfCode}, CC: {result.CyclomaticComplexity}, MI: {result.MaintainabilityIndex:###}";
            CodeElementDescriptorToDataPointString.Add(codeElementDescriptor, metricMsg);
            return Task.FromResult(metricMsg);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    LayoutChangeProvider.LayoutChanged -= LayoutChangeProviderOnLayoutChanged;
                }
                disposedValue = true;
            }
        }
        public void Dispose() { Dispose(true); }
    }
}

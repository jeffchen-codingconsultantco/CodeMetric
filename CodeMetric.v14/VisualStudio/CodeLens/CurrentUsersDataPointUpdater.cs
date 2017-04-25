using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CodeMetric.Core;
using CodeMetric.Core.Shared;
using Microsoft.VisualStudio.CodeSense.Roslyn;

namespace CodeMetric.v14.VisualStudio.CodeLens
{
    [Export(typeof(CurrentUsersDataPointUpdater))]
    public class CurrentUsersDataPointUpdater : IDisposable
    {
        // Can't use an import here since this is loaded dynamically it doesn't have access to the main project's MEF exports
        private readonly ILayoutChangeProvider _layoutChangeProvider = CodeMetricTypeProvider.Get<ILayoutChangeProvider>();
        private readonly List<CurrentUsersDataPointViewModel> _dataPointModels = new List<CurrentUsersDataPointViewModel>();

        private readonly Dictionary<ICodeElementDescriptor, string> _codeElementDescriptorToDataPointString = new Dictionary<ICodeElementDescriptor, string>();
        private bool disposedValue = false; // To detect redundant calls
        public CurrentUsersDataPointUpdater()
        {
            _layoutChangeProvider.LayoutChanged += LayoutChangeProviderOnLayoutChanged;
        }

        public void AddDataPointModel(CurrentUsersDataPointViewModel dataPointModel)
        {
            _dataPointModels.Add(dataPointModel);
        }

        public void RemoveDataPointModel(CurrentUsersDataPointViewModel dataPointModel)
        {
            _dataPointModels.Remove(dataPointModel);
        }

        public Task<string> GetTextForDataPoint(ICodeElementDescriptor codeElementDescriptor)
        {
            if(_codeElementDescriptorToDataPointString.ContainsKey(codeElementDescriptor))
            {
                return Task.FromResult(_codeElementDescriptorToDataPointString[codeElementDescriptor]);
            }

            var cmc = new CodeMetricCalculator();
            var result = cmc.Calculate(codeElementDescriptor.SyntaxNode);

            var metricMsg = $"LOC: {result.LineOfCode}, CC: {result.CyclomaticComplexity}, MI: {result.MaintainabilityIndex:###}";
            _codeElementDescriptorToDataPointString.Add(codeElementDescriptor, metricMsg);
            return Task.FromResult(metricMsg);
        }

        private void LayoutChangeProviderOnLayoutChanged(object sender, EventArgs eventArgs)
        {
            foreach(var dataPointModel in _dataPointModels)
            {
                var codeElementDescriptor = ((CurrentUsersDataPoint)dataPointModel.DataPoint).CodeElementDescriptor;
                if(dataPointModel.IsDisposed)
                {
                    _codeElementDescriptorToDataPointString.Remove(codeElementDescriptor);
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
                        _codeElementDescriptorToDataPointString[codeElementDescriptor] = newText;
                        dataPointModel.RefreshCommand.Execute(null);
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    _layoutChangeProvider.LayoutChanged -= LayoutChangeProviderOnLayoutChanged;
                }
                disposedValue = true;
            }
        }
        public void Dispose() { Dispose(true); }
    }
}

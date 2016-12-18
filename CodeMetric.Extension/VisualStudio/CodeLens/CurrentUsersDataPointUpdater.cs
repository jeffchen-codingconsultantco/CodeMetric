﻿using Microsoft.VisualStudio.CodeSense.Roslyn;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CodeMetric.Core;
using CodeMetric.Core.Halstead;
using CodeMetric.Core.Shared;


namespace TeamCoding.VisualStudio.CodeLens
{
    [Export(typeof(CurrentUsersDataPointUpdater))]
    public class CurrentUsersDataPointUpdater : IDisposable
    {
        // Can't use an import here since this is loaded dynamically it doesn't have access to the main project's MEF exports
        private readonly ILayoutChangeProvider LayoutChangeProvider = CodeMetricTypeProvider.Get<ILayoutChangeProvider>();
        private readonly List<CurrentUsersDataPointViewModel> DataPointModels = new List<CurrentUsersDataPointViewModel>();
        
        private Dictionary<int[], string> CaretMemberHashCodeToDataPointString = new Dictionary<int[], string>();

        private Dictionary<ICodeElementDescriptor, string> CodeElementDescriptorToDataPointString = new Dictionary<ICodeElementDescriptor, string>();
        private bool disposedValue = false; // To detect redundant calls
        public CurrentUsersDataPointUpdater(): base()
        {
            //RemoteModelChangeManager.RemoteModelReceived += RemoteModelChangeManager_RemoteModelReceived;
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
                    //var newText = GetTextForDataPointInternal(codeElementDescriptor);

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
        private void RemoteModelChangeManager_RemoteModelReceived(object sender, EventArgs e)
        {
            //var oldCaretMemberHashCodeToDataPointString = CaretMemberHashCodeToDataPointString;

            //CaretMemberHashCodeToDataPointString = RemoteModelChangeManager.GetOpenFiles()
            //                                  .Where(of => of.CaretPositionInfo != null)
            //                                  .Select(of => new
            //                                  {
            //                                      CaretMemberHashCodes = of.CaretPositionInfo.SyntaxNodeIds,
            //                                      of.IdeUserIdentity.DisplayName
            //                                  })
            //                                  .GroupBy(of => of.CaretMemberHashCodes,
            //                                           new IntArrayEqualityComparer())
            //                                  .ToDictionary(g => g.Key,
            //                                                g => "Current coders: " + string.Join(", ", g.Select(of => of.DisplayName).Distinct()),
            //                                                new IntArrayEqualityComparer());

            //if(!oldCaretMemberHashCodeToDataPointString.DictionaryEqual(CaretMemberHashCodeToDataPointString))
            //{
            //    foreach(var dataPointModel in DataPointModels)
            //    {
            //        var codeElementDescriptor = ((CurrentUsersDataPoint)dataPointModel.DataPoint).CodeElementDescriptor;
            //        if(dataPointModel.IsDisposed)
            //        {
            //            CodeElementDescriptorToDataPointString.Remove(codeElementDescriptor);
            //        }
            //        else
            //        {
            //            bool shouldRefresh = false;
            //            var newText = GetTextForDataPointInternal(codeElementDescriptor);

            //            if(dataPointModel.Descriptor != newText ||
            //                !dataPointModel.IsVisible && !string.IsNullOrEmpty(newText))
            //            {
            //                shouldRefresh = true;
            //            }

            //            if(shouldRefresh && dataPointModel.RefreshCommand.CanExecute(null))
            //            {
            //                CodeElementDescriptorToDataPointString[codeElementDescriptor] = newText;
            //                dataPointModel.RefreshCommand.Execute(null);
            //            }
            //        }
            //    }
            //    DataPointModels.RemoveAll(dvm => dvm.IsDisposed);
            //}
        }
        private string GetTextForDataPointInternal(ICodeElementDescriptor codeElementDescriptor)
        {
            //foreach(var caret in CaretMemberHashCodeToDataPointString.Keys)
            //{
            //    var node = codeElementDescriptor.SyntaxNode;

            //    // Find the first node that we start the node chain from (any node that is tracked; a class or member declaration etc)
            //    var syntaxNodeChain = node.AncestorsAndSelf().ToArray();
            //    var trackedLeafNodes = syntaxNodeChain.Where(n => n.IsUniquelyIdentifiedNode()).Reverse().ToArray();

            //    var foundMatch = false;
            //    for(int i = 0; i < trackedLeafNodes.Length; i++)
            //    {
            //        var matchedLeafNode = trackedLeafNodes[i];
            //        var caretMatchedHashIndex = Array.LastIndexOf(caret, matchedLeafNode.GetValueBasedHashCode());

            //        if(caretMatchedHashIndex == -1)
            //        {
            //            foundMatch = false;
            //            continue;
            //        }

            //        // Now walk up the tree from the matching one, and up the method hashes ensuring we match all the way up
            //        var nodeancestorhashes = matchedLeafNode.AncestorsAndSelf().Select(a => a.GetValueBasedHashCode());
            //        if(nodeancestorhashes.SequenceEqual(caret.Take(caretMatchedHashIndex + 1).Reverse()))
            //        {
            //            foundMatch = true;
            //        }
            //        else
            //        {
            //            foundMatch = false;
            //        }
            //    }

            //    if(foundMatch)
            //    {
            //        return CaretMemberHashCodeToDataPointString[caret];
            //    }
            //}
            return null;
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

            //foreach(var dataPointModel in DataPointModels)
            //{
            //    if((dataPointModel.Descriptor != metricMsg
            //        || !dataPointModel.IsVisible && !string.IsNullOrEmpty(metricMsg))
            //       && dataPointModel.RefreshCommand.CanExecute(null))
            //    {
            //        CodeElementDescriptorToDataPointString[codeElementDescriptor] = metricMsg;
            //        dataPointModel.RefreshCommand.Execute(null);
            //    }
            //}

            return Task.FromResult(metricMsg);
        }
        protected virtual void Dispose(bool disposing)
        {
            //if(!disposedValue)
            //{
            //    if(disposing)
            //    {
            //        RemoteModelChangeManager.RemoteModelReceived -= RemoteModelChangeManager_RemoteModelReceived;
            //    }
            //    disposedValue = true;
            //}
        }
        public void Dispose() { Dispose(true); }
    }
}

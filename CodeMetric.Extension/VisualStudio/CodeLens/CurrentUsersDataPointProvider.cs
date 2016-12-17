﻿using Microsoft.VisualStudio.Alm.Roslyn.Client.Features.WorkspaceUpdateManager;
using Microsoft.VisualStudio.CodeSense.Roslyn;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace TeamCoding.VisualStudio.CodeLens
{
    [Export(typeof(ICodeLensDataPointProvider)), Name(CodeLensName)]
    public class CurrentUsersDataPointProvider : ICodeLensDataPointProvider
    {
        public const string CodeLensName = "Code Metric";
        [Import]
        private readonly CurrentUsersDataPointUpdater DataPointUpdater = null;
        [Import]
        private readonly IWorkspaceUpdateManager WorkspaceUpdateManager = null;
        public bool CanCreateDataPoint(ICodeLensDescriptor descriptor)
        {
            return descriptor is ICodeElementDescriptor;
        }
        public ICodeLensDataPoint CreateDataPoint(ICodeLensDescriptor codeLensDescriptor)
        {
            return new CurrentUsersDataPoint(DataPointUpdater, WorkspaceUpdateManager, (ICodeElementDescriptor)codeLensDescriptor);
        }
    }
}

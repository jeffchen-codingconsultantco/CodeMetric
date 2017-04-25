//------------------------------------------------------------------------------
// <copyright file="CodeMetricPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Reflection;
using System.IO;

namespace CodeMetric.Extension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(CodeMetricPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class CodeMetricPackage : Package
    {
        /// <summary>
        /// CodeMetricPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "0f5e27c1-805e-47fd-a064-8821b88118e3";

        public static CodeMetricPackage Current { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeMetricPackage"/> class.
        /// </summary>
        public CodeMetricPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
            Current = this;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = new AssemblyName(args.Name);
            if (assembly.Name.Contains("CodeMetric.v"))
            {
                return GetVersionedAssembly();
            }

            return null;
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            //Load versioned CodeMetric.vXX dll
            var versionedAssembly = GetVersionedAssembly();


        }

        private Assembly GetVersionedAssembly()
        {
            return Assembly.LoadFrom(
                Path.Combine(
                    Path.GetDirectoryName(typeof(CodeMetricPackage).Assembly.Location), 
                    $"CodeMetric.v{GetMajorVsVersion()}.dll"));
        }

        private int GetMajorVsVersion()
        {
            var dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
            if (Version.TryParse(dte.Version, out var version))
            {
                return version.Major;
            }
            return 15;
        }

        #endregion
    }
}

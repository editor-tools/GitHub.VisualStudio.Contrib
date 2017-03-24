using System;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using EnvDTE;
using EnvDTE80;
using GitHub.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitHub.VisualStudio.Contrib
{
    public class PackageLifecycle : IDisposable
    {
        public const string InstallMessage = "Please install the GitHub Extension for Visual Studio";
        const string GitHubPackagePkgString = "c3d3dc68-c977-411f-b3e8-03b0dccf7dfc";

        Package package;
        AssemblyResolver assemblyResolver;
        CompositionContainer container;

        public PackageLifecycle(Package package)
        {
            this.package = package;
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            try
            {
                var gitHubPackage = FindPackage(GitHubPackagePkgString);
                if(gitHubPackage == null)
                {
                    var message = InstallMessage;
                    dte.StatusBar.Text = message;
                    return;
                }

                assemblyResolver = new AssemblyResolver(gitHubPackage.GetType());
                Init();
            }
            catch (ReflectionTypeLoadException e)
            {
                Trace.WriteLine(e);
                foreach (var ex in e.LoaderExceptions)
                {
                    Trace.WriteLine(ex);
                }

                dte.StatusBar.Text = e.ToString();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                dte.StatusBar.Text = e.ToString();
            }
        }

        IVsPackage FindPackage(string pkgString)
        {
            var guid = new Guid(pkgString);
            IVsPackage package = null;
            Shell?.LoadPackage(ref guid, out package);
            return package;
        }

        void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyCatalog = new AssemblyCatalog(assembly);
            var sp = (IGitHubServiceProvider)ServiceProvider.GetService(typeof(IGitHubServiceProvider));
            container = new CompositionContainer(assemblyCatalog, sp.ExportProvider);
            container.ComposeExportedValue(assemblyResolver);
            container.ComposeExportedValue(package);
            container.GetExportedValues<CommandBase>();
            container.GetExportedValue<GitCloneCommandLine>();
        }

        public void Dispose()
        {
            container?.Dispose();
            assemblyResolver?.Dispose();
        }

        IVsShell Shell => ServiceProvider.GetService(typeof(SVsShell)) as IVsShell;
        IServiceProvider ServiceProvider => package;
    }
}

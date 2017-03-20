namespace GitHub.VisualStudio.Contrib
{
    using System;
    using Microsoft.VisualStudio.Shell;
    using EnvDTE80;
    using System.IO;
    using System.Reflection;
    using System.Diagnostics;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using GitHub.Services;
    using EnvDTE;

    public class PackageLifecycle : IDisposable
    {
        Package package;
        CompositionContainer container;

        public PackageLifecycle(Package package)
        {
            this.package = package;
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            try
            {
                Init(ServiceProvider);
            }
            catch (FileNotFoundException e) when (e.FileName.StartsWith("GitHub.Exports,"))
            {
                var assemblyName = new AssemblyName(e.FileName);
                var message = $"Please install GitHub for Visual Studio version {assemblyName.Version} or later";
                dte.StatusBar.Text = message;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                dte.StatusBar.Text = e.ToString();
            }
        }

        IServiceProvider ServiceProvider => package;

        void Init(IServiceProvider serviceProvider)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyCatalog = new AssemblyCatalog(assembly);
            var sp = (IGitHubServiceProvider)serviceProvider.GetService(typeof(IGitHubServiceProvider));
            container = new CompositionContainer(assemblyCatalog, sp.ExportProvider);
            container.ComposeExportedValue(package);
            container.GetExportedValues<CommandBase>();
            container.GetExportedValue<GitCloneCommandLine>();
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}

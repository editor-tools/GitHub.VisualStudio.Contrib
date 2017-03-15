namespace GitHub.VisualStudio.Contrib
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Diagnostics;
    using System.ComponentModel.Composition.Hosting;
    using EnvDTE;
    using EnvDTE80;
    using GitHub.Services;

    public class GitHubCommandImpl
    {
        public GitHubCommandImpl(IServiceProvider serviceProvider)
        {
            var dte = (DTE2)serviceProvider.GetService(typeof(DTE));
            try
            {
                ExecuteCommand(serviceProvider);
            }
            catch(FileNotFoundException e) when (e.FileName.StartsWith("GitHub.Exports,"))
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

        static void ExecuteCommand(IServiceProvider serviceProvider)
        {
            var assemblyCatalog = new AssemblyCatalog(typeof(GitHubCommandExport).Assembly);
            var sp = (IGitHubServiceProvider)serviceProvider.GetService(typeof(IGitHubServiceProvider));
            var cc = new CompositionContainer(assemblyCatalog, sp.ExportProvider);
            cc.GetExportedValue<GitHubCommandExport>();
        }
    }
}

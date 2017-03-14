namespace GitHub.VisualStudio.Contrib
{
    using System;
    using System.Diagnostics;
    using System.ComponentModel.Composition.Hosting;
    using EnvDTE;
    using EnvDTE80;
    using GitHub.Services;

    public class GitHubCommandImpl
    {
        public GitHubCommandImpl(IServiceProvider serviceProvider)
        {
            try
            {
                var assemblyCatalog = new AssemblyCatalog(typeof(GitHubCommandExport).Assembly);
                var sp = (IGitHubServiceProvider)serviceProvider.GetService(typeof(IGitHubServiceProvider));
                var cc = new CompositionContainer(assemblyCatalog, sp.ExportProvider);
                cc.GetExportedValue<GitHubCommandExport>();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                var dte = (DTE2)serviceProvider.GetService(typeof(DTE));
                dte.StatusBar.Text = e.ToString();
            }
        }
    }
}

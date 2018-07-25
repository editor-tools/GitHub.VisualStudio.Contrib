using System;
using System.Windows;
using System.ComponentModel.Composition;
using GitHub.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export]
    public class GitHubPaneService
    {
        readonly static Guid GitHubPaneGuid = new Guid("6b0fdc0a-f28e-47a0-8eed-cc296beff6d2");
        readonly IConsoleContext console;
        Lazy<dynamic> gitHubPane;

        [ImportingConstructor]
        public GitHubPaneService(IConsoleContext console, IGitHubServiceProvider gitHubServiceProvider)
        {
            this.console = console;

            gitHubPane = new Lazy<dynamic>(() => FindGitHubPane(gitHubServiceProvider));
        }

        public FrameworkElement View
        {
            get => gitHubPane.Value.View;
            set => gitHubPane.Value.View = value;
        }

        static dynamic FindGitHubPane(IGitHubServiceProvider gitHubServiceProvider)
        {
            var usShell = gitHubServiceProvider.GetService<SVsUIShell, IVsUIShell>();
            if (ErrorHandler.Succeeded(usShell.FindToolWindowEx((uint)(__VSFINDTOOLWIN.FTW_fFindFirst | __VSFINDTOOLWIN.FTW_fForceCreate),
                GitHubPaneGuid, 0, out IVsWindowFrame windowFrame)))
            {
                windowFrame.Show();
                if (ErrorHandler.Succeeded(windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out object docView)))
                {
                    return docView;
                }
            }

            return null;
        }

    }
}

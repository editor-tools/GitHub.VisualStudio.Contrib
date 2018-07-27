using System;
using System.ComponentModel.Composition;
using GitHub.Factories;
using GitHub.Services;
using GitHub.ViewModels;
using GitHub.ViewModels.GitHubPane;
using GitHub.VisualStudio.Contrib.UI.Services;
using GitHub.VisualStudio.Contrib.UI.ViewModels;
using GitHub.VisualStudio.Contrib.Vsix;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace GitHub.VisualStudio.Contrib
{
    [Command(PackageGuids.guidGitHubCommandPackageCmdSetString, PackageIds.GitHubCommandId)]
    internal sealed class GitHubCommand : CommandBase
    {
        readonly Lazy<IGitHubToolWindowManager> toolWindowManager;
        readonly IViewViewModelFactory factory;

        [ImportingConstructor]
        internal GitHubCommand(Package package, IGitHubServiceProvider sp,
            [Import(typeof(LocalViewViewModelFactory))] IViewViewModelFactory factory) : base(package)
        {
            this.factory = factory;

            toolWindowManager = new Lazy<IGitHubToolWindowManager>(() => sp.GetService<IGitHubToolWindowManager>());
        }

        internal override void MenuItemCallback(object sender, EventArgs e)
        {
            ShowOpenFromGitHubAsync().FileAndForget("ShowOpenFromGitHub");
        }

        [STAThread]
        async Task ShowOpenFromGitHubAsync()
        {
            var pane = await toolWindowManager.Value.ShowGitHubPane();
            if (pane.Content is INavigationViewModel navigationViewModel)
            {
                var viewModel = factory.CreateViewModel<IHelloWorldViewModel>();
                navigationViewModel.NavigateTo(viewModel);
            }
        }
    }
}

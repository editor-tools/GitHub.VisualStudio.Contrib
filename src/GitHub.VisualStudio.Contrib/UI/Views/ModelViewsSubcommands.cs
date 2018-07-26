using System;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using GitHub.Services;
using GitHub.Factories;
using GitHub.ViewModels;
using GitHub.ViewModels.GitHubPane;
using GitHub.VisualStudio.Contrib.UI.ViewModels;
using Microsoft.VisualStudio.Threading;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export]
    public partial class ModelViewsSubcommands
    {
        readonly ICompositionService compositionService;
        readonly IViewViewModelFactory factory;
        readonly IShowDialogService showDialog;
        readonly IConsoleContext console;
        readonly IGitHubToolWindowManager toolWindowManager;

        [ImportingConstructor]
        public ModelViewsSubcommands(
            ICompositionService compositionService,
            IViewViewModelFactory factory,
            IShowDialogService showDialog,
            IConsoleContext console,
            IGitHubServiceProvider sp)
        {
            this.compositionService = compositionService;
            this.factory = factory;
            this.showDialog = showDialog;
            this.console = console;

            toolWindowManager = sp.GetService<IGitHubToolWindowManager>();
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldViewDialog")]
        public void HelloWorldViewDialog()
        {
            var viewModel = compositionService.GetExportedValue<IHelloWorldViewModel>();
            compositionService.SatisfyImportsOnce(factory);
            showDialog.Show(viewModel).Forget();
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldViewNavigation")]
        public async Task HelloWorldViewNavigationAsync()
        {
            var pane = await toolWindowManager.ShowGitHubPane();
            await pane.ShowPullRequests();

            if (pane.Content is INavigationViewModel navigationViewModel)
            {
                var viewModel = compositionService.GetExportedValue<IHelloWorldViewModel>();
                compositionService.SatisfyImportsOnce(factory);
                navigationViewModel.NavigateTo(viewModel);
            }
        }
    }
}

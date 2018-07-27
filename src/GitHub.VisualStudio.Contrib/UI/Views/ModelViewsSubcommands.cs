using System;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using GitHub.Services;
using GitHub.Factories;
using GitHub.ViewModels;
using GitHub.ViewModels.GitHubPane;
using GitHub.VisualStudio.Contrib.UI.ViewModels;
using GitHub.VisualStudio.Contrib.UI.Services;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export]
    public partial class ModelViewsSubcommands
    {
        readonly IViewViewModelFactory factory;
        readonly IShowDialogService showDialog;
        readonly Lazy<IGitHubToolWindowManager> toolWindowManager;

        [ImportingConstructor]
        public ModelViewsSubcommands(
            [Import(typeof(LocalViewViewModelFactory))] IViewViewModelFactory factory,
            IShowDialogService showDialog,
            IGitHubServiceProvider sp)
        {
            this.factory = factory;
            this.showDialog = showDialog;

            toolWindowManager = new Lazy<IGitHubToolWindowManager>(() => sp.GetService<IGitHubToolWindowManager>());
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldViewDialog")]
        public async Task HelloWorldViewDialogAsync()
        {
            var viewModel = factory.CreateViewModel<IHelloWorldViewModel>();
            await showDialog.Show(viewModel);
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldViewNavigation")]
        public async Task HelloWorldViewNavigationAsync()
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

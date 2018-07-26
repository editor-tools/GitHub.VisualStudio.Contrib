using System;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using GitHub.Factories;
using GitHub.ViewModels.Dialog;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export]
    public partial class ViewViewModelFactorySubcommands
    {
        readonly IViewViewModelFactory viewViewModelFactory;
        readonly GitHubPaneService gitHubPaneService;

        [ImportingConstructor]
        public ViewViewModelFactorySubcommands(IViewViewModelFactory viewViewModelFactory, GitHubPaneService gitHubPaneService)
        {
            this.viewViewModelFactory = viewViewModelFactory;
            this.gitHubPaneService = gitHubPaneService;
        }

        [STAThread]
        [Export, SubcommandMetadata("Clone")]
        public void Clone()
        {
            var view = viewViewModelFactory.CreateView<IRepositoryCloneViewModel>();
            var viewModel = viewViewModelFactory.CreateViewModel<IRepositoryCloneViewModel>();
            view.DataContext = viewModel;
            gitHubPaneService.View = view;
        }
    }
}

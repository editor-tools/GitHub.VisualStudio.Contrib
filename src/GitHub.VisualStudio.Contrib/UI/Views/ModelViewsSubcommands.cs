using System;
using System.ComponentModel.Composition;
using GitHub.Services;
using GitHub.Factories;
using GitHub.VisualStudio.Contrib.UI.Views;
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
        readonly GitHubPaneService gitHubPaneService;
        readonly IConsoleContext console;

        [ImportingConstructor]
        public ModelViewsSubcommands(
            ICompositionService compositionService,
            IViewViewModelFactory factory,
            IShowDialogService showDialog,
            GitHubPaneService gitHubPaneService,
            IConsoleContext console)
        {
            this.compositionService = compositionService;
            this.factory = factory;
            this.showDialog = showDialog;
            this.gitHubPaneService = gitHubPaneService;
            this.console = console;
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldView")]
        public void HelloWorldView()
        {
            var helloWorldView = GetExportedValue<HelloWorldView>();
            var helloWorldViewModel = GetExportedValue<IHelloWorldViewModel>();
            helloWorldView.DataContext = helloWorldViewModel;
            gitHubPaneService.View = helloWorldView;
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldViewDialog")]
        public void HelloWorldViewDialog()
        {
            var viewModel = GetExportedValue<IHelloWorldViewModel>();
            compositionService.SatisfyImportsOnce(factory);
            showDialog.Show(viewModel).Forget();
        }

        T GetExportedValue<T>()
        {
            var factory = new ExportFactory<T>();
            compositionService.SatisfyImportsOnce(factory);
            return factory.Value;
        }

        class ExportFactory<T>
        {
            [Import]
            public T Value = default(T);
        }
    }
}

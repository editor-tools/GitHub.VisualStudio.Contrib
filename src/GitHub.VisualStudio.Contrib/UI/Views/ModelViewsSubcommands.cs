using System;
using System.ComponentModel.Composition;
using GitHub.VisualStudio.Contrib.UI.Views;
using GitHub.VisualStudio.Contrib.UI.ViewModels;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export]
    public partial class ModelViewsSubcommands
    {
        readonly ICompositionService compositionService;
        readonly GitHubPaneService gitHubPaneService;
        readonly IConsoleContext console;

        [ImportingConstructor]
        public ModelViewsSubcommands(ICompositionService compositionService, GitHubPaneService gitHubPaneService, IConsoleContext console)
        {
            this.compositionService = compositionService;
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

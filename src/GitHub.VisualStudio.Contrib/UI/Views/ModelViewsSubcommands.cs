using System;
using System.ComponentModel.Composition;
using GitHub.VisualStudio.Contrib.UI.Views;
using GitHub.VisualStudio.Contrib.UI.ViewModels;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export]
    public partial class ModelViewsSubcommands
    {
        readonly GitHubPaneService gitHubPaneService;
        readonly IConsoleContext console;

        [ImportingConstructor]
        public ModelViewsSubcommands(GitHubPaneService gitHubPaneService, IConsoleContext console)
        {
            this.gitHubPaneService = gitHubPaneService;
            this.console = console;
        }

        [STAThread]
        [Export, SubcommandMetadata("HelloWorldModelView")]
        public void HelloWorldView()
        {
            var helloWorldView = new HelloWorldView();
            var helloWorldViewModel = new HelloWorldViewModel(console);
            helloWorldView.DataContext = helloWorldViewModel;
            gitHubPaneService.View = helloWorldView;
        }
    }
}

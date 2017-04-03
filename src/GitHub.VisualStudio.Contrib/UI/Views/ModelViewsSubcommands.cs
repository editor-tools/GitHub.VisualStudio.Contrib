using System.Reflection;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using GitHub.UI;
using GitHub.Services;
using GitHub.ViewModels;
using GitHub.VisualStudio.Contrib.UI.Views;
using GitHub.VisualStudio.Contrib.UI.ViewModels;

namespace GitHub.VisualStudio.Contrib.Console
{
    public partial class ModelViewsSubcommands
    {
        IGitHubServiceProvider gitHubServiceProvider;
        IConsoleContext console;

        [ImportingConstructor]
        public ModelViewsSubcommands(IGitHubServiceProvider gitHubServiceProvider, IConsoleContext console)
        {
            this.gitHubServiceProvider = gitHubServiceProvider;
            this.console = console;
        }

        [Export, SubcommandMetadata("HelloWorldModelView")]
        public void HelloWorldView()
        {
            var gitHubToolWindowManager = gitHubServiceProvider.GetService<IGitHubToolWindowManager>();
            var homePane = gitHubToolWindowManager.ShowHomePane();
            var container = GetContainer(homePane);
            var helloWorldView = new HelloWorldView();
            var helloWorldViewModel = new HelloWorldViewModel(console);
            helloWorldView.DataContext = helloWorldViewModel;
            container.Content = helloWorldView;
        }

        static UserControl GetContainer(IViewHost viewHost)
        {
            var viewProp = viewHost.GetType().GetProperty("View", BindingFlags.NonPublic | BindingFlags.Instance);
            var view = (UserControl)viewProp.GetValue(viewHost);
            return (UserControl)view.FindName("container");
        }
    }
}

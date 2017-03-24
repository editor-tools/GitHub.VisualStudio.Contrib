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
        HelloWorldView helloWorldView;
        HelloWorldViewModel helloWorldViewModel;

        [ImportingConstructor]
        public ModelViewsSubcommands(IGitHubServiceProvider gitHubServiceProvider,
            HelloWorldView helloWorldView, HelloWorldViewModel helloWorldViewModel)
        {
            this.gitHubServiceProvider = gitHubServiceProvider;
            this.helloWorldView = helloWorldView;
            this.helloWorldViewModel = helloWorldViewModel;
        }

        [Export, SubcommandMetadata("HelloWorldModelView")]
        public void HelloWorldView()
        {
            var gitHubToolWindowManager = gitHubServiceProvider.GetService<IGitHubToolWindowManager>();
            var homePane = gitHubToolWindowManager.ShowHomePane();
            var container = GetContainer(homePane);
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

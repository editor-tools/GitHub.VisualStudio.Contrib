using System.Reflection;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using GitHub.UI;
using GitHub.Services;
using GitHub.ViewModels;

namespace GitHub.VisualStudio.Contrib.Console
{
    public partial class WindowManagerSubcommands
    {
        IConsoleContext console;
        IGitHubServiceProvider gitHubServiceProvider;

        [ImportingConstructor]
        public WindowManagerSubcommands(IConsoleContext console, IGitHubServiceProvider gitHubServiceProvider)
        {
            this.console = console;
            this.gitHubServiceProvider = gitHubServiceProvider;
        }

        [Export, SubcommandMetadata("Clone")]
        public void Clone()
        {
            var homePane = ToolWindowManagerUtilities.ShowHomePane(gitHubServiceProvider);
            var viewWithData = new ViewWithData(UIControllerFlow.Clone);
            homePane.ShowView(viewWithData);
        }

        [Export, SubcommandMetadata("HelloWorldView")]
        public void HelloWorldView()
        {
            var homePane = ToolWindowManagerUtilities.ShowHomePane(gitHubServiceProvider);
            var container = GetContainer(homePane);
            container.Content = new TextBlock { Text = "Hello, World!" };
        }

        static UserControl GetContainer(IViewHost viewHost)
        {
            var viewProp = viewHost.GetType().GetProperty("View", BindingFlags.NonPublic | BindingFlags.Instance);
            var view = (UserControl)viewProp.GetValue(viewHost);
            var container = (UserControl)view.FindName("container");
            return container;
        }
    }
}

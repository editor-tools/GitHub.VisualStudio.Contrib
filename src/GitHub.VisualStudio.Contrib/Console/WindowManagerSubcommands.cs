using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using GitHub.UI;
using GitHub.Services;

namespace GitHub.VisualStudio.Contrib.Console
{
    public partial class WindowManagerSubcommands
    {
        IConsoleContext consoleContext;
        IGitHubServiceProvider gitHubServiceProvider;

        [ImportingConstructor]
        public WindowManagerSubcommands(IConsoleContext consoleContext, IGitHubServiceProvider gitHubServiceProvider)
        {
            this.consoleContext = consoleContext;
            this.gitHubServiceProvider = gitHubServiceProvider;
        }

        [Export, SubcommandMetadata("Clone")]
        public void Clone()
        {
            var homePane = ToolWindowManagerUtilities.ShowHomePane(gitHubServiceProvider);
            var viewWithData = new ViewWithData(UIControllerFlow.Clone);
            homePane.ShowView(viewWithData);
        }

        [Export, SubcommandMetadata("Publish")]
        public void Publish()
        {
            var homePane = ToolWindowManagerUtilities.ShowHomePane(gitHubServiceProvider);
            var viewWithData = new ViewWithData(UIControllerFlow.Publish);
            homePane.ShowView(viewWithData);
        }
    }
}

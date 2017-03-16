using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using EnvDTE;

namespace GitHub.VisualStudio.Contrib
{
    [CommandID("c194ca32-569e-4458-af37-b10f9a95b420", 0x0101)]
    internal sealed class GitHubConsoleCommand : AllowParamsCommandBase
    {
        CommandWindow commandWindow;

        [ImportingConstructor]
        internal GitHubConsoleCommand(Package package) : base(package)
        {
            commandWindow = GetCommandWindow(package);
        }

        private CommandWindow GetCommandWindow(Package package)
        {
            var sp = (IServiceProvider)package;
            var dte2 = (DTE2)sp.GetService(typeof(DTE));
            return dte2.ToolWindows.CommandWindow;
        }

        internal override void MenuItemCallback(object sender, OleMenuCmdEventArgs e)
        {
            var input = (string)e.InValue;
            if(!string.IsNullOrEmpty(input))
            {
                commandWindow.OutputString(input);
            }
        }
    }
}

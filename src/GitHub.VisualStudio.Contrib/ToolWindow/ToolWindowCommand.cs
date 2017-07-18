namespace GitHub.VisualStudio.Contrib.ToolWindow
{
    using System;
    using System.Windows.Controls;
    using System.ComponentModel.Design;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using GitHub.VisualStudio.Contrib.Vsix;

    [CommandID(PackageGuids.guidGitHubCommandPackageCmdSetString, PackageIds.cmdidToolWindowCommand)]
    internal sealed class ToolWindowCommand : CommandBase
    {
        readonly Package package;
        readonly ToolWindowView toolWindowView;
        readonly ToolWindowViewModel toolWindowViewModel;

        [ImportingConstructor]
        public ToolWindowCommand(Package package, ToolWindowView toolWindowView, ToolWindowViewModel toolWindowViewModel) : base(package)
        {
            this.package = package;
            this.toolWindowView = toolWindowView;
            this.toolWindowViewModel = toolWindowViewModel;

            toolWindowView.DataContext = toolWindowViewModel;
        }

        internal override void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                var asm = package.GetType().Assembly;
                var toolWindowType = asm.GetType("GitHub.VisualStudio.Contrib.Vsix.ToolWindow");

                var toolWindowPane = package.FindToolWindow(toolWindowType, 0, true);
                if (null == toolWindowPane || null == toolWindowPane.Frame)
                {
                    throw new NotSupportedException("Cannot create tool window");
                }

                var windowFrame = (IVsWindowFrame)toolWindowPane.Frame;
                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());

                var grid = (Grid)toolWindowPane.Content;
                grid.Children.Clear();
                grid.Children.Add(toolWindowView);
                toolWindowPane.Caption = "ToolWindow";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
    }
}

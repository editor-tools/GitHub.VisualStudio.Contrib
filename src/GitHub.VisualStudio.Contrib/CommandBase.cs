using System;
using System.ComponentModel.Design;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace GitHub.VisualStudio.Contrib
{
    [InheritedExport]
    internal abstract class CommandBase : IDisposable
    {
        MenuCommand menuCommand;
        OleMenuCommandService commandService;

        internal CommandBase(Package package, CommandID commandID)
        {
            menuCommand = new MenuCommand(MenuItemCallback, commandID);

            var serviceProvider = (IServiceProvider)package;
            commandService = (OleMenuCommandService)serviceProvider.GetService(typeof(IMenuCommandService));
            commandService.AddCommand(menuCommand);
        }

        public void Dispose()
        {
            commandService.RemoveCommand(menuCommand);
        }

        internal abstract void MenuItemCallback(object sender, EventArgs e);
    }
}

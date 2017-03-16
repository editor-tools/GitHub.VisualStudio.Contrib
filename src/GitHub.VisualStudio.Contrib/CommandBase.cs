using System;
using System.ComponentModel.Design;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace GitHub.VisualStudio.Contrib
{
    [InheritedExport]
    internal abstract class CommandBase : IDisposable
    {
        OleMenuCommandService commandService;

        internal CommandBase(Package package)
        {
            var commandID = CommandIDAttribute.GetCommandID(GetType());
            Command = new OleMenuCommand(MenuItemCallback, commandID);
            var serviceProvider = (IServiceProvider)package;
            commandService = (OleMenuCommandService)serviceProvider.GetService(typeof(IMenuCommandService));
            commandService.AddCommand(Command);
        }

        internal OleMenuCommand Command { get; }

        public void Dispose()
        {
            commandService.RemoveCommand(Command);
        }

        internal abstract void MenuItemCallback(object sender, EventArgs e);
    }
}

//------------------------------------------------------------------------------
// <copyright file="GitHubCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;
using System.ComponentModel.Composition.Hosting;
using System.Runtime.InteropServices;

namespace GitHub.VisualStudio.Contrib
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GitHubCommand : IDisposable
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("c194ca32-569e-4458-af37-b10f9a95b420");

        Package package;
        OleMenuCommandService commandService;
        MenuCommand menuItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        internal GitHubCommand(Package package)
        {
            this.package = package;
            commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));
            var menuCommandID = new CommandID(CommandSet, CommandId);
            menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public void Dispose()
        {
            commandService.RemoveCommand(menuItem);
        }

        private IServiceProvider ServiceProvider => package;

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            new GitHubCommandImpl(ServiceProvider);
        }
    }
}

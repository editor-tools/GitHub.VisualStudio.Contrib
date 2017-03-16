using System;
using System.ComponentModel.Design;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using GitHub.Services;

namespace GitHub.VisualStudio.Contrib
{
    [Export]
    internal sealed class GitHubCommand : IDisposable
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("c194ca32-569e-4458-af37-b10f9a95b420");

        Package package;
        IStatusBarNotificationService notificationService;
        IVSGitServices vsGitServices;
        OleMenuCommandService commandService;
        MenuCommand menuItem;

        [ImportingConstructor]
        internal GitHubCommand(Package package, IStatusBarNotificationService notificationService, IVSGitServices vsGitServices)
        {
            this.package = package;
            this.notificationService = notificationService;
            this.vsGitServices = vsGitServices;

            commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));
            var menuCommandID = new CommandID(CommandSet, CommandId);
            menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        IServiceProvider ServiceProvider => package;

        public void Dispose()
        {
            commandService.RemoveCommand(menuItem);
        }

        void MenuItemCallback(object sender, EventArgs e)
        {
            var repoPath = vsGitServices.GetActiveRepoPath();
            notificationService.ShowMessage("Hello, World!! " + repoPath);
        }
    }
}

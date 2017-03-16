using System;
using System.ComponentModel.Design;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using GitHub.Services;

namespace GitHub.VisualStudio.Contrib
{
    internal sealed class GitHubCommand : CommandBase
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("c194ca32-569e-4458-af37-b10f9a95b420");

        IStatusBarNotificationService notificationService;
        IVSGitServices vsGitServices;

        [ImportingConstructor]
        internal GitHubCommand(Package package, IStatusBarNotificationService notificationService, IVSGitServices vsGitServices)
            : base(package, new CommandID(CommandSet, CommandId))
        {
            this.notificationService = notificationService;
            this.vsGitServices = vsGitServices;
        }

        internal override void MenuItemCallback(object sender, EventArgs e)
        {
            var repoPath = vsGitServices.GetActiveRepoPath();
            notificationService.ShowMessage("Hello, World! " + repoPath);
        }
    }
}

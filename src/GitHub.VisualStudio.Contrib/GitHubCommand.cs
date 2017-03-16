using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using GitHub.Services;

namespace GitHub.VisualStudio.Contrib
{
    [CommandID("c194ca32-569e-4458-af37-b10f9a95b420", 0x0100)]
    internal sealed class GitHubCommand : CommandBase
    {
        IStatusBarNotificationService notificationService;
        IVSGitServices vsGitServices;

        [ImportingConstructor]
        internal GitHubCommand(Package package, IStatusBarNotificationService notificationService,
            IVSGitServices vsGitServices) : base(package)
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

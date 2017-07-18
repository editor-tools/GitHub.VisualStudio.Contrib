using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using GitHub.Services;
using GitHub.VisualStudio.Contrib.Vsix;

namespace GitHub.VisualStudio.Contrib
{
    [CommandID(PackageGuids.guidGitHubCommandPackageCmdSetString, PackageIds.GitHubCommandId)]
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

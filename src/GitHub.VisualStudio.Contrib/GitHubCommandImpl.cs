using System;
using GitHub.Services;
using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib
{
    public class GitHubCommandImpl
    {
        [ImportingConstructor]
        public GitHubCommandImpl(IGitHubServiceProvider sp)
        {
            var vsGitServices = sp.TryGetService<IVSGitServices>();
            var repo = vsGitServices?.GetActiveRepoPath();

            var statusBarNotificationService = sp.TryGetService<IStatusBarNotificationService>();
            statusBarNotificationService?.ShowMessage(repo);
        }
    }
}

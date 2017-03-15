namespace GitHub.VisualStudio.Contrib
{
    using System.ComponentModel.Composition;
    using GitHub.Services;

    [Export]
    public class GitHubCommandExport
    {
        [ImportingConstructor]
        public GitHubCommandExport(IStatusBarNotificationService notificationService, IVSGitServices vsGitServices)
        {
            var repoPath = vsGitServices.GetActiveRepoPath();
            notificationService.ShowMessage("Hello, World! " + repoPath);
        }
    }
}

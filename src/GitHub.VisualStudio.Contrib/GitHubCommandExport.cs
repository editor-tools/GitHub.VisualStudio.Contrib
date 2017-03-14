namespace GitHub.VisualStudio.Contrib
{
    using System.ComponentModel.Composition;
    using GitHub.Services;

    [Export]
    public class GitHubCommandExport
    {
        [ImportingConstructor]
        public GitHubCommandExport(IGitHubServiceProvider sp)
        {
            var vsGitServices = sp.GetService<IVSGitServices>();
            var repoPath = vsGitServices.GetActiveRepoPath();

            var notificationService = sp.GetService<IStatusBarNotificationService>();
            notificationService.ShowMessage("Hello, World! ActiveRepoPath=" + repoPath);
        }
    }
}

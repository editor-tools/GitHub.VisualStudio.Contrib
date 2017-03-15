namespace GitHub.VisualStudio.Contrib
{
    using System;
    using Microsoft.VisualStudio.Shell;

    public class PackageLifecycle : IDisposable
    {
        GitHubCommand gitHubCommand;

        public PackageLifecycle(Package package)
        {
            gitHubCommand = new GitHubCommand(package);
        }

        public void Dispose()
        {
            gitHubCommand.Dispose();
        }
    }
}

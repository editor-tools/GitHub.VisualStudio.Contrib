using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using GitHub.Services;

namespace GitHub.VisualStudio.Contrib.Console
{
    public class VSGitServicesSubcommands
    {
        IConsoleContext consoleContext;
        IVSGitServices vsGitServices;

        [ImportingConstructor]
        public VSGitServicesSubcommands(IConsoleContext consoleContext, IVSGitServices vsGitServices)
        {
            this.consoleContext = consoleContext;
            this.vsGitServices = vsGitServices;
        }

        [Export, SubcommandMetadata("ActiveRepoPath")]
        public void ActiveRepoPath()
        {
            var path = vsGitServices.GetActiveRepoPath();
            consoleContext.WriteLine(path);
        }

        [Export, SubcommandMetadata("KnownRepositories")]
        public void KnownRepositories()
        {
            var repos = vsGitServices.GetKnownRepositories();
            foreach (var repo in repos)
            {
                consoleContext.WriteLine($"{repo.CloneUrl} ({repo.LocalPath})");
            }
        }

        [Export, SubcommandMetadata("GetLocalClonePathFromGitProvider")]
        public void GetLocalClonePathFromGitProvider()
        {
            consoleContext.WriteLine(vsGitServices.GetLocalClonePathFromGitProvider());
        }
    }
}

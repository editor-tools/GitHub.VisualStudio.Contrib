using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell.Interop;
using GitHub.Services;
using GitHub.Primitives;
using Rothko;

namespace GitHub.VisualStudio.Contrib
{
    [Export]
    public class GitCloneCommandLine
    {
        IVsAppCommandLine vsAppCommandLine;
        IVSGitServices vsGitServices;
        IVSServices vsServices;
        IOperatingSystem operatingSystem;

        [ImportingConstructor]
        public GitCloneCommandLine(IVsAppCommandLine vsAppCommandLine, IVSGitServices vsGitServices,
            ITeamExplorerServices teamExplorerServices, IVSServices vsServices, IOperatingSystem operatingSystem)
        {
            this.vsAppCommandLine = vsAppCommandLine;
            this.vsGitServices = vsGitServices;
            this.vsServices = vsServices;
            this.operatingSystem = operatingSystem;

            var cloneUrl = FindCloneUrl();
            if(cloneUrl == null)
            {
                return;
            }

            TryOpenRepository(cloneUrl);
        }

        bool TryOpenRepository(string cloneUrl)
        {
            return TryOpenKnownRepository(cloneUrl) || TryOpenLocalClonePath(cloneUrl);
        }

        bool TryOpenKnownRepository(string cloneUrl)
        {
            var repos = vsGitServices.GetKnownRepositories();
            foreach (var repo in repos)
            {
                if (cloneUrl == repo.CloneUrl)
                {
                    if(vsServices.TryOpenRepository(repo.LocalPath))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool TryOpenLocalClonePath(string cloneUrl)
        {
            var cloneUri = FindGitHubCloneUri(cloneUrl);
            if(cloneUri == null)
            {
                return false;
            }

            var clonePath = vsGitServices.GetLocalClonePathFromGitProvider();
            if(clonePath == null)
            {
                return false;
            }

            var repoPath = Path.Combine(clonePath, cloneUri.Owner, cloneUri.RepositoryName);
            var repoDir = operatingSystem.Directory.GetDirectory(repoPath);
            if (!repoDir.Exists)
            {
                return false;
            }

            return vsServices.TryOpenRepository(repoPath);
        }

        static UriString FindGitHubCloneUri(string cloneUrl)
        {
            try
            {
                var uriString = new UriString(cloneUrl);
                if (uriString.Host != "github.com")
                {
                    return null;
                }

                if(string.IsNullOrEmpty(uriString.Owner) || string.IsNullOrEmpty(uriString.RepositoryName))
                {
                    return null;
                }

                return uriString;
            }
            catch(Exception e)
            {
                Trace.WriteLine(e);
                return null;
            }
        }

        string FindCloneUrl()
        {
            vsAppCommandLine.GetOption("GitClone", out int isPresent, out string optionValue);
            if (isPresent == 0)
            {
                return null;
            }

            return optionValue;
        }
    }

    class VsAppCommandLineFactory
    {
        IVsAppCommandLine vsAppCommandLine;

        [ImportingConstructor]
        internal VsAppCommandLineFactory(IGitHubServiceProvider sp)
        {
            vsAppCommandLine = sp.GetService<IVsAppCommandLine>();
        }

        [Export]
        internal IVsAppCommandLine VsAppCommandLine => vsAppCommandLine;
    }

}
using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using GitHub.Services;
using GitHub.Primitives;

namespace GitHub.VisualStudio.Contrib
{
    [Export]
    public class GitCloneCommandLine
    {
        Package package;
        IVSGitServices vsGitServices;
        IVSServices vsServices;

        [ImportingConstructor]
        internal GitCloneCommandLine(Package package, IVSGitServices vsGitServices,
            ITeamExplorerServices teamExplorerServices, IVSServices vsServices)
        {
            this.package = package;
            this.vsGitServices = vsGitServices;
            this.vsServices = vsServices;

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
                    if (Directory.Exists(repo.LocalPath))
                    {
                        if(vsServices.TryOpenRepository(repo.LocalPath))
                        {
                            return true;
                        }
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
            var repoPath = Path.Combine(clonePath, cloneUri.Owner, cloneUri.RepositoryName);
            if (!Directory.Exists(repoPath))
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
            var cmdline = (IVsAppCommandLine)ServiceProvider.GetService(typeof(SVsAppCommandLine));
            cmdline.GetOption("GitClone", out int isPresent, out string optionValue);
            if (isPresent == 0)
            {
                return null;
            }

            return optionValue;
        }

        IServiceProvider ServiceProvider => package;
    }
}
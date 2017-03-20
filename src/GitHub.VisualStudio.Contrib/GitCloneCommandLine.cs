using System;
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
        const string GIT_CLONE_PATH = "GitClone_Path";
        Package package;

        [ImportingConstructor]
        internal GitCloneCommandLine(Package package, IVSGitServices vsGitServices)
        {
            this.package = package;
            string url = FindGitCloneUrl();
            if(url == null)
            {
                return;
            }

            var path = DefaultRepositoryPath;
            if(path == null)
            {
                return;
            }

            if(!path.EndsWith($"%{GIT_CLONE_PATH}%"))
            {
                path += $"%{GIT_CLONE_PATH}%";
                DefaultRepositoryPath = path;
            }

            var uriString = new UriString(url);
            var owner = uriString.Owner;
            if(string.IsNullOrEmpty(owner))
            {
                return;
            }

            var gitClonePath = '\\' + owner;
            Environment.SetEnvironmentVariable(GIT_CLONE_PATH, gitClonePath);
        }

        string FindGitCloneUrl()
        {
            var cmdline = (IVsAppCommandLine)ServiceProvider.GetService(typeof(SVsAppCommandLine));
            int isPresent;
            string optionValue;
            cmdline.GetOption("GitClone", out isPresent, out optionValue);
            if(isPresent == 0)
            {
                return null;
            }

            return optionValue;
        }

        IServiceProvider ServiceProvider => package;

        string DefaultRepositoryPath
        {
            set
            {
                using (var key = package.UserRegistryRoot.OpenSubKey(@"TeamFoundation\GitSourceControl\General", true))
                {
                    key.SetValue("DefaultRepositoryPath", value);
                }
            }

            get
            {
                using (var key = package.UserRegistryRoot.OpenSubKey(@"TeamFoundation\GitSourceControl\General", false))
                {
                    return (string)key?.GetValue("DefaultRepositoryPath", null);
                }
            }
        }
    }
}
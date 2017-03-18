using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using GitHub.Services;
using GitHub.ViewModels;

namespace GitHub.VisualStudio.Contrib.Console
{
    class ToolWindowManagerUtilities
    {
        internal static IViewHost ShowHomePane(IGitHubServiceProvider gitHubServiceProvider)
        {
            using (new AssemblyResolver(gitHubServiceProvider.GetType()))
            {
                return ResolvingShowHomePane(gitHubServiceProvider);
            }
        }

        static IViewHost ResolvingShowHomePane(IGitHubServiceProvider gitHubServiceProvider)
        {
            var windowManager = gitHubServiceProvider.GetService<IGitHubToolWindowManager>();
            return windowManager.ShowHomePane();
        }

        class AssemblyResolver : IDisposable
        {
            string dir;

            internal AssemblyResolver(Type type)
            {
                dir = Path.GetDirectoryName(type.Assembly.Location);
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            }

            Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                var name = new AssemblyName(args.Name).Name;
                var file = Path.Combine(dir, name + ".dll");
                if (File.Exists(file))
                {
                    Trace.WriteLine("Resolving: " + file);
                    return Assembly.LoadFrom(file);
                }

                return null;
            }

            public void Dispose()
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }
    }
}

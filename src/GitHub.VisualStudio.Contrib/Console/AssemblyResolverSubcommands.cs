using System;
using System.IO;
using System.Reflection;
using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib.Console
{
    class AssemblyResolverSubcommands
    {
        IConsoleContext console;
        AssemblyResolver assemblyResolver;

        [ImportingConstructor]
        public AssemblyResolverSubcommands(IConsoleContext console, AssemblyResolver assemblyResolver)
        {
            this.console = console;
            this.assemblyResolver = assemblyResolver;
        }

        [Export, SubcommandMetadata("ResolvedAssemblies")]
        public void ResolvedAssemblies()
        {
            foreach (var ra in assemblyResolver.ResolvedAssemblies)
            {
                console.WriteLine(ra.Value.Location);
                console.WriteLine($"  Resolve: {ra.Key.Name}");
                console.WriteLine($"  RequestingAssembly: {ra.Key.RequestingAssembly}");
            }
        }

        [Export, SubcommandMetadata("LoadedAssemblies")]
        public void LoadedAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (IsGitHubAssembly(assembly))
                    {
                        console.WriteLine(GetName(assembly.FullName));
                        console.WriteLine($"  FullName: {assembly.FullName}");
                        console.WriteLine($"  CodeBase: {assembly.CodeBase}");
                        console.WriteLine($"  Location: {assembly.Location}");
                    }
                }
                catch (Exception e)
                {
                    console.WriteLine(e.ToString());
                }
            }
        }

        static string GetName(string fullName)
        {
            var split = fullName?.Split(',');
            if(split == null || split.Length < 1)
            {
                return "<Unknown>";
            }

            return split[0];
        }

        bool IsGitHubAssembly(Assembly assembly)
        {
            if (assembly.FullName.StartsWith("GitHub.VisualStudio.Contrib"))
            {
                return false;
            }

            if (assembly.FullName.StartsWith("GitHub"))
            {
                return true;
            }

            string codeBaseDirectory = FindCodeBaseDirectory(assembly);
            if (assemblyResolver.BaseDirectory.Equals(codeBaseDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        static string FindCodeBaseDirectory(Assembly assembly)
        {
            try
            {
                var localPath = new Uri(assembly.CodeBase).LocalPath;
                return Path.GetDirectoryName(localPath);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}

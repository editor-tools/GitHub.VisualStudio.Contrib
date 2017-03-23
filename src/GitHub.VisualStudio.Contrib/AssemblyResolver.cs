using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel.Composition;
using System.Collections.Generic;

namespace GitHub.VisualStudio.Contrib
{
    [Export]
    class AssemblyResolver : IDisposable
    {
        internal AssemblyResolver(Type type) : this(Path.GetDirectoryName(type.Assembly.Location))
        {
        }

        internal AssemblyResolver(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
            ResolvedAssemblies = new Dictionary<ResolveEventArgs, Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        internal string BaseDirectory { get; }
        internal IDictionary<ResolveEventArgs, Assembly> ResolvedAssemblies { get; }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var assemblyName = new AssemblyName(args.Name);
                var name = assemblyName.Name;
                var file = Path.Combine(BaseDirectory, name + ".dll");
                if (!File.Exists(file))
                {
                    return null;
                }

                var targetAssemblyName = AssemblyName.GetAssemblyName(file);
                if (args.Name != targetAssemblyName.FullName)
                {
                    if (!name.StartsWith("GitHub."))
                    {
                        Trace.WriteLine($"Not redirecting '{args.Name}' to '{targetAssemblyName.FullName}' (doesn't start with 'GitHub.*').");
                        return null;
                    }

                    Trace.WriteLine($"Redirecting '{args.Name}' to '{targetAssemblyName.FullName}'.");
                }

                var assembly = Assembly.LoadFrom(file);
                Trace.WriteLine($"Resolving '{assembly.FullName}' at '{file}'.");

                ResolvedAssemblies[args] = assembly;
                return assembly;
            }
            catch(Exception e)
            {
                Trace.WriteLine($"Error resolving '{args.Name}': {e}");
                return null;
            }
        }
    }
}

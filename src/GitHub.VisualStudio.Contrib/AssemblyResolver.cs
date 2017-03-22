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
        internal AssemblyResolver(Type type, params string[] preloadFileNames)
        {
            BaseDirectory = Path.GetDirectoryName(type.Assembly.Location);
            ResolvedAssemblies = new Dictionary<ResolveEventArgs, Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            foreach(var fileName in preloadFileNames)
            {
                var file = Path.Combine(BaseDirectory, fileName);
                var assembly = Assembly.LoadFrom(file);
                Trace.WriteLine($"Loaded '{assembly}' from '{file}'.");
            }
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        internal string BaseDirectory { get; }
        internal IDictionary<ResolveEventArgs, Assembly> ResolvedAssemblies { get; }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // NOTE: This is currently resolving too much!
            var name = new AssemblyName(args.Name).Name;
            var file = Path.Combine(BaseDirectory, name + ".dll");
            if (File.Exists(file))
            {
                Trace.WriteLine($"Resolving '{args.Name}' to '{file}'.");
                var assembly = Assembly.LoadFrom(file);
                ResolvedAssemblies[args] = assembly;
                return assembly;
            }

            return null;
        }
    }
}

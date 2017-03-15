namespace GitHub.VisualStudio.Contrib.Vsix
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using GhostAssemblies;

    public class GhostObjectLifecycle
    {
        GhostAssemblyLoader ghostAssemblyLoader;
        string typeName;
        Assembly assembly;
        IDisposable instance;
        object[] args;

        public GhostObjectLifecycle(string ghostPaths, string assemblyName, string typeName, params object[] args)
        {
            ghostAssemblyLoader = new GhostAssemblyLoader(ghostPaths, assemblyName);
            this.typeName = typeName;
            this.args = args;
        }


        public object GetInstance()
        {
            if (instance != null && ghostAssemblyLoader.ResolveAssembly() == assembly)
            {
                return instance;
            }

            return CreateInstance();
        }

        object CreateInstance()
        {
            DisposeInstance();

            assembly = ghostAssemblyLoader.ResolveAssembly();
            var type = assembly.GetType(typeName);
            instance = (IDisposable)Activator.CreateInstance(type, args);
            return instance;
        }

        void DisposeInstance()
        {
            try
            {
                instance?.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            finally
            {
                instance = null;
            }
        }
    }
}

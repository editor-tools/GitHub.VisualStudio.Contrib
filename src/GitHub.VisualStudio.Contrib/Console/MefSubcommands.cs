using System;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitHub.VisualStudio.Contrib.Console
{
    public class MefSubcommands
    {
        IServiceProvider serviceProvider;
        IConsoleContext consoleContext;

        [ImportingConstructor]
        public MefSubcommands([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider, IConsoleContext consoleContext)
        {
            this.serviceProvider = serviceProvider;
            this.consoleContext = consoleContext;
        }

        [Export, SubcommandMetadata("mef-assemblies")]
        public void MefAssemblies()
        {
            foreach (var asm in GetComponentAssemblies())
            {
                consoleContext.WriteLine(asm);
            }
        }

        [Export, SubcommandMetadata("mef-dump")]
        public async void MefDump()
        {
            foreach (var assemblyFile in GetComponentAssemblies())
            {
                consoleContext.WriteLine(assemblyFile);

                try
                {
                    await Task.Run(() =>
                    {
                        DumpAssembly(assemblyFile);
                    });
                }
                catch (Exception e)
                {
                    consoleContext.WriteLine(e.Message);
                }
            }
        }

        void DumpAssembly(string assemblyFile)
        {
            var catalog = new AssemblyCatalog(assemblyFile);
            foreach (var part in catalog)
            {
                DumpPart(part);
            }
        }

        void DumpPart(ComposablePartDefinition part)
        {
            consoleContext.WriteLine("   " + part);
            foreach (var exp in part.ExportDefinitions)
            {
                consoleContext.WriteLine("     + " + exp);
            }

            foreach (var imp in part.ImportDefinitions)
            {
                consoleContext.WriteLine("     - " + imp.ContractName);
            }
        }

        string[] GetComponentAssemblies()
        {
            var componentModelHost = (IVsComponentModelHost)serviceProvider.GetService(typeof(SVsComponentModelHost));
            var assemblyFiles = new string[0];
            uint num;
            componentModelHost.GetComponentAssemblies((uint)assemblyFiles.Length, assemblyFiles, out num);
            assemblyFiles = new string[num];
            componentModelHost.GetComponentAssemblies((uint)assemblyFiles.Length, assemblyFiles, out num);
            return assemblyFiles;
        }

    }
}

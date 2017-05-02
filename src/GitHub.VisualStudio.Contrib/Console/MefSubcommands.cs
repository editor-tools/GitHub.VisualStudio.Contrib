using System;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.ComponentModel.Composition.Diagnostics;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.Composition.Reflection;
using System.Threading.Tasks;

namespace GitHub.VisualStudio.Contrib.Console
{
    public class MefSubcommands
    {
        IComponentModel componentModel;
        IVsComponentModelHost componentModelHost;
        IConsoleContext consoleContext;

        [ImportingConstructor]
        public MefSubcommands(IConsoleContext consoleContext, [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.consoleContext = consoleContext;
            componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            componentModelHost = (IVsComponentModelHost)serviceProvider.GetService(typeof(SVsComponentModelHost));
        }

        [Export, SubcommandMetadata("mef-errors")]
        public void MefErrors(string[] args)
        {
            var errFile = Path.Combine(ComponentModelCacheDirectory, "Microsoft.VisualStudio.Default.err");
            if(!File.Exists(errFile))
            {
                consoleContext.WriteLine($"Couldn't find MEF error file at '{errFile}'.");
                return;
            }

            consoleContext.WriteLine(errFile);
            var text = File.ReadAllText(errFile);
            consoleContext.WriteLine(text);
        }

        [Export, SubcommandMetadata("mef-FindSatisfyingExports")]
        public async void FindSatisfyingExports(string[] args)
        {
            if (args.Length != 1)
            {
                consoleContext.WriteLine("Search for string in assembly");
                return;
            }

            var search = args[0];

            var runtimeComposition = await LoadDefaultRuntimeCompositionAsync();
            var assemblyLocations = new HashSet<string>();
            foreach (var part in runtimeComposition.Parts)
            {
                if (FindSatisfyingExports(part, search))
                {
                    FindSatisfyingExports(part, search, true);
                    assemblyLocations.Add(part.TypeRef.Resolve().Assembly.Location);
                }
            }

            foreach(var assemblyLocation in assemblyLocations)
            {
                consoleContext.WriteLine(assemblyLocation);
            }
        }

        async Task<RuntimeComposition> LoadDefaultRuntimeCompositionAsync()
        {
            string cacheDir = ComponentModelCacheDirectory;
            var file = Path.Combine(cacheDir, "Microsoft.VisualStudio.Default.cache");

            RuntimeComposition runtimeComposition;
            using (FileStream stream = File.Open(file, FileMode.Open))
            {
                runtimeComposition = await new CachedComposition().LoadRuntimeCompositionAsync(stream, Resolver.DefaultInstance);
            }

            return runtimeComposition;
        }

        bool FindSatisfyingExports(RuntimeComposition.RuntimePart runtimePart, string search, bool output = false)
        {
            bool found = false;

            if (output)
            {
                var partType = runtimePart.TypeRef.Resolve();
                consoleContext.WriteLine($"{partType.FullName}");
            }

            foreach (var importingMember in runtimePart.ImportingMembers)
            {
                foreach (var export in importingMember.SatisfyingExports)
                {
                    if (export.DeclaringTypeRef.AssemblyName.Name.Contains(search))
                    {
                        found = true;
                        if (output)
                        {
                            consoleContext.WriteLine($"   .{importingMember.ImportingMember.Name} = {ToMemberPath(export)}");
                        }
                        else
                        {
                            return found;
                        }
                    }
                }
            }

            foreach (var importingConstructorArgument in runtimePart.ImportingConstructorArguments)
            {
                foreach (var export in importingConstructorArgument.SatisfyingExports)
                {
                    if (export.DeclaringTypeRef.AssemblyName.Name.Contains(search))
                    {
                        found = true;
                        if (output)
                        {
                            consoleContext.WriteLine($"   ({importingConstructorArgument.ImportingParameter.Name} = {ToMemberPath(export)})");
                        }
                        else
                        {
                            return found;
                        }
                    }
                }
            }

            return found;
        }

        static string ToMemberPath(RuntimeComposition.RuntimeExport export)
        {
            var declaringType = export.DeclaringTypeRef.Resolve();
            var memberInfo = export.MemberRef.Resolve();
            var path = declaringType.FullName;
            if (memberInfo != null)
            {
                path += "." + memberInfo.Name;
            }

            return path;
        }

        [Export, SubcommandMetadata("mef-assemblies")]
        public void MefAssemblies(string[] args)
        {
            var asms = GetComponentAssemblies();
            if(args.Length == 1)
            {
                var filter = args[0];
                asms = asms.Where(a => a.Contains(filter)).ToArray();
            }

            foreach (var asm in asms)
            {
                consoleContext.WriteLine(asm);
            }
        }

        [Export, SubcommandMetadata("mef-dump")]
        public async void MefDump(string[] args)
        {
            var asms = GetComponentAssemblies();
            if (args.Length == 1)
            {
                var filter = args[0];
                asms = asms.Where(a => a.Contains(filter)).ToArray();
            }

            var count = asms.Count();
            foreach(var asm in asms)
            {
                try
                {
                    consoleContext.WriteLine(asm);
                    if (count > 1)
                    {
                        continue;
                    }

                    await Task.Run(() =>
                    {
                        DumpAssembly(asm);
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
            var ci = new CompositionInfo(catalog, componentModel.DefaultExportProvider);
            var writer = new StringWriter();
            CompositionInfoTextFormatter.Write(ci, writer);
            consoleContext.WriteLine(writer.ToString());
        }

        string ComponentModelCacheDirectory
        {
            get
            {
                string folderPath;
                componentModelHost.GetCatalogCacheFolder(out folderPath);
                return folderPath;
            }
        }

        IEnumerable <string> GetComponentAssemblies()
        {
            var assemblyFiles = new string[0];
            uint num;
            componentModelHost.GetComponentAssemblies((uint)assemblyFiles.Length, assemblyFiles, out num);
            assemblyFiles = new string[num];
            componentModelHost.GetComponentAssemblies((uint)assemblyFiles.Length, assemblyFiles, out num);
            return assemblyFiles;
        }

    }
}

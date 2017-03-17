using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using EnvDTE;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Export(typeof(IConsoleContext))]
    public class ConsoleContext : IConsoleContext
    {
        CommandWindow commandWindow;

        [ImportingConstructor]
        internal ConsoleContext(Package package)
        {
            commandWindow = GetCommandWindow(package);
        }

        static CommandWindow GetCommandWindow(Package package)
        {
            var sp = (IServiceProvider)package;
            var dte2 = (DTE2)sp.GetService(typeof(DTE));
            return dte2.ToolWindows.CommandWindow;
        }

        public void Write(string text)
        {
            commandWindow.OutputString(text);
        }

        public void WriteLine(string text)
        {
            commandWindow.OutputString(text + Environment.NewLine);
        }

        public void Activate()
        {
            commandWindow.Parent.Activate();
        }
    }
}

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;

namespace GitHub.VisualStudio.Contrib.Console
{
    [CommandID("c194ca32-569e-4458-af37-b10f9a95b420", 0x0101)]
    internal sealed class ConsoleCommand : AllowParamsCommandBase
    {
        IConsoleContext consoleContext;
        IEnumerable<Lazy<Action, ISubcommandMetadata>> commands;

        [ImportingConstructor]
        internal ConsoleCommand(Package package, IConsoleContext consoleContext,
            [ImportMany] IEnumerable<Lazy<Action, ISubcommandMetadata>> commands) : base(package)
        {
            this.consoleContext = consoleContext;
            this.commands = commands;
        }

        internal override void MenuItemCallback(object sender, OleMenuCmdEventArgs e)
        {
            var input = (string)e.InValue;
            if (string.IsNullOrEmpty(input))
            {
                if(input == null)
                {
                    // Show Console Window when executed from menu button.
                    consoleContext.Activate();
                }

                consoleContext.WriteLine("Available subcommands");
                consoleContext.WriteLine("");
                foreach (var command in commands)
                {
                    consoleContext.WriteLine(command.Metadata.Name);
                }

                return;
            }

            foreach (var command in commands)
            {
                if (command.Metadata.Name == input)
                {
                    command.Value();
                    return;
                }
            }

            consoleContext.WriteLine($"Unknown command '{input}'.");
        }
    }


}

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.Linq;
using GitHub.VisualStudio.Contrib.Vsix;
using Task = System.Threading.Tasks.Task;

namespace GitHub.VisualStudio.Contrib.Console
{
    [Command(PackageGuids.guidGitHubCommandPackageCmdSetString, PackageIds.GitHubConsoleCommandId)]
    internal sealed class ConsoleCommand : AllowParamsCommandBase
    {
        IConsoleContext consoleContext;
        IEnumerable<Lazy<Action, ISubcommandMetadata>> commands;
        IEnumerable<Lazy<Action<string[]>, ISubcommandMetadata>> commandsWithArgs;
        IEnumerable<Lazy<Func<Task>, ISubcommandMetadata>> asyncCommands;
        IEnumerable<Lazy<Func<string[], Task>, ISubcommandMetadata>> asyncCommandsWithArgs;

        [ImportingConstructor]
        internal ConsoleCommand(Package package, IConsoleContext consoleContext,
            [ImportMany] IEnumerable<Lazy<Action, ISubcommandMetadata>> commands,
            [ImportMany] IEnumerable<Lazy<Action<string[]>, ISubcommandMetadata>> commandsWithArgs,
            [ImportMany] IEnumerable<Lazy<Func<Task>, ISubcommandMetadata>> asyncCommands,
            [ImportMany] IEnumerable<Lazy<Func<string[], Task>, ISubcommandMetadata>> asyncCommandsWithArgs) : base(package)
        {
            this.consoleContext = consoleContext;
            this.commands = commands;
            this.commandsWithArgs = commandsWithArgs;
            this.asyncCommands = asyncCommands;
            this.asyncCommandsWithArgs = asyncCommandsWithArgs;
        }

        internal override void MenuItemCallback(object sender, OleMenuCmdEventArgs e)
        {
            var input = (string)e.InValue;
            if (string.IsNullOrEmpty(input))
            {
                if (input == null)
                {
                    // Show Console Window when executed from menu button.
                    consoleContext.Activate();
                }

                consoleContext.WriteLine("Available subcommands");
                consoleContext.WriteLine("");
                var subcommands = commands.Select(c => c.Metadata)
                    .Concat(commandsWithArgs.Select(c => c.Metadata))
                    .Concat(asyncCommands.Select(c => c.Metadata))
                    .Concat(asyncCommandsWithArgs.Select(c => c.Metadata));
                foreach (var subcommand in subcommands)
                {
                    consoleContext.WriteLine(subcommand.Name);
                }

                return;
            }

            var split = CmdLineToArgvW.SplitArgs(input);
            var commandName = split.First();
            var commandArgs = split.Skip(1).ToArray();

            var command = commands.Where(c => c.Metadata.Name == commandName).Select(c => c.Value).FirstOrDefault();
            if (command != null)
            {
                command();
                return;
            }

            var commandWithArgs = commandsWithArgs.Where(c => c.Metadata.Name == commandName).Select(c => c.Value).FirstOrDefault();
            if (commandWithArgs != null)
            {
                commandWithArgs(commandArgs);
                return;
            }

            var asyncCommand = asyncCommands.Where(c => c.Metadata.Name == commandName).Select(c => c.Value).FirstOrDefault();
            if (asyncCommand != null)
            {
                asyncCommand().FileAndForget(commandName);
                return;
            }

            var asyncCommandWithArgs = asyncCommandsWithArgs.Where(c => c.Metadata.Name == commandName).Select(c => c.Value).FirstOrDefault();
            if (asyncCommandWithArgs != null)
            {
                asyncCommandWithArgs(commandArgs).FileAndForget(commandName);
                return;
            }

            consoleContext.WriteLine($"Unknown command '{input}'.");
        }
    }


}

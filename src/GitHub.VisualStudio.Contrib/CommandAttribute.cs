using System;
using System.Linq;
using System.ComponentModel.Design;
using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib
{
    [MetadataAttribute]
    internal class CommandAttribute : ExportAttribute
    {
        public CommandAttribute(string menuGroup, int commandId) : base(typeof(CommandBase))
        {
            MenuGroup = menuGroup;
            CommandID = commandId;
        }

        public string MenuGroup { get; }
        public int CommandID { get; }

        public CommandID GetCommandID()
        {
            return new CommandID(new Guid(MenuGroup), CommandID);
        }

        public static CommandID GetCommandID(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(CommandAttribute), false);
            var attribute = (CommandAttribute)attributes.FirstOrDefault();
            if(attribute == null)
            {
                throw new ArgumentException($"Type '{type}' doesn't have a '{typeof(CommandAttribute)}' attribute");
            }

            return attribute.GetCommandID();
        }
    }
}

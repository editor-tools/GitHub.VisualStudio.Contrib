using System;
using System.Linq;
using System.ComponentModel.Design;
using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib
{
    [MetadataAttribute]
    internal class CommandIDAttribute : Attribute
    {
        public CommandIDAttribute(string menuGroup, int commandId)
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
            var attributes = type.GetCustomAttributes(typeof(CommandIDAttribute), false);
            var attribute = (CommandIDAttribute)attributes.FirstOrDefault();
            if(attribute == null)
            {
                throw new ArgumentException($"Type '{type}' doesn't have a '{typeof(CommandIDAttribute)}' attribute");
            }

            return attribute.GetCommandID();
        }
    }
}

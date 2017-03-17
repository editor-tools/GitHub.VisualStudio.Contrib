using System;
using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib.Console
{
    [MetadataAttribute]
    public class SubcommandMetadataAttribute : Attribute, ISubcommandMetadata
    {
        public SubcommandMetadataAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}

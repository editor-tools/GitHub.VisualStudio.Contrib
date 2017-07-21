using System;
using System.Windows;
using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib
{
    [Export]
    public partial class ApplicationResources : ResourceDictionary
    {
        public ApplicationResources()
        {
            InitializeComponent();
        }
    }
}

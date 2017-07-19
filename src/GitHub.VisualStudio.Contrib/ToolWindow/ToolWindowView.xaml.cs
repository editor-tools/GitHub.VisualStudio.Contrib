namespace GitHub.VisualStudio.Contrib.ToolWindow
{
    using System.Windows;
    using System.Windows.Controls;
    using System.ComponentModel.Composition;

    [Export]
    public partial class ToolWindowView : UserControl
    {
        public ToolWindowView()
        {
            InitializeComponent();
        }
    }
}
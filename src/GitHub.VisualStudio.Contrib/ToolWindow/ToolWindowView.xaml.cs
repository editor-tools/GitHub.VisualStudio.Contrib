//------------------------------------------------------------------------------
// <copyright file="ToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace GitHub.VisualStudio.Contrib.ToolWindow
{
    using System.Windows;
    using System.Windows.Controls;
    using System.ComponentModel.Composition;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Interaction logic for ToolWindowControl.
    /// </summary>
    [Export]
    public partial class ToolWindowView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindowView"/> class.
        /// </summary>
        public ToolWindowView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "ToolWindow");
        }
    }
}
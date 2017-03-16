using System;
using Microsoft.VisualStudio.Shell;

namespace GitHub.VisualStudio.Contrib
{
    // See https://code.msdn.microsoft.com/windowsdesktop/AllowParams-2005-9442298f
    internal abstract class AllowParamsCommandBase : CommandBase
    {
        public AllowParamsCommandBase(Package package) : base(package)
        {
            Command.ParametersDescription = "$";
        }

        internal override void MenuItemCallback(object sender, EventArgs e)
        {
            MenuItemCallback(sender, (OleMenuCmdEventArgs)e);
        }

        internal abstract void MenuItemCallback(object sender, OleMenuCmdEventArgs e);
    }
}

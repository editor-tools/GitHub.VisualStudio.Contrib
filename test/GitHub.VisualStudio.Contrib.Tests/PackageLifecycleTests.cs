using System;
using EnvDTE;
using EnvDTE80;
using NSubstitute;
using NUnit.Framework;
using Microsoft.VisualStudio.Shell;

namespace GitHub.VisualStudio.Contrib.Tests
{
    public class PackageLifecycleTests
    {
        [Test]
        public void InstallMessage_WhenNotInstalled_ShowStatusBarText()
        {
            var package = Substitute.For<Package>();
            var sp = (IServiceProvider)package;
            var dte = Substitute.For<DTE2>();
            sp.GetService(typeof(DTE)).Returns(dte);

            using (var packageLifecycle = new PackageLifecycle(package)) { }

            dte.StatusBar.Received(1).Text = PackageLifecycle.InstallMessage;
        }
    }
}

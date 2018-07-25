using System.ComponentModel.Composition;
using GitHub.UI;
using GitHub.VisualStudio.Contrib.UI.ViewModels;
using ReactiveUI;

namespace GitHub.VisualStudio.Contrib.UI.Views
{
    public class GenericHelloWorldView : ViewBase<IHelloWorldViewModel, GenericHelloWorldView>
    {
    }

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class HelloWorldView : GenericHelloWorldView
    {
        public HelloWorldView()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
            {
            });
        }
    }
}
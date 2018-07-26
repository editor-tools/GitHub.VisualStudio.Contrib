using System.ComponentModel.Composition;
using GitHub.UI;
using GitHub.Exports;
using GitHub.VisualStudio.Contrib.UI.ViewModels;
using ReactiveUI;

namespace GitHub.VisualStudio.Contrib.UI.Views
{
    public class GenericHelloWorldView : ViewBase<IHelloWorldViewModel, GenericHelloWorldView>
    {
    }

    [ExportViewFor(typeof(IHelloWorldViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
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
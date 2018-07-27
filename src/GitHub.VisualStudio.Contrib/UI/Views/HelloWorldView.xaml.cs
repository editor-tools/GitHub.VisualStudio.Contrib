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
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyObservable(x => x.ViewModel.GoTo.CanExecuteObservable)
                    .BindTo(this, x => x.GoToPanel.Visibility);

                this.WhenAnyObservable(x => x.ViewModel.Clone.CanExecuteObservable)
                    .BindTo(this, x => x.ClonePanel.Visibility);

                this.WhenAnyObservable(x => x.ViewModel.Open.CanExecuteObservable)
                    .BindTo(this, x => x.OpenPanel.Visibility);
            });
        }
    }
}
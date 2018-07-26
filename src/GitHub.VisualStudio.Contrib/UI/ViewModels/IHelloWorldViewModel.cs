using GitHub.ViewModels;
using GitHub.ViewModels.Dialog;
using ReactiveUI;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    public interface IHelloWorldViewModel : IViewModel, IDialogContentViewModel
    {
        IReactiveCommand<object> SayHello { get; }
    }
}
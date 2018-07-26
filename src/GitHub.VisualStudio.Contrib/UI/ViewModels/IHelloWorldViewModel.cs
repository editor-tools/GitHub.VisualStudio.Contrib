using GitHub.ViewModels;
using GitHub.ViewModels.Dialog;
using GitHub.ViewModels.GitHubPane;
using ReactiveUI;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    public interface IHelloWorldViewModel : IViewModel, IDialogContentViewModel, IPanePageViewModel
    {
        IReactiveCommand<object> SayHello { get; }
    }
}
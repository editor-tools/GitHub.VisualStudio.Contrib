using GitHub.Services;
using GitHub.ViewModels;
using GitHub.ViewModels.Dialog;
using GitHub.ViewModels.GitHubPane;
using ReactiveUI;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    public interface IHelloWorldViewModel : IViewModel, IDialogContentViewModel, IPanePageViewModel, IOpenInBrowser
    {
        IReactiveCommand<object> GoTo { get; }

        IReactiveCommand<object> Clone { get; }

        IReactiveCommand<object> Open { get; }

        string TargetUrl { get; set; }

        string BlobName { get; }

        string DefaultPath { get; }

        GitHubContext Context { get; }
    }
}
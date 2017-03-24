using GitHub.ViewModels;
using ReactiveUI;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    public interface IHelloWorldViewModel : IViewModel
    {
        IReactiveCommand<object> SayHello { get; }
    }
}
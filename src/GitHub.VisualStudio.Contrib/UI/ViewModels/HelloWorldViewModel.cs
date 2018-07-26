using System;
using System.Reactive.Linq;
using System.ComponentModel.Composition;
using ReactiveUI;
using GitHub.VisualStudio.Contrib.Console;
using System.Reactive;
using System.Threading.Tasks;
using GitHub.ViewModels.GitHubPane;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    [Export(typeof(IHelloWorldViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HelloWorldViewModel : PanePageViewModelBase, IHelloWorldViewModel
    {
        IConsoleContext console;

        [ImportingConstructor]
        public HelloWorldViewModel(IConsoleContext console)
        {
            this.console = console;
            SayHello = ReactiveCommand.Create();
            SayHello.Subscribe(_ => OnSayHello());

            Done = ReactiveCommand.Create();
        }

        public IReactiveCommand<object> SayHello { get; }

        public IObservable<object> Done { get; }

        void OnSayHello()
        {
            console.WriteLine("Hello, World!");
        }
    }
}
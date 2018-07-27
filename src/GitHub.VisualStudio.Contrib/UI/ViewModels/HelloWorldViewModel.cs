using System;
using System.Reactive.Linq;
using System.ComponentModel.Composition;
using ReactiveUI;
using GitHub.ViewModels.GitHubPane;
using GitHub.VisualStudio.Contrib.Console;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    [Export(typeof(IHelloWorldViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HelloWorldViewModel : PanePageViewModelBase, IHelloWorldViewModel
    {
        Uri webUrl;

        [ImportingConstructor]
        public HelloWorldViewModel(IConsoleContext console)
        {
            SayHello = ReactiveCommand.Create();
            SayHello.Subscribe(_ => console.WriteLine("Hello, World!"));

            WebUrl = new Uri("https://github.com/editor-tools/GitHub.VisualStudio.Contrib");

            Done = ReactiveCommand.Create();
        }

        public IReactiveCommand<object> SayHello { get; }

        public IObservable<object> Done { get; }

        public Uri WebUrl
        {
            get { return webUrl; }
            private set { this.RaiseAndSetIfChanged(ref webUrl, value); }
        }
    }
}
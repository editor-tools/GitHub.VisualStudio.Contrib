using System;
using System.Reactive.Linq;
using System.ComponentModel.Composition;
using ReactiveUI;
using GitHub.ViewModels;
using GitHub.VisualStudio.Contrib.Console;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    [Export(typeof(IHelloWorldViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HelloWorldViewModel : ViewModelBase, IHelloWorldViewModel
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

        public string Title => "Dialog Title";

        public IObservable<object> Done { get; }

        void OnSayHello()
        {
            console.WriteLine("Hello, World!");
        }
    }
}
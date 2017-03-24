﻿using System;
using System.Reactive.Linq;
using System.ComponentModel.Composition;
using ReactiveUI;
using GitHub.ViewModels;
using GitHub.VisualStudio.Contrib.Console;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class HelloWorldViewModel : BaseViewModel, IHelloWorldViewModel
    {
        IConsoleContext console;

        [ImportingConstructor]
        public HelloWorldViewModel(IConsoleContext console)
        {
            this.console = console;
            SayHello = ReactiveCommand.Create();
            SayHello.Subscribe(_ => OnSayHello());
        }

        public IReactiveCommand<object> SayHello { get; }

        void OnSayHello()
        {
            console.WriteLine("Hello, World!");
        }
    }
}
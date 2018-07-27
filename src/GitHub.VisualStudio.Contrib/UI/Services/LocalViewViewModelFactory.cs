using System;
using System.Windows;
using System.ComponentModel.Composition;
using GitHub.Factories;
using GitHub.ViewModels;

namespace GitHub.VisualStudio.Contrib.UI.Services
{
    /// <summary>
    /// This facade ensures that local views registered using <see cref="T:GitHub.Exports.ExportViewForAttribute"/>
    /// are visible to the <see cref="IViewViewModelFactory"/>.
    /// </summary>
    [Export]
    public class LocalViewViewModelFactory : IViewViewModelFactory
    {
        readonly ICompositionService compositionService;
        readonly IViewViewModelFactory factory;

        [ImportingConstructor]
        public LocalViewViewModelFactory(
            ICompositionService compositionService,
            IViewViewModelFactory factory)
        {
            this.compositionService = compositionService;
            this.factory = factory;

            compositionService.SatisfyImportsOnce(factory);
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : IViewModel
        {
            return compositionService.GetExportedValue<TViewModel>();
        }

        public FrameworkElement CreateView<TViewModel>() where TViewModel : IViewModel
        {
            return factory.CreateView<TViewModel>();
        }

        public FrameworkElement CreateView(Type viewModel)
        {
            return factory.CreateView(viewModel);
        }
    }

    static class ICompositionServiceExtensions
    {
        internal static T GetExportedValue<T>(this ICompositionService compositionService)
        {
            var factory = new ExportFactory<T>();
            compositionService.SatisfyImportsOnce(factory);
            return factory.Value;
        }

        class ExportFactory<T>
        {
            [Import]
            public T Value = default(T);
        }
    }
}

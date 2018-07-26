using System.ComponentModel.Composition;

namespace GitHub.VisualStudio.Contrib.Console
{
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

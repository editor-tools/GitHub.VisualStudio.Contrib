using System;
using System.IO;
using System.Reactive.Linq;
using System.ComponentModel.Composition;
using ReactiveUI;
using GitHub.Services;
using GitHub.ViewModels.GitHubPane;

namespace GitHub.VisualStudio.Contrib.UI.ViewModels
{
    [Export(typeof(IHelloWorldViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HelloWorldViewModel : PanePageViewModelBase, IHelloWorldViewModel
    {
        readonly IGitHubContextService contextService;
        readonly ITeamExplorerContext teamExplorerContext;

        GitHubContext context;
        string targetUrl;
        string blobName;
        string defaultPath;
        Uri webUrl;

        [ImportingConstructor]
        public HelloWorldViewModel(
            IGitHubContextService contextService,
            ITeamExplorerContext teamExplorerContext,
            IRepositoryCloneService repositoryCloneService,
            IGitHubServiceProvider serviceProvider)
        {
            this.contextService = contextService;
            this.teamExplorerContext = teamExplorerContext;

            Title = "GitHub URL";

            GoTo = ReactiveCommand.Create(this.WhenAnyValue(x => x.BlobName).Select(b => b != null));
            GoTo.Subscribe(_ =>
            {
                var localPath = teamExplorerContext.ActiveRepository?.LocalPath;
                contextService.TryOpenFile(localPath, Context);
            });

            Clone = ReactiveCommand.Create(this.WhenAnyValue(x =>
                x.DefaultPath).Select(d => d is string dir && !Directory.Exists(d)));
            Clone.Subscribe(_ =>
            {
                // await repositoryCloneService.CloneRepository(cloneUrl, repositoryDirName, targetDir);
            });

            Open = ReactiveCommand.Create(this.WhenAnyValue(x =>
                x.DefaultPath).Select(d => d is string dir && Directory.Exists(d)));
            Open.Subscribe(_ =>
            {
                var dte = serviceProvider.GetService<EnvDTE.DTE>();
                dte.ExecuteCommand("File.OpenFolder", DefaultPath);
                dte.ExecuteCommand("View.TfsTeamExplorer");
                contextService.TryOpenFile(DefaultPath, Context);
            });

            Context = contextService.FindContextFromClipboard();
            TargetUrl = Context?.Url;

            this.WhenAnyValue(x => x.TargetUrl).Subscribe(u =>
            {
                Context = contextService.FindContextFromUrl(u);
            });

            this.WhenAnyValue(x => x.Context).Subscribe(c =>
            {
                BlobName = c?.BlobName;

                DefaultPath =
                    repositoryCloneService.DefaultClonePath is string home &&
                    context?.Owner is string owner &&
                    context?.RepositoryName is string repositoryName ?
                    Path.Combine(home, owner, repositoryName) : null;
            });

            Done = ReactiveCommand.Create();
        }

        public string TargetUrl
        {
            get { return targetUrl; }
            set { this.RaiseAndSetIfChanged(ref targetUrl, value); }
        }

        public GitHubContext Context
        {
            get { return context; }
            private set { this.RaiseAndSetIfChanged(ref context, value); }
        }

        public string BlobName
        {
            get { return blobName; }
            private set { this.RaiseAndSetIfChanged(ref blobName, value); }
        }

        public string DefaultPath
        {
            get { return defaultPath; }
            private set { this.RaiseAndSetIfChanged(ref defaultPath, value); }
        }

        public Uri WebUrl
        {
            get { return webUrl; }
            private set { this.RaiseAndSetIfChanged(ref webUrl, value); }
        }

        public IReactiveCommand<object> GoTo { get; }

        public IReactiveCommand<object> Clone { get; }

        public IReactiveCommand<object> Open { get; }

        public IObservable<object> Done { get; }
    }
}
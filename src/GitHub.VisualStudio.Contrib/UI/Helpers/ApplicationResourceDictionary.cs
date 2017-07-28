using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Markup;
using GitHub.VisualStudio.UI.Helpers;

namespace GitHub.UI.Helpers
{
    public class ApplicationResourceDictionary : ResourceDictionary
    {
        static bool isInDesignMode;
        Uri applicationResources;

        static ApplicationResourceDictionary()
        {
            isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
        }

        public Uri ApplicationResources
        {
            get
            {
                return applicationResources;
            }

            set
            {
                applicationResources = value;

                if (!isInDesignMode)
                {
                    MergeApplicationResources(value);
                }
            }
        }

        void MergeApplicationResources(Uri source)
        {
            var resources = Application.Current.Resources;

            var uriContext = (IUriContext)this;
            var uri = new Uri(uriContext.BaseUri, source);
            var sourceUrl = uri.ToString();
            foreach (var child in resources.MergedDictionaries)
            {
                if(child.ToString() == sourceUrl)
                {
                    if (child.GetType() == typeof(ApplicationMergedDictionary))
                    {
                        return;
                    }

                    if (child.GetType().FullName == typeof(ApplicationMergedDictionary).FullName)
                    {
                        resources.MergedDictionaries.Remove(child);
                        (child as IDisposable)?.Dispose();
                        break;
                    }
                }
            }

            var applicationMergedDictionary = new ApplicationMergedDictionary(uri);
            resources.MergedDictionaries.Add(applicationMergedDictionary);
        }

        class ApplicationMergedDictionary : ResourceDictionary
        {
            readonly string url;

            public ApplicationMergedDictionary(Uri uri)
            {
                url = uri.ToString();
                //var dictionary = new ResourceDictionary { Source = uri };
                var dictionary = new ThemeDictionaryManager { Source = uri };
                MergedDictionaries.Add(dictionary);
            }

            public override string ToString()
            {
                return url;
            }
        }
    }
}

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.PlatformUI;
using System.ComponentModel;

namespace GitHub.VisualStudio.Contrib.ToolWindow
{
    [Export]
    public class ToolWindowViewModel : INotifyPropertyChanged
    {
        string text;

        public ToolWindowViewModel()
        {
            HelloCommand = new DelegateCommand(p  =>
            {
                System.Diagnostics.Trace.WriteLine("hmmmmmmmmmmmmmmmmmmmm");
                Text = "Hello, World!";
            });
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if(text != value)
                {
                    text = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
        }

        public ICommand HelloCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

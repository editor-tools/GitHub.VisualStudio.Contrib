namespace GitHub.VisualStudio.Contrib.Console
{
    public interface IConsoleContext
    {
        void Write(string text);
        void WriteLine(string text);
        void Activate();
    }
}
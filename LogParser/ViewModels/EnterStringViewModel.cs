using Caliburn.Micro;

namespace LogParser.ViewModels
{
    public sealed class EnterStringViewModel : Screen
    {
        public string Value { get; set; }

        public void Accept() { TryClose(true); }
        public void Cancel() { TryClose(false); }
    }
}
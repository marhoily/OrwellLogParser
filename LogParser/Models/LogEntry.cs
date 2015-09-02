namespace LogParser.ViewModels
{
    public sealed class LogEntry
    {
        private string _line;

        public string Line
        {
            get { return _line; }
            set { }
        }

        public LogLine Attrs { get; set; }

        public LogEntry(string line, LogLine attrs)
        {
            _line = line;
            Attrs = attrs;
        }
    }
}
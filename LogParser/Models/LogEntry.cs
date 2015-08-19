using System;

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

        public DateTime Timestamp { get; set; }
        public int GroupId { get; set; }

        public LogEntry(DateTime timestamp, string line, int groupId)
        {
            Timestamp = timestamp;
            _line = line;
            GroupId = groupId;
        }
    }
}
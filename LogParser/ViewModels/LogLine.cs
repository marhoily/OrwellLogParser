using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace LogParser.ViewModels
{
    public class LogLine
    {
        public List<string> Lines { get; }
        public string Line => string.Join("\r\n", Lines);
        public DateTime Timestamp { get; set; }
        public int Group { get; set; }

        public LogLine(string line)
        {
            Lines = new List<string> { line};
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace LogParser.ViewModels
{
    public sealed class LogLine
    {
        public List<string> Lines { get; private set; }
        public string Line
        {
            get { return string.Join("\r\n", Lines); }
            set {  }
        }

        public DateTime Timestamp { get; set; }
        public int GroupId { get; set; }
        public string Level { get; set; }
        public string Sid { get; set; }
        public string Method { get; set; }
        public string MethodAttr { get; set; }

        public LogLine(string line)
        {
            Lines = new List<string> { line};
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace LogParser.ViewModels
{
    public sealed class LogLine
    {
        public List<string> Lines { get; }
        public string Line
        {
            get { return string.Join("\r\n", Lines); }
            set {  }
        }

        public DateTime Timestamp { get; set; }
        public int GroupId { get; set; }

        public LogLine(string line)
        {
            Lines = new List<string> { line};
        }
    }
}
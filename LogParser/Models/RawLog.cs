using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LogParser.ViewModels
{
    public sealed class RawLog
    {
        public List<LogLine> AllLines { get; private set; }
        public int GroupsCount { get; private set; }

        public RawLog(FileInfo logFile)
        {
            AllLines = File.ReadAllLines(logFile.FullName)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new LogLine(s))
                .ToList();

            AllLines = GroupByNewLine(AllLines).ToList();
            GroupByTimestamp(AllLines);
            GroupBySid(AllLines);
        }

        private void GroupByTimestamp(List<LogLine> readAllLines)
        {
            var currentGroup = 0;
            var last = AllLines[0].Timestamp;
            foreach (var readAllLine in readAllLines)
            {
                if (readAllLine.Timestamp - last > TimeSpan.FromMinutes(10))
                    currentGroup++;
                last = readAllLine.Timestamp;
                readAllLine.GroupId = currentGroup;
            }
            GroupsCount = currentGroup;
        }
        private void GroupBySid(List<LogLine> readAllLines)
        {
            var counter = 0;
            foreach (var group in readAllLines.ToLookup(l => l.Sid, l => l))
            {
                counter++;
                foreach (var logLine in group)
                {
                    logLine.Sid = counter.ToString();
                }
            }
        }

        private static IEnumerable<LogLine> GroupByNewLine(List<LogLine> readAllLines)
        {
            var current = default(LogLine);
            foreach (var t in readAllLines)
            {
                // t:09/02/2015 12.45.58.970918;
                var substring = t.Lines[0].Substring(2, 26);
                DateTime value;
                if (DateTime.TryParseExact(substring,
                    "MM/dd/yyyy HH.mm.ss.ffffff",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out value))
                {
                    t.Lines[0] = t.Lines[0].Substring(30);
                    if (t.Lines[0].StartsWith("Debug:"))
                    {
                        t.Lines[0] = t.Lines[0].Substring(7);
                        t.Level = "Debug";
                    }
                    if (t.Lines[0].StartsWith("Error:"))
                    {
                        t.Lines[0] = t.Lines[0].Substring(7);
                        t.Level = "Error";
                    }
                    if (t.Lines[0].StartsWith("s:"))
                    {
                        t.Sid = t.Lines[0].Substring(2, 36);
                        t.Lines[0] = t.Lines[0].Substring(40);
                    }
                    if (t.Lines[0].StartsWith("m:"))
                    {
                        var idx = t.Lines[0].IndexOf(';');
                        if (idx != -1)
                        {
                            t.Method = t.Lines[0].Substring(2, idx - 2);
                            t.Lines[0] = t.Lines[0].Substring(idx + 2);
                        }
                    }
                    if (t.Lines[0].StartsWith("a:"))
                    {
                        var idx = t.Lines[0].IndexOf(';');
                        if (idx != -1)
                        {
                            t.MethodAttr = t.Lines[0].Substring(2, idx - 2);
                            t.Lines[0] = t.Lines[0].Substring(idx + 2);
                        }
                    }
                    if (t.Lines[0].StartsWith("data:"))
                    {
                        t.Lines[0] = t.Lines[0].Substring(7, t.Lines[0].Length - 9);
                    }
                    if (current != null)
                        yield return current;
                    current = t;
                    current.Timestamp = value;
                }
                else
                {
                    current.Lines.Add(t.Lines[0]);
                }
            }
            if (current != null)
                yield return current;


        }

    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LogParser.ViewModels
{
    public sealed class RawLog
    {
        public List<LogLine> AllLines { get; }
        public int GroupsCount { get; private set; }

        public RawLog(FileInfo logFile)
        {
            AllLines = File.ReadAllLines(logFile.FullName)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new LogLine(s))
                .ToList();

            AllLines = GroupByNewLine(AllLines).ToList();
            GroupByTimestamp(AllLines);
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

        private static IEnumerable<LogLine> GroupByNewLine(List<LogLine> readAllLines)
        {
            var current = default(LogLine);
            foreach (var t in readAllLines)
            {
                // <08/14/2015 11.24.01.663388>
                var substring = t.Lines[0].Substring(1, 26);
                DateTime value;
                if (DateTime.TryParseExact(substring,
                    "MM/dd/yyyy hh.mm.ss.ffffff",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out value))
                {
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Caliburn.Micro;

namespace LogParser.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly List<LogLine> _readAllLines;

        public ShellViewModel()
        {
            Groups = new List<int>();
            Filters = new List<string> { "OnDisconnected", "Account is in active"/*, "UpdatePresence"*/ };

            _readAllLines = File.ReadAllLines(@"C:\Users\ilm\Documents\bug #1\DpbRoutingEngineLog_sgw20049921_14Aug2015.log")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new LogLine(s))
                .ToList();

            _readAllLines = Group1(_readAllLines).ToList();
            Group2(_readAllLines);
            Filter();
        }

        private void Filter()
        {
            FilteredLines = _readAllLines
                .Where(l => l.Group == SelectedGroup)
                .Where(l => Filters.Any(f => Satisfies(l.Line, f)))
                .ToList();
            NotifyOfPropertyChange(() => FilteredLines);
        }

        private void Group2(List<LogLine> readAllLines)
        {
            var currentGroup = 0;
            var last = _readAllLines[0].Timestamp;
            foreach (var readAllLine in readAllLines)
            {
                if (readAllLine.Timestamp - last > TimeSpan.FromMinutes(10))
                    currentGroup++;
                last = readAllLine.Timestamp;
                readAllLine.Group = currentGroup;
            }
            for (var i = 0; i < currentGroup; i++)
                Groups.Add(i);

        }

        private static IEnumerable<LogLine> Group1(List<LogLine> readAllLines)
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

        // public override string DisplayName => "Shell Window";

        public List<LogLine> FilteredLines { get; set; }
        public List<int> Groups { get; set; }

        public int SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                Filter();
            }
        }

        private static bool Satisfies(string line, string filter)
        {
            return line.Contains(filter);
        }

        public readonly List<string> Filters;
        private int _selectedGroup;
    }
}

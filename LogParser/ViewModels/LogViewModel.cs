using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;

namespace LogParser.ViewModels
{
    public sealed class LogViewModel : PropertyChangedBase
    {
        private readonly RawLog _logFile;

        public LogViewModel(RawLog logFile)
        {
            _logFile = logFile;
            Groups = new List<int>();
            Filters = new List<string> { "OnDisconnected", "Account is in active", "Started HubAgent\\SubscribeCustomerAsync"/*, "UpdatePresence"*/ };

            for (var i = 0; i < logFile.GroupsCount; i++)
                Groups.Add(i);

            Filter();

        }
        private void Filter()
        {
            FilteredLines = _logFile.AllLines
                .Where(l => l.Group == SelectedGroup)
                .Where(l => Filters.Any(f => Satisfies(l.Line, f)))
                .ToList();
            NotifyOfPropertyChange(() => FilteredLines);
        }

        // public override string DisplayName => "Shell Window";

        public List<LogLine> FilteredLines { get; set; }
        public List<int> Groups { get; }

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
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;

namespace LogParser.ViewModels
{
    public sealed class LogViewModel : PropertyChangedBase
    {
        private readonly List<Filter> _filters;
        private readonly RawLog _logFile;
        private int _selectedGroup;

        public LogViewModel(RawLog logFile)
        {
            _logFile = logFile;
            Groups = new List<int>();
            _filters = new List<Filter>
            {
                Filter.S1(@"OnDisconnected", x => "OnDisconnected: " + x.FromSubstringTillTheEndoOfLine("ConnectionId:")),
                Filter.S1(@"Unknown error ConnectionId:", x => "Account is in active collaboration: " + x.FromSubstringTillTheEndoOfLine("ConnectionId:")),
                Filter.S1(@"Started HubAgent\SubscribeCustomerAsync"),
                Filter.S1(@"UpdatePresence Payload", x => x.RemoveAllBefore("UpdatePresence")),
                Filter.S1(@"Started HubAgent\UnsubscribeCustomerAsync"),
                Filter.S1(@"Started HubAgent\UnsubscribeEmployeeAsync")
            };

            for (var i = 0; i < logFile.GroupsCount; i++)
                Groups.Add(i);

            DoFilter();
        }

        public List<LogEntry> FilteredLines { get; set; }
        public List<int> Groups { get; set; }

        public int SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                DoFilter();
            }
        }

        private void DoFilter()
        {
            FilteredLines = _logFile.AllLines
                .Where(l => l.GroupId == SelectedGroup)
                .Where(l => _filters.Any(f => f.Test(l.Line)))
                .Select(l => new LogEntry(
                    l.Timestamp,
                    _filters.First(f => f.Test(l.Line)).Display(l.Line),
                    l.GroupId))
                .ToList();
            NotifyOfPropertyChange(() => FilteredLines);
        }
    }
}
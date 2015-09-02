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
                Filter.S1(x => x.Method == @"Hub-OnDisconnected" && x.MethodAttr == "start"),
                Filter.S1(x => x.Method == @"HubAgent-SubscriptionDisconnected"),
                Filter.S1(x => x.Method == @"HubAgent-Signal_Subscribed" && x.MethodAttr == "start"),
                Filter.S1(x => x.Method == @"HubAgent-SubscribeCustomerAsync"),
                Filter.S1(x => x.Line.Contains(@"Message:Account is in active collaboration")),
            };

            for (var i = 0; i < logFile.GroupsCount; i++)
                Groups.Add(i);

            DoFilter();
        }

        public List<LogLine> FilteredLines { get; set; }
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
                .Where(l => _filters.Any(f => f.Test(l)))
              //  .Select(l => new LogEntry(_filters.First(f => f.Test(l)).Display(l.Line), l))
                .ToList();
            NotifyOfPropertyChange(() => FilteredLines);
        }
    }
}
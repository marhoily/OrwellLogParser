using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

namespace LogParser.ViewModels
{
    public sealed class LogViewModel : PropertyChangedBase
    {
        private readonly List<Filter> _filters;
        private readonly List<Filter> _andFilters = new List<Filter>();
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

        public void ExcludeSif()
        {
            var enterStringViewModel = new EnterStringViewModel();
            if (IoC.Get<IWindowManager>().ShowDialog(enterStringViewModel) != true) return;
            int result;
            if (int.TryParse(enterStringViewModel.Value, out result))
            {
                var str = result.ToString();
                _andFilters.Add(Filter.S1(l => l.Sid != str));
                DoFilter();
            }
            else
            {
                MessageBox.Show("Int is expected!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
                .Where(l => _andFilters.All(f => f.Test(l)))
                .ToList();
            NotifyOfPropertyChange(() => FilteredLines);
        }
    }
}
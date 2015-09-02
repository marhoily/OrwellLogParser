using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using LogParser.Models;

namespace LogParser.ViewModels
{
    public sealed class FiltersViewModel : PropertyChangedBase
    {
        private int _selectedGroup;

        public FiltersViewModel(RawLog logFile)
        {
            AndFilters = new ObservableCollection<Filter>();
            LogView = new LogViewModel(this, logFile);
            Groups = new List<int>();
            Filters = new ObservableCollection<Filter>
            {
                Filter.S1(x => x.Method == @"Hub-OnDisconnected" && x.MethodAttr == "start"),
                Filter.S1(x => x.Method == @"HubAgent-SubscriptionDisconnected"),
                Filter.S1(x => x.Method == @"HubAgent-Signal_Subscribed" && x.MethodAttr == "start"),
                Filter.S1(x => x.Method == @"HubAgent-SubscribeCustomerAsync"),
                Filter.S1(x => x.Line.Contains(@"Message:Account is in active collaboration"))
            };

            for (var i = 0; i < logFile.GroupsCount; i++)
                Groups.Add(i);

            DoFilter();

            IoC.Get<IWindowManager>().ShowWindow(LogView);
        }

        public ObservableCollection<Filter> Filters { get; private set; }
        public ObservableCollection<Filter> AndFilters { get; private set; }
        public LogViewModel LogView { get; private set; }

        public List<int> Groups { get; set; }

        public void RemoveFilter(Filter filter)
        {
            Filters.Remove(filter);
            AndFilters.Remove(filter);
            DoFilter();
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

        public void ExcludeSid(LogLine line)
        {
            var str = line.Sid;
            AndFilters.Add(Filter.S1(l => l.Sid != str));
            DoFilter();
        }

        public void ExcludeSif()
        {
            var enterStringViewModel = new EnterStringViewModel();
            if (IoC.Get<IWindowManager>().ShowDialog(enterStringViewModel) != true) return;
            int result;
            if (int.TryParse(enterStringViewModel.Value, out result))
            {
                var str = result.ToString();
                AndFilters.Add(Filter.S1(l => l.Sid != str));
                DoFilter();
            }
            else
            {
                MessageBox.Show("Int is expected!", "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DoFilter()
        {
            LogView.DoFilter(SelectedGroup, Filters, AndFilters);
        }
    }
}
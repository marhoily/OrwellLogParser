using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using LogParser.Models;

namespace LogParser.ViewModels
{
    public sealed class LogViewModel : Screen
    {
        public FiltersViewModel FiltersView { get; set; }
        private readonly RawLog _logFile;

        public LogViewModel(FiltersViewModel filtersView, RawLog logFile)
        {
            FiltersView = filtersView;
            _logFile = logFile;
        }

        public List<LogLine> FilteredLines { get; set; }

        public void DoFilter(int selectedGroup, IEnumerable<Filter> filters, IEnumerable<Filter> andFilters)
        {
            FilteredLines = _logFile.AllLines
                .Where(l => l.GroupId == selectedGroup)
                .Where(l => filters.Any(f => f.Test(l)))
                .Where(l => andFilters.All(f => f.Test(l)))
                .ToList();
            NotifyOfPropertyChange(() => FilteredLines);
        }
    }
}
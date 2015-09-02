using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using LogParser.ViewModels;

namespace LogParser.Utilities
{
    public sealed class GoToDataGridOwner_DataContext_FiltersView : IValueConverter
    {
        public static GoToDataGridOwner_DataContext_FiltersView Instance { get; private set; }

        static GoToDataGridOwner_DataContext_FiltersView()
        {
            Instance = new GoToDataGridOwner_DataContext_FiltersView();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var c = value as DataGridColumn;

            if (c == null) return value;
            var type = c.GetType();
            var propertyInfo = type.GetProperty("DataGridOwner",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var convert = (DataGrid)propertyInfo.GetValue(c, null);
            var logViewModel = (LogViewModel)convert.DataContext;

            return logViewModel.FiltersView;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;

        }
    }
}
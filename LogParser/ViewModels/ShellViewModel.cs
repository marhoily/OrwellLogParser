using System.IO;
using Caliburn.Micro;
using LogParser.Models;

namespace LogParser.ViewModels
{
    public class ShellViewModel : Screen
    {
        private FileInfo _selectedFile;

        public ShellViewModel()
        {
            var folder = @"C:\Users\ilm\Documents\MuMuLi";
            Files = new DirectoryInfo(folder)
                .GetFiles("*.*", SearchOption.AllDirectories);

        }

        public FileInfo[] Files { get; set; }

        public FileInfo SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                FiltersViewModel = new FiltersViewModel(new RawLog(_selectedFile));
                NotifyOfPropertyChange(() => FiltersViewModel);
            }
        }

        public FiltersViewModel FiltersViewModel { get; set; }
      
    }
}

using System.IO;
using Caliburn.Micro;

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
                LogViewModel = new LogViewModel(new RawLog(_selectedFile));
                NotifyOfPropertyChange(() => LogViewModel);
            }
        }

        public LogViewModel LogViewModel { get; set; }
      
    }
}

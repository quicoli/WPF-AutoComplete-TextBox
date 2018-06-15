using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace WpfControls.CS.Test
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region "Fields"

        private ICommand _cancelCommand;
        private ICommand _openCommand;

        private FileSystemInfo _selectedPath;
        #endregion

        #region "Constructor"
        public MainWindowViewModel()
        {
            SelectedPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }
        #endregion

        #region "Events"

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region "Properties"

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(ExecuteCancelCommand, null));

        public string FileName { get; set; }

        public ICommand OpenCommand => _openCommand ?? (_openCommand = new DelegateCommand(ExecuteOpenCommand, null));

        public FileSystemInfo SelectedPath
        {
            get => _selectedPath;
            set { _selectedPath = value; RaisePropertyChanged("SelectedPath"); }
        }
        #endregion

        #region "Methods"

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteCancelCommand(object param)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteOpenCommand(object param)
        {
            try
            {
                Process.Start(SelectedPath.FullName);
                Application.Current.Shutdown();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }
}

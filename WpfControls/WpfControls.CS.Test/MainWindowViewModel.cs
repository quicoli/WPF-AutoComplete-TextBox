

namespace WpfControls.CS.Test
{

    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Input;
    using System.Windows;

    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region "Fields"

        private ICommand _cancelCommand;
        private string _fileName;
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

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(ExecuteCancelCommand, null);
                }
                return _cancelCommand;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new DelegateCommand(ExecuteOpenCommand, null);
                }
                return _openCommand;
            }
        }

        public FileSystemInfo SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; RaisePropertyChanged("SelectedPath"); }
        }
        #endregion

        #region "Methods"

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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

            }
        }

        #endregion

    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleDemo.Annotations;

namespace SimpleDemo
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private string selectedNumber;

        public string SelectedNumber
        {
            get => selectedNumber;
            set
            {
                selectedNumber = value;
                OnPropertyChanged();
            }
        }

        private string selectedNumber2;

        public string SelectedNumber2
        {
            get => selectedNumber2;
            set
            {
                selectedNumber2 = value;
                OnPropertyChanged();
            }
        }

        private string selectedString;

        public string SelectedString
        {
            get => selectedString;
            set
            {
                selectedString = value;
                OnPropertyChanged();
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using CommunityToolkit.Mvvm.Input;
using SimpleDemo.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SimpleDemo
{
    public partial class MainWindowViewModel : INotifyPropertyChanged
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

        private string selectedFruit;

        public string SelectedFruit
        {
            get => selectedFruit;
            set
            {
                selectedFruit = value;
                OnPropertyChanged();
            }
        }

        private string typedFruit;

        public string TypedFruit
        {
            get => typedFruit;
            set
            {
                typedFruit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> fruits;
        public ObservableCollection<string> Fruits
        {
            get => fruits;
            set
            {
                fruits = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            Fruits = new ObservableCollection<string>();
            Fruits.Add("apple");
            Provider = new FruitsProvider(ref fruits);
        }

        private FruitsProvider provider;
        public FruitsProvider Provider
        {
            get => provider;
            set
            {
                provider = value;
                OnPropertyChanged();
            }
        }

        [RelayCommand]
        private void ConfirmAdd()
        {
            if (SelectedFruit == null && !Fruits.Any(x => x.Equals(TypedFruit, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                if (MessageBox.Show($"Fruit {TypedFruit} isn't in the list. Do you want to add it?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Fruits.Add(TypedFruit);
                    SelectedFruit = Fruits.First(x => x == TypedFruit);
                }
            }

        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
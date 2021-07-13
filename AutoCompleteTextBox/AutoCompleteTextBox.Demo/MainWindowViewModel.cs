using AutoCompleteTextBox.Demo.Model;
using JulMar.Windows.Validations;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace AutoCompleteTextBox.Demo
{
    public class MainWindowViewModel : ValidatingViewModel
    {
        private ObservableCollection<Person> _people;
        private Person _selectedPerson;

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set { SetPropertyValue(ref _selectedPerson, value); }
        }

        public ObservableCollection<Person> People
        {
            get => _people;
            private set { SetPropertyValue(ref _people, value); }
        }

        public MainWindowViewModel()
        {
            People = new ObservableCollection<Person>()
            {
                new Person() { Name = "Chris Lee", State = StateFactory.FromAbbreviation("NY"), Color = WebColorsFactory.FromColor(Colors.DarkGoldenrod) },
                new Person() { Name = "David Smith", State = StateFactory.FromAbbreviation("MS"), Color = WebColorsFactory.FromColor(Colors.Blue) },
                new Person() { Name = "Jane Allen", State = StateFactory.FromAbbreviation("AL"), Color = WebColorsFactory.FromColor(Colors.Aqua) },
                new Person() { Name = "John Doe", State = StateFactory.FromAbbreviation("TN"), Color = WebColorsFactory.FromColor(Colors.AliceBlue) },
                new Person() { Name = "Maria Hernandez", State = StateFactory.FromAbbreviation("CA"), Color = WebColorsFactory.FromColor(Colors.ForestGreen) },
            };

            SelectedPerson = People[0];
        }
    }
}
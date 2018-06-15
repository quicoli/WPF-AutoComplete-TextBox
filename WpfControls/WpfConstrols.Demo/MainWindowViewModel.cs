using JulMar.Windows.Validations;
using WpfConstrols.Demo.Model;

namespace WpfConstrols.Demo
{
    public class MainWindowViewModel: ValidatingViewModel
    {
        private State _selectedState;
        private string _name;

        [System.ComponentModel.DataAnnotations.Required]
        public State SelectedState
        {
            get => _selectedState;
            set { _selectedState = value; RaisePropertyChanged(()=> SelectedState); }
        }


        [System.ComponentModel.DataAnnotations.Required]
        public string Name
        {
            get => _name;
            set { _name = value; RaisePropertyChanged(()=>Name); }
        }
    }
}
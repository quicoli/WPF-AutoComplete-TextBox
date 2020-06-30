using JulMar.Windows.Validations;

namespace AutoCompleteTextBox.Demo.Model
{
    public class Person : ValidatingViewModel
    {
        private State _state;
        private string _name;
        private WebColor _webColor;

        [System.ComponentModel.DataAnnotations.Required]
        public State State
        {
            get => _state;
            set { SetPropertyValue(ref _state, value); }
        }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name
        {
            get => _name;
            set { SetPropertyValue(ref _name, value); }
        }

        public WebColor Color
        {
            get => _webColor;
            set { SetPropertyValue(ref _webColor, value); }
        }
    }
}

using System.Windows;
using System.Windows.Data;

namespace AutoCompleteTextBox
{
    public class BindingEvaluator : FrameworkElement
    {

        #region "Fields"


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(BindingEvaluator), new FrameworkPropertyMetadata(string.Empty));

        #endregion

        #region "Constructors"

        public BindingEvaluator(Binding binding)
        {
            ValueBinding = binding;
        }

        #endregion

        #region "Properties"

        public string Value
        {
            get => (string)GetValue(ValueProperty);

            set => SetValue(ValueProperty, value);
        }

        public Binding ValueBinding { get; set; }

        #endregion

        #region "Methods"

        public string Evaluate(object dataItem)
        {
            DataContext = dataItem;
            SetBinding(ValueProperty, ValueBinding);
            return Value;
        }

        #endregion
    }
}
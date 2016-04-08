namespace WpfControls
{

    using System.Windows;
    using System.Windows.Data;
    public class BindingEvaluator : FrameworkElement
    {

        #region "Fields"


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(BindingEvaluator), new FrameworkPropertyMetadata(string.Empty));

        private Binding _valueBinding;
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
            get { return (string)GetValue(ValueProperty); }

            set { SetValue(ValueProperty, value); }
        }

        public Binding ValueBinding
        {
            get { return _valueBinding; }
            set { _valueBinding = value; }
        }

        #endregion

        #region "Methods"

        public string Evaluate(object dataItem)
        {
            this.DataContext = dataItem;
            SetBinding(ValueProperty, ValueBinding);
            return Value;
        }

        #endregion

    }

}

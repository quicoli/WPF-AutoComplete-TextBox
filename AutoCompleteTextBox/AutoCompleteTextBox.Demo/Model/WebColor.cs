using JulMar.Windows.Mvvm;
using System.Diagnostics;
using System.Windows.Media;

namespace AutoCompleteTextBox.Demo.Model
{
    [DebuggerDisplay("WebColor = {Name} {HexCode}")]
    public class WebColor : SimpleViewModel
    {
        private string _name;
        private string _hexCode;
        private string _decimalCode;
        private Color _color;
        private SolidColorBrush _brush;

        public string Name
        {
            get { return _name; }
            set { SetPropertyValue(ref _name, value); }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                SetPropertyValue(ref _color, value);
                Brush = new SolidColorBrush(value);
                HexCode = $"#{value.R:X2}{value.G:X2}{value.B:X2}";
                DecimalCode = $"{value.R}, {value.G}, {value.B}";
            }
        }

        public SolidColorBrush Brush
        {
            get { return _brush; }
            set { SetPropertyValue(ref _brush, value); }
        }

        public string HexCode
        {
            get { return _hexCode; }
            private set { SetPropertyValue(ref _hexCode, value); }
        }

        public string DecimalCode
        {
            get { return _decimalCode; }
            private set { SetPropertyValue(ref _decimalCode, value); }
        }

        public WebColor()
        { }

        public WebColor(Color c, string name = null)
        {
            Name = name ?? c.GetType().Name;
            Color = c;
        }
    }
}

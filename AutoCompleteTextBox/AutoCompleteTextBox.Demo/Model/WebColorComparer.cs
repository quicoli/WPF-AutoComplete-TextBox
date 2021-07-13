using System.Collections.Generic;
using System.Windows.Media;

namespace AutoCompleteTextBox.Demo.Model
{
    /// <summary>
    /// Sort color by HSB
    /// </summary>
    public class WebColorComparer : IComparer<WebColor>
    {
        private static readonly Comparer<float> floatComparer = Comparer<float>.Default;

        public static System.Drawing.Color ToDrawingColor(Color c)
        {
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public int Compare(WebColor x, WebColor y)
        {
            if (x == null)
                return y == null ? 0 : 1;
            if (y == null)
                return -1;

            var color1 = ToDrawingColor(x.Color);
            var color2 = ToDrawingColor(y.Color);

            int result = floatComparer.Compare(color1.A, color2.A);
            if (result != 0) return result;

            result = floatComparer.Compare(color1.GetHue(), color2.GetHue());
            if (result != 0) return result;

            result = floatComparer.Compare(color1.GetSaturation(), color2.GetSaturation());
            if (result != 0) return result;

            result = floatComparer.Compare(color1.GetBrightness(), color2.GetBrightness());
            if (result != 0) return result;

            return 0;
        }
    }
}

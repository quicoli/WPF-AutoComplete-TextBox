using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace AutoCompleteTextBox.Demo.Model
{
    public static class WebColorsFactory
    {
        private static readonly Lazy<List<WebColor>> webColors = new Lazy<List<WebColor>>(GetWebColors);

        private static List<WebColor> GetWebColors()
        {
            var data = GetConstants(typeof(Colors));
            return data.Select(kvp => new WebColor(kvp.Value, kvp.Key)).ToList();
        }

        public static IList<WebColor> Create()
        {
            return webColors.Value;
        }

        static List<KeyValuePair<string, Color>> GetConstants(Type sourceType)
        {
            var properties = sourceType
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.PropertyType == typeof(Color));
            var list = new List<KeyValuePair<string, Color>>();
            MethodInfo getMethod = null;
            foreach (var info in properties)
            {
                if (getMethod == null)
                    getMethod = info.GetGetMethod();
                var c = (Color)info.GetValue(null, null);
                list.Add(new KeyValuePair<string, Color>(info.Name, c));
            }
            return list;
        }

        public static WebColor FromColor(Color color)
        {
            return webColors.Value.FirstOrDefault(c => c.Color == color);
        }

        public static WebColor FromName(string name)
        {
            return webColors.Value.FirstOrDefault(c => string.Equals(name, c.Name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

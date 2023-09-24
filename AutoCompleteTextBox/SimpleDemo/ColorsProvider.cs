using AutoCompleteTextBox.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace SimpleDemo
{
    public sealed class ColorsProvider : IComboSuggestionProvider
    {
        private static readonly IReadOnlyList<string> _wpfColorNames = typeof(Colors)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Select(x => x.Name).ToArray();

        public IEnumerable GetSuggestions(string filter)
        {
            return _wpfColorNames.Where(x => x.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable GetFullCollection() => _wpfColorNames;
    }
}

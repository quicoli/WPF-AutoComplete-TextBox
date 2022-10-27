using AutoCompleteTextBox.Editors;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleDemo
{
    public sealed class FruitsProvider : ISuggestionProvider
    {
        private readonly ObservableCollection<string> fruits;

        public IEnumerable GetSuggestions(string filter)
        {
            return fruits.Where(x => x.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase));
        }

        public FruitsProvider(ref ObservableCollection<string> fruits)
        {
            this.fruits = fruits;
        }
    }
}

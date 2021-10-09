using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoCompleteTextBox.Editors;

namespace SimpleDemo
{
    public class NumbersProvider: ISuggestionProvider
    {
        private readonly List<string> numbers = new();

        public NumbersProvider()
        {
            for (int i = 0; i < 10000; i++)
            {
                numbers.Add(i.ToString());
            }
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return numbers.Where(x => x.Contains(filter));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoCompleteTextBox.Editors;

namespace SimpleDemo
{
    public class LongStringProvider: ISuggestionProvider
    {
        private readonly List<string> items = new();

        public LongStringProvider()
        {
            for (int i = 0; i < 10000; i++)
            {
                items.Add("abcdefghijklmnopqrstuvxwyz0123456789ABCDEFGHIJKLMNOPQRSTUVXWYX");
            }
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return items.Where(x => x.Contains(filter));
        }
    }
}
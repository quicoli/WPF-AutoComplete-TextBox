using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoCompleteTextBox.Demo.Model;
using AutoCompleteTextBox.Editors;

namespace AutoCompleteTextBox.Demo.Providers
{
    public class StateSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable<State> ListOfStates { get; set; }

        public State GetExactSuggestion(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            return
                ListOfStates
                    .FirstOrDefault(state => string.Equals(state.Name, filter, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<State> GetSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return null;
            System.Threading.Thread.Sleep(1000);
            return
                ListOfStates
                    .Where(state => state.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) > -1)
                    .ToList();
        }

        IEnumerable ISuggestionProvider.GetSuggestions(string filter)
        {
            return GetSuggestions(filter);
        }

        public StateSuggestionProvider()
        {
            var states = StateFactory.CreateStateList();
            ListOfStates = states;
        }
    }
}
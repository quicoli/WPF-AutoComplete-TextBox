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

            public IEnumerable GetSuggestions(string filter)
            {
                if (string.IsNullOrWhiteSpace(filter)) return null;
                return
                    ListOfStates
                        .Where(state => state.Name.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

            }

            public StateSuggestionProvider()
            {
                var states = StateFactory.CreateStateList();
                ListOfStates = states;
            }
        }
    
}
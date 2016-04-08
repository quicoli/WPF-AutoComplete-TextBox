using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WpfConstrols.Demo.Model;
using WpfControls.Editors;

namespace WpfConstrols.Demo.Providers
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
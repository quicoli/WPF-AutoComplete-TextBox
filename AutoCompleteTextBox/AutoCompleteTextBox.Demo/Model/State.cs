using System.Collections.Generic;
using System.Linq;

namespace AutoCompleteTextBox.Demo.Model
{
    public class State
    {
        public string Name { get; set; }
        public string Abbreviations { get; set; }
    }

    /// <summary>
    /// Return a static list of all U.S. states
    /// </summary>
    /// <returns></returns>
    public static class StateFactory
    {
        public static IList<State> CreateStateList()
        {
            List<State> states = new List<State>
            {
                new State {Abbreviations = "AL", Name = "Alabama"},
                new State {Abbreviations = "AK", Name = "Alaska"},
                new State {Abbreviations = "AR", Name = "Arkansas"},
                new State {Abbreviations = "CA", Name = "California"},
                new State {Abbreviations = "CO", Name = "Colorado"},
                new State {Abbreviations = "CT", Name = "Connecticut"},
                new State {Abbreviations = "DE", Name = "Delaware"},
                new State {Abbreviations = "FL", Name = "Florida"},
                new State {Abbreviations = "GA", Name = "Georgia"},
                new State {Abbreviations = "HI", Name = "Hawaii"},
                new State {Abbreviations = "ID", Name = "Idaho"},
                new State {Abbreviations = "IL", Name = "Illinois"},
                new State {Abbreviations = "IN", Name = "Indiana"},
                new State {Abbreviations = "IA", Name = "Iowa"},
                new State {Abbreviations = "KS", Name = "Kansas"},
                new State {Abbreviations = "KY", Name = "Kentucky"},
                new State {Abbreviations = "LA", Name = "Louisiana"},
                new State {Abbreviations = "ME", Name = "Maine"},
                new State {Abbreviations = "MD", Name = "Maryland"},
                new State {Abbreviations = "MA", Name = "Massachusetts"},
                new State {Abbreviations = "MI", Name = "Michigan"},
                new State {Abbreviations = "MN", Name = "Minnesota"},
                new State {Abbreviations = "MS", Name = "Mississippi"},
                new State {Abbreviations = "MO", Name = "Missouri"},
                new State {Abbreviations = "MT", Name = "Montana"},
                new State {Abbreviations = "NE", Name = "Nebraska"},
                new State {Abbreviations = "NV", Name = "Nevada"},
                new State {Abbreviations = "NH", Name = "New Hampshire"},
                new State {Abbreviations = "NJ", Name = "New Jersey"},
                new State {Abbreviations = "NM", Name = "New Mexico"},
                new State {Abbreviations = "NY", Name = "New York"},
                new State {Abbreviations = "NC", Name = "North Carolina"},
                new State {Abbreviations = "NV", Name = "Nevada"},
                new State {Abbreviations = "ND", Name = "North Dakota"},
                new State {Abbreviations = "OH", Name = "Ohio"},
                new State {Abbreviations = "OK", Name = "Oklahoma"},
                new State {Abbreviations = "OR", Name = "Oregon"},
                new State {Abbreviations = "PA", Name = "Pennsylvania"},
                new State {Abbreviations = "RI", Name = "Rhode Island"},
                new State {Abbreviations = "SC", Name = "South Carolina"},
                new State {Abbreviations = "SD", Name = "South Dakota"},
                new State {Abbreviations = "TN", Name = "Tennessee"},
                new State {Abbreviations = "TX", Name = "Texas"},
                new State {Abbreviations = "UT", Name = "Utah"},
                new State {Abbreviations = "VT", Name = "Vermont"},
                new State {Abbreviations = "VA", Name = "Virginia"},
                new State {Abbreviations = "WA", Name = "Washington"},
                new State {Abbreviations = "WV", Name = "West Virginia"},
                new State {Abbreviations = "WI", Name = "Wisconsin"},
                new State {Abbreviations = "WY", Name = "Wyoming"}
            };

            return states.ToList();
        }
    }
}
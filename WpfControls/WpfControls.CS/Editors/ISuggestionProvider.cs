using System.Collections;

namespace AutoCompleteTextBox.Editors
{
    public interface ISuggestionProvider
    {

        #region Public Methods

        IEnumerable GetSuggestions(string filter);

        #endregion Public Methods

    }
}

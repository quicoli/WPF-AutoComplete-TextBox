namespace WpfControls.Editors
{

    using System;
    using System.Collections;
    public class SuggestionProvider : ISuggestionProvider
    {


        #region Private Fields

        private Func<string, IEnumerable> _method;

        #endregion Private Fields

        #region Public Constructors

        public SuggestionProvider(Func<string, IEnumerable> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            _method = method;
        }

        #endregion Public Constructors

        #region Public Methods

        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            return _method(filter);
        }

        #endregion Public Methods

    }
}

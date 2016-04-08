namespace WpfControls.Editors
{

    using System.Diagnostics;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class SelectionAdapter
    {

        #region "Fields"


        private Selector _selectorControl;
        #endregion

        #region "Constructors"

        public SelectionAdapter(Selector selector)
        {
            SelectorControl = selector;
            SelectorControl.PreviewMouseUp += OnSelectorMouseDown;
        }

        #endregion

        #region "Events"

        public delegate void CancelEventHandler();

        public delegate void CommitEventHandler();

        public delegate void SelectionChangedEventHandler();

        public event CancelEventHandler Cancel;
        public event CommitEventHandler Commit;
        public event SelectionChangedEventHandler SelectionChanged;
        #endregion

        #region "Properties"

        public Selector SelectorControl
        {
            get { return _selectorControl; }
            set { _selectorControl = value; }
        }

        #endregion

        #region "Methods"

        public void HandleKeyDown(KeyEventArgs key)
        {
            Debug.WriteLine(key.Key);
            switch (key.Key)
            {
                case Key.Down:
                    IncrementSelection();
                    break;
                case Key.Up:
                    DecrementSelection();
                    break;
                case Key.Enter:
                    if (Commit != null)
                    {
                        Commit();
                    }

                    break;
                case Key.Escape:
                    if (Cancel != null)
                    {
                        Cancel();
                    }

                    break;
                case Key.Tab:
                    if (Commit != null)
                    {
                        Commit();
                    }

                    break;
            }
        }

        private void DecrementSelection()
        {
            if (SelectorControl.SelectedIndex == -1)
            {
                SelectorControl.SelectedIndex = SelectorControl.Items.Count - 1;
            }
            else
            {
                SelectorControl.SelectedIndex -= 1;
            }
            if (SelectionChanged != null)
            {
                SelectionChanged();
            }
        }

        private void IncrementSelection()
        {
            if (SelectorControl.SelectedIndex == SelectorControl.Items.Count - 1)
            {
                SelectorControl.SelectedIndex = -1;
            }
            else
            {
                SelectorControl.SelectedIndex += 1;
            }
            if (SelectionChanged != null)
            {
                SelectionChanged();
            }
        }

        private void OnSelectorMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Commit != null)
            {
                Commit();
            }
        }

        #endregion

    }

}

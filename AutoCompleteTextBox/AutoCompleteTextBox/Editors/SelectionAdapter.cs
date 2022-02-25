using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AutoCompleteTextBox.Editors
{
    public class SelectionAdapter
    {
        public class PreSelectionAdapterFinishArgs {
            public EventCause cause;
            public bool is_cancel;
            public bool handled;
        }

        #region "Fields"
        #endregion

        #region "Constructors"

        public SelectionAdapter(Selector selector)
        {
            SelectorControl = selector;
            SelectorControl.PreviewMouseUp += OnSelectorMouseDown;
        }

        #endregion

        #region "Events"

        public enum EventCause { Other, PopupClosed, ItemClicked, EnterPressed, EscapePressed, TabPressed, MouseDown}
        public delegate void CancelEventHandler(EventCause cause);

        public delegate void CommitEventHandler(EventCause cause);

        public delegate void SelectionChangedEventHandler();

        public event CancelEventHandler Cancel;
        public event CommitEventHandler Commit;
        public event SelectionChangedEventHandler SelectionChanged;
        #endregion

        #region "Properties"

        public Selector SelectorControl { get; set; }

        #endregion

        #region "Methods"

        public void HandleKeyDown(KeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.Down:
                    IncrementSelection();
                    break;
                case Key.Up:
                    DecrementSelection();
                    break;
                case Key.Enter:
                    Commit?.Invoke(EventCause.EnterPressed);

                    break;
                case Key.Escape:
                    Cancel?.Invoke(EventCause.EscapePressed);

                    break;
                case Key.Tab:
                    Commit?.Invoke(EventCause.TabPressed);

                    break;
                default:
                    return;
            }
            key.Handled = true;
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

            SelectionChanged?.Invoke();
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

            SelectionChanged?.Invoke();
        }

        private void OnSelectorMouseDown(object sender, MouseButtonEventArgs e)
        {
            // If sender is the RepeatButton from the scrollbar we need to 
            // to skip this event otherwise focus get stuck in the RepeatButton
            // and list is scrolled up or down til the end.
            if (e.OriginalSource.GetType() != typeof(RepeatButton))
            {
                Commit?.Invoke(EventCause.MouseDown);
                e.Handled = true;
            }
        }

        #endregion

    }

}
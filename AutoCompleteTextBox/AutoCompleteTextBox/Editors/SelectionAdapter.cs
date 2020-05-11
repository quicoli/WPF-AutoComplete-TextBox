using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AutoCompleteTextBox.Editors
{
    public class SelectionAdapter
    {

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

        public delegate void CancelEventHandler();

        public delegate void CommitEventHandler();

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
            //Debug.WriteLine(key.Key);
            switch (key.Key)
            {
                case Key.Down:
                    IncrementSelection();
                    break;
                case Key.Up:
                    DecrementSelection();
                    break;
                case Key.Enter:
                    Commit?.Invoke();

                    break;
                case Key.Escape:
                    Cancel?.Invoke();

                    break;
                case Key.Tab:
                    Commit?.Invoke();

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
            Commit?.Invoke();
            e.Handled = true;
        }

        #endregion

    }

}
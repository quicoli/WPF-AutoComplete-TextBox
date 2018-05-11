namespace WpfControls.Editors
{

    using System;
    using System.Collections;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    [TemplatePart(Name = AutoCompleteTextBox.PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = AutoCompleteTextBox.PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = AutoCompleteTextBox.PartSelector, Type = typeof(Selector))]
    public class AutoCompleteTextBox : Control
    {

        #region "Fields"

        public const string PartEditor = "PART_Editor";
        public const string PartPopup = "PART_Popup";

        public const string PartSelector = "PART_Selector";
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(200));
        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register("DisplayMember", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register("IconPlacement", typeof(IconPlacement), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(IconPlacement.Left));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(AutoCompleteTextBox));
        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register("LoadingContent", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(ISuggestionProvider), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(CharacterCasing.Normal));
        public static readonly DependencyProperty MaxPopUpHeightProperty = DependencyProperty.Register("MaxPopUpHeight", typeof(int), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(600));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SuggestionBackgroundProperty = DependencyProperty.Register("SuggestionBackground", typeof(Brush), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(Brushes.White));
        private BindingEvaluator _bindingEvaluator;

        private TextBox _editor;

        private DispatcherTimer _fetchTimer;

        private string _filter;

        private bool _isUpdatingText;

        private Selector _itemsSelector;

        private Popup _popup;

        private SelectionAdapter _selectionAdapter;

        private bool _selectionCancelled;

        private SuggestionsAdapter _suggestionsAdapter;

        
        #endregion

        #region "Constructors"

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        #endregion

        #region "Properties"

       
        public int MaxPopupHeight
        {
            get { return (int) GetValue(MaxPopUpHeightProperty); }
            set { SetValue(MaxPopUpHeightProperty, value);}
        }

      
        public BindingEvaluator BindingEvaluator
        {
            get { return _bindingEvaluator; }
            set { _bindingEvaluator = value; }
        }

        public CharacterCasing CharacterCasing
        {
            get { return (System.Windows.Controls.CharacterCasing) GetValue(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value);}
        }

        public int MaxLength
        {
            get { return (int) GetValue(DelayProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }

            set { SetValue(DelayProperty, value); }
        }

        public string DisplayMember
        {
            get { return (string)GetValue(DisplayMemberProperty); }

            set { SetValue(DisplayMemberProperty, value); }
        }

        public TextBox Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public DispatcherTimer FetchTimer
        {
            get { return _fetchTimer; }
            set { _fetchTimer = value; }
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public object Icon
        {
            get { return GetValue(IconProperty); }

            set { SetValue(IconProperty, value); }
        }

        public IconPlacement IconPlacement
        {
            get { return (IconPlacement)GetValue(IconPlacementProperty); }

            set { SetValue(IconPlacementProperty, value); }
        }

        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }

            set { SetValue(IconVisibilityProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }

            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }

            set { SetValue(IsLoadingProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }

            set { SetValue(IsReadOnlyProperty, value); }
        }

        public Selector ItemsSelector
        {
            get { return _itemsSelector; }
            set { _itemsSelector = value; }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }

            set { SetValue(ItemTemplateProperty, value); }
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return ((DataTemplateSelector)(GetValue(AutoCompleteTextBox.ItemTemplateSelectorProperty))); }
            set { SetValue(AutoCompleteTextBox.ItemTemplateSelectorProperty, value); }
        }

        public object LoadingContent
        {
            get { return GetValue(LoadingContentProperty); }

            set { SetValue(LoadingContentProperty, value); }
        }

        public Popup Popup
        {
            get { return _popup; }
            set { _popup = value; }
        }

        public ISuggestionProvider Provider
        {
            get { return (ISuggestionProvider)GetValue(ProviderProperty); }

            set { SetValue(ProviderProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }

            set { SetValue(SelectedItemProperty, value); }
        }

        public SelectionAdapter SelectionAdapter
        {
            get { return _selectionAdapter; }
            set { _selectionAdapter = value; }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }

            set { SetValue(WatermarkProperty, value); }
        }
        public Brush SuggestionBackground {
            get { return (Brush)GetValue(SuggestionBackgroundProperty); }

            set { SetValue(SuggestionBackgroundProperty, value); }
        }

        #endregion

        #region "Methods"

        public static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox act = null;
            act = d as AutoCompleteTextBox;
            if (act != null)
            {
                if (act.Editor != null & !act._isUpdatingText)
                {
                    act._isUpdatingText = true;
                    act.Editor.Text = act.BindingEvaluator.Evaluate(e.NewValue);
                    act._isUpdatingText = false;
                }
            }
        }

        private void ScrollToSelectedItem()
        {
            ListBox listBox = ItemsSelector as ListBox;
            if (listBox != null && listBox.SelectedItem != null)
                listBox.ScrollIntoView(listBox.SelectedItem);
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Editor = Template.FindName(PartEditor, this) as TextBox;
            Popup = Template.FindName(PartPopup, this) as Popup;
            ItemsSelector = Template.FindName(PartSelector, this) as Selector;
            BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            if (Editor != null)
            {
                Editor.TextChanged += OnEditorTextChanged;
                Editor.PreviewKeyDown += OnEditorKeyDown;
                Editor.LostFocus += OnEditorLostFocus;

                if (SelectedItem != null) 
                {
                    _isUpdatingText = true;
                    Editor.Text = BindingEvaluator.Evaluate(SelectedItem);
                    _isUpdatingText = false;
                }

            }

            this.GotFocus += AutoCompleteTextBox_GotFocus;

            if (Popup != null)
            {
                Popup.StaysOpen = false;
                Popup.Opened += OnPopupOpened;
                Popup.Closed += OnPopupClosed;
            }
            if (ItemsSelector != null)
            {
                SelectionAdapter = new SelectionAdapter(ItemsSelector);
                SelectionAdapter.Commit += OnSelectionAdapterCommit;
                SelectionAdapter.Cancel += OnSelectionAdapterCancel;
                SelectionAdapter.SelectionChanged += OnSelectionAdapterSelectionChanged;
                ItemsSelector.PreviewMouseDown += ItemsSelector_PreviewMouseDown;
            }
        }
        private void ItemsSelector_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos_item = (e.OriginalSource as FrameworkElement)?.DataContext;
            if (pos_item == null)
                return;
            if (!ItemsSelector.Items.Contains(pos_item))
                return;
            ItemsSelector.SelectedItem = pos_item;
            OnSelectionAdapterCommit();
        }
        private void AutoCompleteTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor?.Focus();
        }

        private string GetDisplayText(object dataItem)
        {
            if (BindingEvaluator == null)
            {
                BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                return dataItem.ToString();
            }
            return BindingEvaluator.Evaluate(dataItem);
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (SelectionAdapter != null)
            {
                if (IsDropDownOpen)
                    SelectionAdapter.HandleKeyDown(e);
                else
                    IsDropDownOpen = e.Key == Key.Down || e.Key == Key.Up;
            }
        }

        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                IsDropDownOpen = false;
            }
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText)
                return;
            if (FetchTimer == null)
            {
                FetchTimer = new DispatcherTimer();
                FetchTimer.Interval = TimeSpan.FromMilliseconds(Delay);
                FetchTimer.Tick += OnFetchTimerTick;
            }
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();
            SetSelectedItem(null);
            if (Editor.Text.Length > 0)
            {
                IsLoading = true;
                IsDropDownOpen = true;
                ItemsSelector.ItemsSource = null;
                FetchTimer.IsEnabled = true;
                FetchTimer.Start();
            }
            else
            {
                IsDropDownOpen = false;
            }
        }

        private void OnFetchTimerTick(object sender, EventArgs e)
        {
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();
            if (Provider != null && ItemsSelector != null)
            {
                Filter = Editor.Text;
                if (_suggestionsAdapter == null)
                {
                    _suggestionsAdapter = new SuggestionsAdapter(this);
                }
                _suggestionsAdapter.GetSuggestions(Filter);
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            if (!_selectionCancelled)
            {
                OnSelectionAdapterCommit();
            }
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            _selectionCancelled = false;
            ItemsSelector.SelectedItem = SelectedItem;
        }

        private void OnSelectionAdapterCancel()
        {
            _isUpdatingText = true;
            Editor.Text = SelectedItem == null ? Filter : GetDisplayText(SelectedItem);
            Editor.SelectionStart = Editor.Text.Length;
            Editor.SelectionLength = 0;
            _isUpdatingText = false;
            IsDropDownOpen = false;
            _selectionCancelled = true;
        }

        private void OnSelectionAdapterCommit()
        {
            if (ItemsSelector.SelectedItem != null)
            {
                SelectedItem = ItemsSelector.SelectedItem;
                _isUpdatingText = true;
                Editor.Text = GetDisplayText(ItemsSelector.SelectedItem);
                SetSelectedItem(ItemsSelector.SelectedItem);
                _isUpdatingText = false;
                IsDropDownOpen = false;
            }
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            _isUpdatingText = true;
            if (ItemsSelector.SelectedItem == null)
            {
                Editor.Text = Filter;
            }
            else
            {
                Editor.Text = GetDisplayText(ItemsSelector.SelectedItem);
            }
            Editor.SelectionStart = Editor.Text.Length;
            Editor.SelectionLength = 0;
            ScrollToSelectedItem();
            _isUpdatingText = false;
        }

        private void SetSelectedItem(object item)
        {
            _isUpdatingText = true;
            SelectedItem = item;
            _isUpdatingText = false;
        }
        #endregion

        #region "Nested Types"

        private class SuggestionsAdapter
        {

            #region "Fields"

            private AutoCompleteTextBox _actb;

            private string _filter;
            #endregion

            #region "Constructors"

            public SuggestionsAdapter(AutoCompleteTextBox actb)
            {
                _actb = actb;
            }

            #endregion

            #region "Methods"

            public void GetSuggestions(string searchText)
            {
                _filter = searchText;
                _actb.IsLoading = true;
                ParameterizedThreadStart thInfo = new ParameterizedThreadStart(GetSuggestionsAsync);
                Thread th = new Thread(thInfo);
                th.Start(new object[] {
                searchText,
                _actb.Provider
            });
            }

            private void DisplaySuggestions(IEnumerable suggestions, string filter)
            {
                if (_filter != filter)
                {
                    return;
                }
                if (_actb.IsDropDownOpen)
                {
                    _actb.IsLoading = false;
                    _actb.ItemsSelector.ItemsSource = suggestions;
                    _actb.IsDropDownOpen = _actb.ItemsSelector.HasItems;
                }

            }

            private void GetSuggestionsAsync(object param)
            {
                object[] args = param as object[];
                string searchText = Convert.ToString(args[0]);
                ISuggestionProvider provider = args[1] as ISuggestionProvider;
                IEnumerable list = provider.GetSuggestions(searchText);
                _actb.Dispatcher.BeginInvoke(new Action<IEnumerable, string>(DisplaySuggestions), DispatcherPriority.Background, new object[] {
                list,
                searchText
            });
            }

            #endregion

        }

        #endregion

    }

}

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

namespace AutoCompleteTextBox.Editors
{
    [TemplatePart(Name = PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartSelector, Type = typeof(Selector))]
    [TemplatePart(Name = PartExpander, Type = typeof(Expander))]
    public class AutoCompleteComboBox : Control
    {

        #region "Fields"

        public const string PartEditor = "PART_Editor";
        public const string PartPopup = "PART_Popup";

        public const string PartSelector = "PART_Selector";
        public const string PartExpander = "PART_Expander";
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(200));
        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register("DisplayMember", typeof(string), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register("IconPlacement", typeof(IconPlacement), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(IconPlacement.Left));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(AutoCompleteComboBox));
        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register("LoadingContent", typeof(object), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(IComboSuggestionProvider), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(string.Empty, propertyChangedCallback: null, coerceValueCallback: null, isAnimationProhibited: false, defaultUpdateSourceTrigger: UpdateSourceTrigger.LostFocus, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(CharacterCasing.Normal));
        public static readonly DependencyProperty MaxPopUpHeightProperty = DependencyProperty.Register("MaxPopUpHeight", typeof(int), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(600));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SuggestionBackgroundProperty = DependencyProperty.Register("SuggestionBackground", typeof(Brush), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(Brushes.White));
        private bool _isUpdatingText;
        private bool _selectionCancelled;

        private SuggestionsAdapter _suggestionsAdapter;


        #endregion

        #region "Constructors"

        static AutoCompleteComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(typeof(AutoCompleteComboBox)));
        }

        #endregion

        #region "Properties"


        public int MaxPopupHeight
        {
            get => (int)GetValue(MaxPopUpHeightProperty);
            set => SetValue(MaxPopUpHeightProperty, value);
        }


        public BindingEvaluator BindingEvaluator { get; set; }

        public CharacterCasing CharacterCasing
        {
            get => (CharacterCasing)GetValue(CharacterCasingProperty);
            set => SetValue(CharacterCasingProperty, value);
        }

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public int Delay
        {
            get => (int)GetValue(DelayProperty);

            set => SetValue(DelayProperty, value);
        }

        public string DisplayMember
        {
            get => (string)GetValue(DisplayMemberProperty);

            set => SetValue(DisplayMemberProperty, value);
        }

        public TextBox Editor { get; set; }
        public Expander Expander { get; set; }

        public DispatcherTimer FetchTimer { get; set; }

        public string Filter
        {
            get => (string)GetValue(FilterProperty);

            set => SetValue(FilterProperty, value);
        }

        public object Icon
        {
            get => GetValue(IconProperty);

            set => SetValue(IconProperty, value);
        }

        public IconPlacement IconPlacement
        {
            get => (IconPlacement)GetValue(IconPlacementProperty);

            set => SetValue(IconPlacementProperty, value);
        }

        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);

            set => SetValue(IconVisibilityProperty, value);
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);

            set
            {
                this.Expander.IsExpanded = value;
                SetValue(IsDropDownOpenProperty, value);
            }
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);

            set => SetValue(IsLoadingProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);

            set => SetValue(IsReadOnlyProperty, value);
        }

        public Selector ItemsSelector { get; set; }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);

            set => SetValue(ItemTemplateProperty, value);
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get => ((DataTemplateSelector)(GetValue(ItemTemplateSelectorProperty)));
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        public object LoadingContent
        {
            get => GetValue(LoadingContentProperty);

            set => SetValue(LoadingContentProperty, value);
        }

        public Popup Popup { get; set; }

        public IComboSuggestionProvider Provider
        {
            get => (IComboSuggestionProvider)GetValue(ProviderProperty);

            set => SetValue(ProviderProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);

            set => SetValue(SelectedItemProperty, value);
        }

        public SelectionAdapter SelectionAdapter { get; set; }

        public string Text
        {
            get => (string)GetValue(TextProperty);

            set => SetValue(TextProperty, value);
        }

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);

            set => SetValue(WatermarkProperty, value);
        }
        public Brush SuggestionBackground
        {
            get => (Brush)GetValue(SuggestionBackgroundProperty);

            set => SetValue(SuggestionBackgroundProperty, value);
        }

        #endregion

        #region "Methods"

        public static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteComboBox act = null;
            act = d as AutoCompleteComboBox;
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
            if (ItemsSelector is ListBox listBox && listBox.SelectedItem != null)
                listBox.ScrollIntoView(listBox.SelectedItem);
        }

        public new BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding){
            var res = base.SetBinding(dp, binding);
            CheckForParentTextBindingChange();
            return res;
        }
        public new BindingExpressionBase SetBinding(DependencyProperty dp, String  path) {
            var res = base.SetBinding(dp, path);
            CheckForParentTextBindingChange();
            return res;
        }
        public new void ClearValue(DependencyPropertyKey key) {
            base.ClearValue(key);
            CheckForParentTextBindingChange();
        }
        public new void ClearValue(DependencyProperty dp) {
            base.ClearValue(dp);
            CheckForParentTextBindingChange();
        }
        private void CheckForParentTextBindingChange(bool force=false) {
            var CurrentBindingMode = BindingOperations.GetBinding(this, TextProperty)?.UpdateSourceTrigger ?? UpdateSourceTrigger.Default;
            if (CurrentBindingMode != UpdateSourceTrigger.PropertyChanged)//preventing going any less frequent than property changed
                CurrentBindingMode = UpdateSourceTrigger.Default;


            if (CurrentBindingMode == CurrentTextboxTextBindingUpdateMode && force == false)
                return;
            var binding = new Binding {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = CurrentBindingMode,
                Path = new PropertyPath(nameof(Text)),
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
            };
            CurrentTextboxTextBindingUpdateMode = CurrentBindingMode;
            Editor?.SetBinding(TextBox.TextProperty, binding);
        }

        private UpdateSourceTrigger CurrentTextboxTextBindingUpdateMode;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Editor = Template.FindName(PartEditor, this) as TextBox;
            Popup = Template.FindName(PartPopup, this) as Popup;
            ItemsSelector = Template.FindName(PartSelector, this) as Selector;
            Expander = Template.FindName(PartExpander, this) as Expander;

            BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            if (Editor != null)
            {
                Editor.TextChanged += OnEditorTextChanged;
                Editor.PreviewKeyDown += OnEditorKeyDown;
                Editor.LostFocus += OnEditorLostFocus;
                CheckForParentTextBindingChange(true);

                if (SelectedItem != null)
                {
                    _isUpdatingText = true;
                    Editor.Text = BindingEvaluator.Evaluate(SelectedItem);
                    _isUpdatingText = false;
                }

            }
            if (Expander != null)
            {
                Expander.IsExpanded = false;
                Expander.Collapsed += Expander_Expanded;
                Expander.Expanded += Expander_Expanded;
            }

            GotFocus += AutoCompleteComboBox_GotFocus;

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

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = Expander.IsExpanded;
            if (!this.IsDropDownOpen)
            {
                return;
            }
            if (_suggestionsAdapter == null)
            {
                _suggestionsAdapter = new SuggestionsAdapter(this);
            }
            _suggestionsAdapter.ShowFullCollection();

        }

        private void ItemsSelector_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement)?.DataContext == null)
                return;
            if (!ItemsSelector.Items.Contains(((FrameworkElement)e.OriginalSource)?.DataContext))
                return;
            ItemsSelector.SelectedItem = ((FrameworkElement)e.OriginalSource)?.DataContext;
            OnSelectionAdapterCommit(SelectionAdapter.EventCause.ItemClicked);
            e.Handled = true;
        }
        private void AutoCompleteComboBox_GotFocus(object sender, RoutedEventArgs e)
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
            Text = Editor.Text;
            if (_isUpdatingText)
                return;
            if (FetchTimer == null)
            {
                FetchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(Delay) };
                FetchTimer.Tick += OnFetchTimerTick;
            }
            FetchTimer.IsEnabled = false;
            FetchTimer.Stop();
            SetSelectedItem(null);
            if (Editor.Text.Length > 0)
            {
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
                OnSelectionAdapterCommit(SelectionAdapter.EventCause.PopupClosed);
            }
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            _selectionCancelled = false;
            ItemsSelector.SelectedItem = SelectedItem;
        }

        public event EventHandler<SelectionAdapter.PreSelectionAdapterFinishArgs> PreSelectionAdapterFinish;
        private bool PreSelectionEventSomeoneHandled(SelectionAdapter.EventCause cause, bool is_cancel) {
            if (PreSelectionAdapterFinish == null)
                return false;
            var args = new SelectionAdapter.PreSelectionAdapterFinishArgs { cause = cause, is_cancel = is_cancel };
            PreSelectionAdapterFinish?.Invoke(this, args);
            return args.handled;

        }
        private void OnSelectionAdapterCancel(SelectionAdapter.EventCause cause)
        {
            if (PreSelectionEventSomeoneHandled(cause, true))
                return;
            _isUpdatingText = true;
            Editor.Text = SelectedItem == null ? Filter : GetDisplayText(SelectedItem);
            Editor.SelectionStart = Editor.Text.Length;
            Editor.SelectionLength = 0;
            _isUpdatingText = false;
            IsDropDownOpen = false;
            _selectionCancelled = true;
        }

        private void OnSelectionAdapterCommit(SelectionAdapter.EventCause cause)
        {
            if (PreSelectionEventSomeoneHandled(cause, false))
                return;

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
            Editor.Text = ItemsSelector.SelectedItem == null ? Filter : GetDisplayText(ItemsSelector.SelectedItem);
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

            private readonly AutoCompleteComboBox _actb;

            private string _filter;
            #endregion

            #region "Constructors"

            public SuggestionsAdapter(AutoCompleteComboBox actb)
            {
                _actb = actb;
            }

            #endregion

            #region "Methods"

            public void GetSuggestions(string searchText)
            {
                _filter = searchText;
                _actb.IsLoading = true;
                // Do not open drop down if control is not focused
                if (_actb.IsKeyboardFocusWithin)
                    _actb.IsDropDownOpen = true;
                _actb.ItemsSelector.ItemsSource = null;
                ParameterizedThreadStart thInfo = GetSuggestionsAsync;
                Thread th = new Thread(thInfo);
                th.Start(new object[] { searchText, _actb.Provider });
            }
            public void ShowFullCollection()
            {
                _filter = string.Empty;
                _actb.IsLoading = true;
                // Do not open drop down if control is not focused
                if (_actb.IsKeyboardFocusWithin)
                    _actb.IsDropDownOpen = true;
                _actb.ItemsSelector.ItemsSource = null;
                ParameterizedThreadStart thInfo = GetFullCollectionAsync;
                Thread th = new Thread(thInfo);
                th.Start(_actb.Provider);
            }

            private void DisplaySuggestions(IEnumerable suggestions, string filter)
            {
                if (_filter != filter)
                {
                    return;
                }
                _actb.IsLoading = false;
                _actb.ItemsSelector.ItemsSource = suggestions;
                // Close drop down if there are no items
                if (_actb.IsDropDownOpen)
                {
                    _actb.IsDropDownOpen = _actb.ItemsSelector.HasItems;
                }
            }

            private void GetSuggestionsAsync(object param)
            {
                if (param is object[] args)
                {
                    string searchText = Convert.ToString(args[0]);
                    if (args[1] is IComboSuggestionProvider provider)
                    {
                        IEnumerable list = provider.GetSuggestions(searchText);
                        _actb.Dispatcher.BeginInvoke(new Action<IEnumerable, string>(DisplaySuggestions), DispatcherPriority.Background, list, searchText);
                    }
                }
            }
            private void GetFullCollectionAsync(object param)
            {
                if (param is IComboSuggestionProvider provider)
                {
                    IEnumerable list = provider.GetFullCollection();
                    _actb.Dispatcher.BeginInvoke(new Action<IEnumerable, string>(DisplaySuggestions), DispatcherPriority.Background, list, string.Empty);
                }

            }

            #endregion

        }

        #endregion

    }

}

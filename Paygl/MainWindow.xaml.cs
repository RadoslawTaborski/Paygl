using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Paygl.Views;
using System.Windows.Markup;
using PayglService.cs;
using System.Globalization;
using System.Threading;

namespace Paygl
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region FIELDS

        public const int MenuButtonHeight = 50;
        public const int ViewbarButtonWidth = 120;

        private static MyButton _btnView;
        private static UserControl _selectedView;
        private static UserControl _previousView;
        private List<UserControl> _views;
        private static MyButton _btnAnalyse;

        private static ItemsControl _icOperationsButtons;
        private static MyButton _btnImport;
        private static MyButton _btnAddManually;
        private static MyButton _btnAddGroups;

        private static ItemsControl _icAnalyseButtons;
        private static MyButton _btnShowOperations;
        private static MyButton _btnAnalysis;
        private static MyButton _btnFilters;
        private static MyButton _btnAnalysisManager;

        private static MyButton _btnSettings;

        #endregion

        #region CONSTRUCTORS
        public MainWindow()
        {
            InitializeComponent();
            ConfigurationManager.ReadConfig("./configuration.json");
            Thread.CurrentThread.CurrentUICulture = Service.ReadLanguage();
            ViewManager.SetMainWindow(this);
        }
        #endregion

        #region EVENTS
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _views = new List<UserControl>();
                SecondMenu.Visibility = Visibility.Hidden;

                Service.SetService();
                var menuButtons = new List<MyButton>();
                _btnView = CreateButton("btnOperations", Properties.strings._btnTransactions, MenuButtonHeight, btnOperations_Click);
                _btnAnalyse = CreateButton("btnAnalyse", Properties.strings._btnAnalyse, MenuButtonHeight, btnAnalyse_Click);
                _btnSettings = CreateButton("btnSettings", Properties.strings._btnSettings, MenuButtonHeight, btnSettings_Click);
                menuButtons.Add(_btnView);
                menuButtons.Add(_btnAnalyse);
                menuButtons.Add(_btnSettings);
                _firstPanel.ItemsSource = menuButtons;

                _icOperationsButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b1Buttons = new List<MyButton>();
                _btnImport = CreateButton("btnImport", Properties.strings._btnImport, MenuButtonHeight, btnImport_Click);
                _btnAddManually = CreateButton("btnAddManually", Properties.strings._btnAddManualy, MenuButtonHeight, BtnAddManually_Click);
                _btnAddGroups = CreateButton("btnAddGroups", Properties.strings._btnAddGroup, MenuButtonHeight, BtnAddGroups_Click);
                b1Buttons.Add(_btnImport);
                b1Buttons.Add(_btnAddManually);
                b1Buttons.Add(_btnAddGroups);
                b1Buttons.Add(_btnShowOperations);
                if (_icOperationsButtons != null) _icOperationsButtons.ItemsSource = b1Buttons;

                _icAnalyseButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b2Buttons = new List<MyButton>();
                _btnFilters = CreateButton("btnFilters", Properties.strings._btnFilters, MenuButtonHeight, btnFilters_Click);
                _btnAnalysisManager= CreateButton("btnAnalysisManager", Properties.strings._btnAnalysisManager, MenuButtonHeight, btnAnalysisManager_Click);
                _btnAnalysis = CreateButton("btnAnalysis", Properties.strings._btnAnalysis, MenuButtonHeight, btnAnalysis_Click);
                _btnShowOperations = CreateButton("btnShowOperations", Properties.strings._btnSearching, MenuButtonHeight, BtnShowOperations_Click);
                b2Buttons.Add(_btnShowOperations);
                b2Buttons.Add(_btnAnalysis);
                b2Buttons.Add(_btnFilters);
                b2Buttons.Add(_btnAnalysisManager);
                if (_icAnalyseButtons != null) _icAnalyseButtons.ItemsSource = b2Buttons;
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, ex.Message);
                dialog.ShowDialog();
            }
        }

        #region menuItems
        private void btnOperations_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideSecondMenu(_icOperationsButtons);
            SetSecondMenu(_icOperationsButtons);
        }

        private void btnAnalyse_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideSecondMenu(_icAnalyseButtons);
            SetSecondMenu(_icAnalyseButtons);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new SettingsView());
        }
        #endregion

        #region OperationItems
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ImportOperationsView());
        }

        private void BtnAddManually_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ManuallyOperationsView());
        }

        private void BtnAddGroups_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new AddGroupsView());
        }
        #endregion

        #region AnalyseItems
        private void btnFilters_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new FiltersManager());
        }

        private void btnAnalysis_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new AnalysisView());
        }

        private void BtnShowOperations_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ShowOperations());
        }

        private void btnAnalysisManager_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new AnalysisManager());
        }
        #endregion

        #region TITLE_BAR
        /// <summary>
        /// Handles the MouseDown event of the TitleBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ClickCount == 2)
            {
                AdjustWindowSize();
            }
            else
            {
                if (Application.Current.MainWindow != null) Application.Current.MainWindow.DragMove();
            }
        }

        /// <summary>
        /// Adjusts the size of the window.
        /// </summary>
        private void AdjustWindowSize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                var resourceUri = new Uri("img/max.png", UriKind.Relative);
                var streamInfo = Application.GetResourceStream(resourceUri);

                if (streamInfo == null) return;
                var temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush
                {
                    ImageSource = temp
                };
                MaxButton.Background = brush;
            }
            else
            {
                WindowState = WindowState.Maximized;
                var resourceUri = new Uri("img/min.png", UriKind.Relative);
                var streamInfo = Application.GetResourceStream(resourceUri);

                if (streamInfo == null) return;
                var temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush
                {
                    ImageSource = temp
                };
                MaxButton.Background = brush;
            }
        }

        /// <summary>
        /// Handles the Click event of the MinButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Handles the Click event of the MaxButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #endregion

        #region private
        private void SetSecondMenu(ItemsControl itemsControl)
        {
            _secondStockPanel.Children.Clear();
            _secondStockPanel.Children.Add(itemsControl);
        }

        private void ShowOrHideSecondMenu(ItemsControl itemsControl)
        {
            if (_secondStockPanel.Children.Contains(itemsControl))
            {
                SecondMenu.Visibility = SecondMenu.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                SecondMenu.Visibility = Visibility.Visible;
            }
        }

        private void CreateAndOpenNewView(UserControl view)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            AddUserControl(view);
            OpenUserControl(view);
        }

        private MyButton CreateButton(string name, string content, int height, RoutedEventHandler operation)
        {
            var button = new MyButton
            {
                Name = name,
                Content = content,
            };
            button.Click += operation;
            button.Height = height;
            button.Style = (Style)FindResource("MyButton");

            return button;
        }

        private ButtonWithObject CreateViewBarButton(string name, string content, int width, object insideObject, bool isSelected, RoutedEventHandler operation)
        {
            var brush = (Brush)FindResource("MyAzure");
            var style = (Style)FindResource("MyButton");
            if (isSelected)
            {
                brush = (Brush)FindResource("MyWhite");
                style = (Style)FindResource("MyButtonBarView");
            }

            var columnDefinition1 = new ColumnDefinition()
            {
                Width = new GridLength((width - 30 < 0)?0:width-30),
            };
            var columnDefinition2 = new ColumnDefinition()
            {
                Width = new GridLength(20),
            };
            var grid = new Grid
            {
                Width = (width - 10 < 0) ? 0 : width - 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush)FindResource("Transparent"),
                ColumnDefinitions =
                {
                    columnDefinition1,
                    columnDefinition2
                },
            };

            var label = new Label
            {
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Center,               
                Background = (Brush)FindResource("Transparent"),
                Foreground = brush,
            };
            var closeButton = CreateCloseButton(insideObject);
            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
            label.SetValue(Grid.ColumnProperty, 0);
            closeButton.SetValue(Grid.ColumnProperty, 1);
            grid.Children.Add(label);
            grid.Children.Add(closeButton);

            var button = new ButtonWithObject
            {
                Name = name,
                Content = grid,
                Object = insideObject,
                Context = label,
            };
            button.Click += operation;
            if (!isSelected)
            {
                button.MouseEnter += MouseEnter_Event;
                button.MouseLeave += MouseLeave_Event;
            }
            button.Width = width;
            button.Style = style;

            return button;
        }

        private void MouseLeave_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            ((Label) button.Context).Foreground = (Brush)FindResource("MyAzure");
        }

        private void MouseEnter_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            ((Label) button.Context).Foreground = (Brush)FindResource("MyDarkGrey");
        }

        public void AddUserControl(UserControl uc)
        {
            if (_views.Any(u => (u as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName))
            {
                return;
            }
            _views.Add(uc);
        }

        public void OpenUserControl(UserControl uc)
        {
            _previousView = _selectedView;
            _selectedView = _views.FirstOrDefault(u => (u as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            brdMain.Child = _selectedView;
            UpdateViewBar(_selectedView);
        }

        public void RemoveUserControl(UserControl uc)
        {
            var userControl = _views.First(u => (u as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            _views.Remove(userControl);
        }

        private void UpdateViewBar(UserControl selected)
        {
            _viewBarStockPanel.Children.Clear();
            var barWidth = _viewBarStockPanel.ActualWidth;
            var defaultButtonWidth = ViewbarButtonWidth;
            if (_views.Count * defaultButtonWidth > barWidth)
            {
                defaultButtonWidth = (int) barWidth / _views.Count;
            }

            foreach (var item in _views)
            {
                var tmp = CreateViewBarButton($"btn{item}", (item as IRepresentative)?.RepresentativeName, defaultButtonWidth, item, (item as IRepresentative)?.RepresentativeName == (selected as IRepresentative)?.RepresentativeName, BtnView_Click);
                _viewBarStockPanel.Children.Add(tmp);
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject)?.Object as UserControl;
            OpenUserControl(uc);
        }

        #endregion

        private ButtonWithObject CreateCloseButton(object uc)
        {
            var newButton = new ButtonWithObject
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\x-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Center
                },
                Object = uc,
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Style = (Style) FindResource("MyButton"),
            };
            newButton.Click += RemoveFromBarView_Click;

            return newButton;
        }

        private void RemoveFromBarView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject)?.Object as UserControl;
            RemoveFromBarView(uc);

            e.Handled = true;
        }

        public void RemoveFromBarView(UserControl uc)
        {
            RemoveUserControl(uc);
            if ((_selectedView as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName)
            {
                OpenUserControl(_previousView);
            }
            if ((_previousView as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName)
            {
                _previousView = _views.Any() ? _views[0] : null;
            }
            UpdateViewBar(_selectedView);
        }

        private void SecondMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            SecondMenu.Visibility = Visibility.Hidden;
        }
    }
}
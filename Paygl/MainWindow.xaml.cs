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

        public static readonly string Path = "settings.dat";
        public const int MENU_BUTTON_HEIGHT = 50;
        public const int VIEWBAR_BUTTON_WIDTH = 120;

        private static Button _btnView;
        private static UserControl _selectedView;
        private static UserControl _previousView;
        private List<UserControl> _views;
        private static Button _btnAnalyse;

        private static ItemsControl _icOperationsButtons;
        private static Button _btnImport;
        private static Button _btnAddManually;
        private static Button _btnAddGroups;

        private static ItemsControl _icAnalyseButtons;
        private static Button _btnShowOperations;
        private static Button _btnAnalysis;
        private static Button _btnFilters;
        private static Button _btnAnalysisManager;

        #endregion

        #region CONSTRUCTORS
        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl-PL");
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

                ConfigurationManager.ReadConfig("./configuration.json");
                Service.SetService();
                var menuButtons = new List<Button>();
                _btnView = CreateButton("btnOperations", Properties.strings._btnTransactions, MENU_BUTTON_HEIGHT, btnOperations_Click);
                _btnAnalyse = CreateButton("btnAnalyse", "Analiza", MENU_BUTTON_HEIGHT, btnAnalyse_Click);
                menuButtons.Add(_btnView);
                menuButtons.Add(_btnAnalyse);
                _firstPanel.ItemsSource = menuButtons;

                _icOperationsButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b1Buttons = new List<Button>();
                _btnImport = CreateButton("btnImport", "Importuj", MENU_BUTTON_HEIGHT, btnImport_Click);
                _btnAddManually = CreateButton("btnAddManually", "Dodaj manualnie", MENU_BUTTON_HEIGHT, BtnAddManually_Click);
                _btnAddGroups = CreateButton("btnAddGroups", "Dodaj grupę", MENU_BUTTON_HEIGHT, BtnAddGroups_Click);
                b1Buttons.Add(_btnImport);
                b1Buttons.Add(_btnAddManually);
                b1Buttons.Add(_btnAddGroups);
                b1Buttons.Add(_btnShowOperations);
                if (_icOperationsButtons != null) _icOperationsButtons.ItemsSource = b1Buttons;

                _icAnalyseButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b2Buttons = new List<Button>();
                _btnFilters = CreateButton("btnFilters", "Filtry", MENU_BUTTON_HEIGHT, btnFilters_Click);
                _btnAnalysisManager= CreateButton("btnAnalysisManager", "Menadżer Analizy", MENU_BUTTON_HEIGHT, btnAnalysisManager_Click);
                _btnAnalysis = CreateButton("btnAnalysis", "Analiza", MENU_BUTTON_HEIGHT, btnAnalysis_Click);
                _btnShowOperations = CreateButton("btnShowOperations", "Wyszukiwanie", MENU_BUTTON_HEIGHT, BtnShowOperations_Click);
                b2Buttons.Add(_btnShowOperations);
                b2Buttons.Add(_btnAnalysis);
                b2Buttons.Add(_btnFilters);
                b2Buttons.Add(_btnAnalysisManager);
                if (_icAnalyseButtons != null) _icAnalyseButtons.ItemsSource = b2Buttons;
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox("Komunikat", ex.Message);
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
        /// Handles the Click event of the MenuButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var subWindow = new Settings();
            subWindow.ShowDialog();

            if (!subWindow.ChangeSettings) return;

            //TODO: do something
        }

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
                if (SecondMenu.Visibility == Visibility.Hidden)
                {
                    SecondMenu.Visibility = Visibility.Visible;
                }
                else
                {
                    SecondMenu.Visibility = Visibility.Hidden;
                }

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

        private Button CreateButton(string name, string content, int height, RoutedEventHandler operation)
        {
            var button = new Button
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
                Width = new GridLength(VIEWBAR_BUTTON_WIDTH - 30),
            };
            var columnDefinition2 = new ColumnDefinition()
            {
                Width = new GridLength(20),
            };
            var grid = new Grid
            {
                Width = VIEWBAR_BUTTON_WIDTH - 10,
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

            foreach (var item in _views)
            {
                _btnView = CreateViewBarButton($"btn{item}", (item as IRepresentative)?.RepresentativeName, VIEWBAR_BUTTON_WIDTH, item, (item as IRepresentative)?.RepresentativeName == (selected as IRepresentative)?.RepresentativeName, BtnView_Click);
                _viewBarStockPanel.Children.Add(_btnView);
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
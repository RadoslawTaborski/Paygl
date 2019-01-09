using Paygl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Paygl.Views;
using DataAccess;
using DataBaseWithBusinessLogicConnector;
using DataBaseWithBusinessLogicConnector.Dal.Adapters;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Dal.Mappers;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using PayglService.cs;

namespace Paygl
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region FIELDS

        public static readonly string Path = "settings.dat";
        public const int MENU_BUTTON_HEIGHT = 50;

        private static Button _btnOperations;
        private static Button _btnAnalyse;

        private static ItemsControl _icOperationsButtons;
        private static Button _btnImport;
        private static Button _btnAddManually;
        private static Button _btnAddGroups;
        private static Button _btnShowOperations;

        private static ItemsControl _icAnalyseButtons;
        #endregion

        #region CONSTRUCTORS
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region EVENTS
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SecondMenu.Visibility = Visibility.Hidden;

                ConfigurationManager.ReadConfig("./configuration.json");
                Service.SetService();
                var menuButtons = new List<Button>();
                _btnOperations = CreateButton("btnOperations", "Operacje", MENU_BUTTON_HEIGHT, btnOperations_Click);
                _btnAnalyse = CreateButton("btnAnalyse", "Analiza", MENU_BUTTON_HEIGHT, btnAnalyse_Click);
                menuButtons.Add(_btnOperations);
                menuButtons.Add(_btnAnalyse);
                _firstPanel.ItemsSource = menuButtons;

                _icOperationsButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b1Buttons = new List<Button>();
                _btnImport = CreateButton("btnImport", "Importuj", MENU_BUTTON_HEIGHT, btnImport_Click);
                _btnAddManually = CreateButton("btnAddManually", "Dodaj manualnie", MENU_BUTTON_HEIGHT, BtnAddManually_Click);
                _btnAddGroups = CreateButton("btnAddGroups", "Dodaj grupę", MENU_BUTTON_HEIGHT, BtnAddGroups_Click);
                _btnShowOperations = CreateButton("btnShowOperations", "Pokaż", MENU_BUTTON_HEIGHT, BtnShowOperations_Click);
                b1Buttons.Add(_btnImport);
                b1Buttons.Add(_btnAddManually);
                b1Buttons.Add(_btnAddGroups);
                b1Buttons.Add(_btnShowOperations);
                _icOperationsButtons.ItemsSource = b1Buttons;

                _icAnalyseButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b2Buttons = new List<Button>();
                _icAnalyseButtons.ItemsSource = b2Buttons;
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
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            brdMain.Child = new ImportOperationsView();
        }

        private void BtnAddManually_Click(object sender, RoutedEventArgs e)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            brdMain.Child = new ManuallyOperationsView();
        }

        private void BtnAddGroups_Click(object sender, RoutedEventArgs e)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            brdMain.Child = new AddGroupsView();
        }

        private void BtnShowOperations_Click(object sender, RoutedEventArgs e)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            brdMain.Child = new ShowOperations();
        }
        #endregion

        #region AnalyseItems
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

        private Button CreateButton(string name, string content, int height, RoutedEventHandler operation)
        {
            var button = new Button
            {
                Name = name,
                Content = content
            };
            button.Click += operation;
            button.Height = height;
            button.Style = (Style)FindResource("MyButton");

            return button;
        }
        #endregion
    }
}
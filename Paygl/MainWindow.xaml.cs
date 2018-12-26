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

        private static Button _b1;
        private static Button _b2;

        private static ItemsControl _b1Items;
        private static Button _b1_1;
        private static Button _b1_2;
        private static Button _b1_3;

        private static ItemsControl _b2Items;
        #endregion

        #region CONSTRUCTORS
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region EVENTS

        #region menuItems
        private void btnOperations_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideSecondMenu(_b1Items);
            SetSecondMenu(_b1Items);
        }

        private void btnAnalyse_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideSecondMenu(_b2Items);
            SetSecondMenu(_b2Items);
        }
        #endregion

        #region OperationItems
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            brdMain.Child = new ImportView();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            brdMain.Child = new ManuallyAddView();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SecondMenu.Visibility = Visibility.Hidden;

                ConfigurationManager.ReadConfig("./configuration.json");
                Service.SetService();
                var menuButtons = new List<Button>();
                _b1 = CreateButton("btnOperations", "Operacje", MENU_BUTTON_HEIGHT, btnOperations_Click);
                _b2 = CreateButton("btnAnalyse", "Analiza", MENU_BUTTON_HEIGHT, btnAnalyse_Click);
                menuButtons.Add(_b1);
                menuButtons.Add(_b2);
                _firstPanel.ItemsSource = menuButtons;

                _b1Items = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b1Buttons = new List<Button>();
                _b1_1 = CreateButton("btnImport", "Importuj", MENU_BUTTON_HEIGHT, btnImport_Click);
                _b1_2 = CreateButton("btnAdd", "Dodaj manualnie", MENU_BUTTON_HEIGHT, btnAdd_Click);
                _b1_3 = CreateButton("btnShow", "Przeglądaj", MENU_BUTTON_HEIGHT, btnShow_Click);
                b1Buttons.Add(_b1_1);
                b1Buttons.Add(_b1_2);
                b1Buttons.Add(_b1_3);
                _b1Items.ItemsSource = b1Buttons;

                _b2Items = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b2Buttons = new List<Button>();
                _b2Items.ItemsSource = b2Buttons;
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox("Komunikat", ex.Message);
                dialog.ShowDialog();
            }
        }

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
            var button = new Button();
            button.Name = name;
            button.Content = content;
            button.Click += operation;
            button.Height = height;
            button.Style = (Style)FindResource("MyButton");

            return button;
        }
        #endregion
    }
}
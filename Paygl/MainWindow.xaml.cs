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

namespace Paygl
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        #region FIELDS

        public static readonly string Path = "settings.dat";

        private static Button _b1 = new Button();
        private static Button _b2 = new Button();
        private static Button _b3 = new Button();

        #endregion

        #region CONSTRUCTORS
        public MainWindow()
        {
            InitializeComponent();

            var buttons = new List<Button>();

            _b1.Name = "btn1";
            _b1.Content = "btn1";
            _b1.Click += _b1_Click;
            buttons.Add(_b1);

            _b2.Name = "btn2";
            _b2.Content = "btn2";
            _b2.Click += _b2_Click;
            buttons.Add(_b2);

            _b3.Name = "btn3";
            _b3.Content = "btn3";
            _b3.Click += _b3_Click;
            buttons.Add(_b3);

            ic.ItemsSource = buttons;
        }
        #endregion


        private void test()
        {
            var dbManager = new DatabaseManager(new MySqlConnectionFactory(), "localhost", "paygl", "root", "");
            var dbConnector = new DbConnector(dbManager);
            var languageAdapter = new LanguageAdapter(dbConnector);
            var userAdapter = new UserAdapter(dbConnector);
            var userDetailsAdapter = new UserDetailsAdapter(dbConnector);
            var transactionTypeAdapter = new TransactionTypeAdapter(dbConnector);
            var transferTypeAdapter = new TransferTypeAdapter(dbConnector);
            var frequenceAdapter = new FrequenceAdapter(dbConnector);
            var importanceAdapter = new ImportanceAdapter(dbConnector);
            var tagAdapter = new TagAdapter(dbConnector);
            var operationAdapter = new OperationAdapter(dbConnector);
            var operationDetailsAdapter = new OperationDetailsAdapter(dbConnector);
            var operationTagRelationAdapter = new OperationTagAdapter(dbConnector);

            var languages = new LanguageMapper().ConvertToBusinessLogicEntitiesCollection(languageAdapter.GetAll());
            var language = languages.Where(l => l.ShortName == "pl-PL").First();

            var dalUser = userAdapter.GetById(1);
            var user = new UserMapper().ConvertToBusinessLogicEntity(dalUser);
            user.SetDetails(new UserDetailsMapper().ConvertToBusinessLogicEntity(userDetailsAdapter.GetById(dalUser.DetailsId)));

            var transactionTypes = new TransactionTypeMapper().ConvertToBusinessLogicEntitiesCollection(transactionTypeAdapter.GetAll($"language_id={language.Id}")).ToList();
            var transferTypes = new TransferTypeMapper().ConvertToBusinessLogicEntitiesCollection(transferTypeAdapter.GetAll($"language_id={language.Id}")).ToList();
            var frequencies = new FrequenceMapper().ConvertToBusinessLogicEntitiesCollection(frequenceAdapter.GetAll($"language_id={language.Id}")).ToList();
            var importances = new ImportanceMapper().ConvertToBusinessLogicEntitiesCollection(importanceAdapter.GetAll($"language_id={language.Id}")).ToList();

            var tags = new TagMapper().ConvertToBusinessLogicEntitiesCollection(tagAdapter.GetAll($"language_id={language.Id}")).ToList();
            var operations = new OperationMapper(user, importances,frequencies,transactionTypes,transferTypes).ConvertToBusinessLogicEntitiesCollection(operationAdapter.GetAll($"user_id={user.Id}")).ToList();

            var filter = "";
            foreach(var operation in operations)
            {
                filter += $"operation_id={operation.Id} AND ";
            }
            filter = filter.Substring(0, filter.Length - 4);

            var relations = new RelationMapper(operations, tags).ConvertToBusinessLogicEntitiesCollection(operationTagRelationAdapter.GetAll(filter));
            var relTags = relations.Item1;
            var relOperations = relations.Item2;

            foreach(var operation in operations)
            {
                operation.SetDetailsList(new OperationDetailsMapper().ConvertToBusinessLogicEntitiesCollection(operationDetailsAdapter.GetAll($"operaiton_id={operation.Id}")));
                operation.SetTags(relTags.Where(r => r.OperationId == operation.Id));
            }

            foreach(var tag in tags)
            {
                tag.SetOperations(relOperations.Where(r => r.TagId == tag.Id));
            }
        }

        #region EVENTS

        private void _b1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender);
            brdMain.Child = new AnalysisView();
        }

        private void _b2_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender);
            brdMain.Child = new ImportView();
        }

        private void _b3_Click(object sender, RoutedEventArgs e)
        {
            test();
        }

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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paygl
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        #region FIELDS
        private readonly bool[] _changes = new bool[14];
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets a value indicating whether changes have occurred.
        /// </summary>
        /// <value>
        ///   <c>true</c> if changes have occurred; otherwise, <c>false</c>.
        /// </value>
        public bool ChangeSettings { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public Settings()
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow;

            if (mainWindow == null) return;
            Left = mainWindow.Left + (mainWindow.Width) / 2 - Width / 2;
            Top = mainWindow.Top + (mainWindow.Height) / 2 - Height / 2;
        }
        #endregion

        #region  PRIVATE
        private void SetSettings()
        {

        }

        private void GetSettings()
        {

        }

        private void Update()
        {

        }
        #endregion

        #region EVENTS
        /// <summary>
        /// Handles the Click event of the BtnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the SettingApply control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SettingApply_Click(object sender, RoutedEventArgs e)
        {
            SetSettings();
            ChangeSettings = true;
        }


        #region TITLE_BAR
        /// <summary>
        /// Handles the MouseDown event of the TitleBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            var thisCurrentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            thisCurrentWindow?.DragMove();
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #endregion
    }
}

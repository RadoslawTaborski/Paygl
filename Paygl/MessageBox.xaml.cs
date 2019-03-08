using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Paygl
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox
    {
        public MessageBox(string header, string body)
        {
            InitializeComponent();
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow == null) return;
            Left = mainWindow.Left + (mainWindow.Width) / 2 - Width / 2;
            Top = mainWindow.Top + (mainWindow.Height) / 2 - Height / 2;

            Name.Text = header;
            RtbInfo.Document.Blocks.Clear();
            RtbInfo.Document.Blocks.Add(new Paragraph(new Run(body)));
            BlockCollection blocks = RtbInfo.Document.Blocks;
            foreach (Block b in blocks)
                b.TextAlignment = TextAlignment.Center;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
        #endregion
    }
}

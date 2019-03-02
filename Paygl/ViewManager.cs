using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Paygl
{
    public static class ViewManager
    {
        private static MainWindow _main;

        public static void SetMainWindow(MainWindow main)
        {
            _main = main;
        }

        public static void AddUserControl(UserControl uc)
        {
            _main.AddUserControl(uc);
        }

        public static void OpenUserControl(UserControl uc)
        {
            _main.OpenUserControl(uc);
        }

        public static void RemoveUserControl(UserControl uc)
        {
            _main.RemoveFromBarView(uc);
        }
    }
}

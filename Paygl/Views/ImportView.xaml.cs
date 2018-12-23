using DataAccess;
using DataBaseWithBusinessLogicConnector;
using DataBaseWithBusinessLogicConnector.Dal.Adapters;
using DataBaseWithBusinessLogicConnector.Dal.Mappers;
using Importer;
using PayglService.cs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl
    {
        private Service _service;
        public Service Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new Service();
                }

                return _service;

            }
        }
        public ImportView()
        {
            InitializeComponent();
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            Service.LoadAttributes();
        }

        private void LoadOperations()
        {
            Service.LoadOperations();
        }

        private void ImportOperations_Click(object sender, RoutedEventArgs e)
        {
            Service.Import();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using DataBaseWithBusinessLogicConnector.Entities;
using PayglService.cs;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : IRepresentative
    {
        private ObservableRangeCollection<Language> _observableLanguages;

        public SettingsView()
        {
            InitializeComponent();
            _tbUserLanguage.Text = Service.Language.FullName;
            _observableLanguages = new ObservableRangeCollection<Language>(Service.Languages);
            _cbAppLanguage.ItemsSource = _observableLanguages;
            _cbAppLanguage.SelectedItem = _observableLanguages.First(l => l.ShortName == ConfigurationManager.Language());
        }

        public string RepresentativeName { get; set; } = Properties.strings.settingsRN;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(((Language) _cbAppLanguage.SelectedItem).ShortName);
        }

        public override string ToString()
        {
            return "Settings";
        }
    }
}

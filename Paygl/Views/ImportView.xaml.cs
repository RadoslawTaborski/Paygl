using DataAccess;
using DataBaseWithBusinessLogicConnector;
using DataBaseWithBusinessLogicConnector.Dal.Adapters;
using DataBaseWithBusinessLogicConnector.Dal.Mappers;
using DataBaseWithBusinessLogicConnector.Entities;
using Importer;
using PayglService.cs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Operation> _operations;
        private Operation _operation;
        private int _index;

        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequence> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<Operation> _observableOperations;
        private ObservableRangeCollection<Tag> _observableTags;

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
            Background = Brushes.Azure;
            LoadAttributes();
            LoadOperations();

            SetEditableControls();
            UserEditableControlsVisibility(Visibility.Hidden);
        }

        private void SetEditableControls()
        {
            _observableFrequencies = new ObservableRangeCollection<Frequence>(Service.Frequencies);
            this.cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            this.cbImportance.ItemsSource = _observableImportances;
            _observableOperations = new ObservableRangeCollection<Operation>();
            _observableOperations.Add(null);
            _observableOperations.AddRange(Service.Operations);
            this.cbRelated.ItemsSource = _observableOperations;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            this.cbTags.ItemsSource = _observableTags;
        }

        private void ResetEditableControls()
        {
            _selectedTags = new List<Tag>();
            TagStack.Children.Clear();
            cbRelated.SelectedItem = null;
            cbTags.SelectedItem = null;
            cbFrequent.SelectedItem = null;
            cbImportance.SelectedItem = null;
        }

        private void UserEditableControlsVisibility(Visibility v)
        {
            cbFrequent.Visibility = v;
            cbImportance.Visibility = v;
            cbTags.Visibility = v;
            cbRelated.Visibility = v;
            tbNewDescription.Visibility = v;
        }

        private void AddObservableOperation(Operation operation)
        {
            _observableOperations.Add(operation);
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
            _operations = Service.Import();
            if (_operations.Count > 0)
            {
                Show(0);
            }
        }

        private void ImportAccept_Click(object sender, RoutedEventArgs e)
        {
            _operation.SetImportance(cbImportance.SelectedItem as Importance);
            _operation.SetFrequence(cbFrequent.SelectedItem as Frequence);
            foreach (var item in _selectedTags)
            {
                _operation.AddTag(item);
            }
            _operation.SetParent(cbRelated.SelectedItem as Operation);
            _operation.ChangeDescription(tbNewDescription.Text);

            Service.UpdateOperationComplex(_operation);
            AddObservableOperation(_operation);

            ShowNext();
        }

        private void ImportIgnore_Click(object sender, RoutedEventArgs e)
        {
            ShowNext();
        }

        private void Show(int index)
        {
            if (index < _operations.Count)
            {
                _operation = _operations[index];

                ResetEditableControls();

                tbDescription.Text = _operation.Description;
                tbNewDescription.Text = _operation.Description;
                lDate.Content = _operation.Date.ToString("dd.MM.yyyy");
                lAmount.Content = _operation.Amount;
                lTransaction.Content = _operation.TransactionType.Text;
                lTransfer.Content = _operation.TransferType.Text;

                UserEditableControlsVisibility(Visibility.Visible);
            }
        }

        private void ShowNext()
        {
            _index++;
            Show(_index);
        }

        private void ShowPrevious()
        {
            _index--;
            Show(_index);
        }

        private void CbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = cbTags.SelectedItem as Tag;

            if (selected != null)
            {
                if (!IsExistInSelectedTags(selected))
                {
                    _selectedTags.Add(selected);
                    var newlabel = new Label
                    {
                        Content = selected.ToString()
                    };
                    newlabel.MouseDoubleClick += TagLabel_MouseDoubleClick;
                    newlabel.Style = (Style)FindResource("MyLabel");
                    TagStack.Children.Add(newlabel);
                }
            }
        }

        private void TagLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            var tag = Service.Tags.Where(t => t.Text.Equals(label.Content)).First();
            TagStack.Children.Remove(label);
            _selectedTags.Remove(tag);
        }

        private bool IsExistInSelectedTags(Tag newTag)
        {
            foreach (var item in _selectedTags)
            {
                if (item.Equals(newTag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

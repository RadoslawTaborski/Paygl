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

        public ImportView()
        {
            InitializeComponent();
            Background = Brushes.Azure;

            LoadAttributes();
            LoadOperations();

            SetEditableControls();
            UserEditableControlsVisibility(Visibility.Hidden);

            _operations = Service.Import();
            if (_operations.Count > 0)
            {
                Show(0);
            }
        }

        private void SetEditableControls()
        {
            _observableFrequencies = new ObservableRangeCollection<Frequence>(Service.Frequencies);
            this.cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            this.cbImportance.ItemsSource = _observableImportances;
            _observableOperations = new ObservableRangeCollection<Operation>();
            _observableOperations.Add(null);
            _observableOperations.AddRange(Service.Operations.Where(o => o.Parent == null));
            this.cbRelated.ItemsSource = _observableOperations;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            this.cbTags.ItemsSource = _observableTags;
        }

        private void ResetEditableControls()
        {
            _selectedTags = new List<Tag>();
            cbRelated.SelectedItem = null;
            cbTags.SelectedItem = null;
            cbFrequent.SelectedItem = null;
            cbImportance.SelectedItem = null;
            TagStack.Children.Clear();
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

        private void ImportAccept_Click(object sender, RoutedEventArgs e)
        {
            var tmp = tbDescription.Text;
            _operation.SetImportance(cbImportance.SelectedItem as Importance);
            _operation.SetFrequence(cbFrequent.SelectedItem as Frequence);
            foreach (var item in _selectedTags)
            {
                _operation.AddTag(item);
            }
            _operation.SetParent(cbRelated.SelectedItem as Operation);
            _operation.ChangeDescription(tbNewDescription.Text);
            _operation.SetShortDescription(tbNewDescription.Text);

            try
            {
                Service.UpdateOperationComplex(_operation);
                if (_operation.Parent == null)
                {
                    AddObservableOperation(_operation);
                }
                _operations.Remove(_operation);
            } catch (Exception ex)
            {
                var dialog = new MessageBox("Komunikat", ex.Message);
                _operation.ChangeDescription(tmp);
                dialog.ShowDialog();
            }
            Show(_index);
        }

        private void ImportIgnore_Click(object sender, RoutedEventArgs e)
        {
            _operations.Remove(_operation);
            Show(_index);
        }

        private void Show(int index)
        {
            if (index < _operations.Count && index >= 0)
            {
                _operation = _operations[index];

                ResetEditableControls();

                tbDescription.Text = _operation.Description;
                tbNewDescription.Text = _operation.ShortDescription;
                lDate.Content = _operation.Date.ToString("dd.MM.yyyy");
                lAmount.Content = _operation.Amount;
                cbFrequent.SelectedItem = _operation.Frequence;
                cbImportance.SelectedItem = _operation.Importance;
                lTransaction.Content = _operation.TransactionType.Text;
                lTransfer.Content = _operation.TransferType.Text;
                foreach (var tag in _operation.Tags)
                {
                    SetTagLabel(tag.Tag);
                }

                UserEditableControlsVisibility(Visibility.Visible);
            }
        }

        private void ShowNext()
        {
            if (_index < _operations.Count - 1)
            {
                _index++;
                Show(_index);
            }
        }

        private void ShowPrevious()
        {
            if (_index > 0)
            {
                _index--;
                Show(_index);
            }
        }

        private void CbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = cbTags.SelectedItem as Tag;

            if (selected != null)
            {
                SetTagLabel(selected);
            }
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

        private void SetTagLabel(Tag tag)
        {
            if (!IsExistInSelectedTags(tag))
            {
                _selectedTags.Add(tag);
                var newborder = new Border();
                newborder.Style = (Style)FindResource("MyBorder");

                var newstackpanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                var newlabel = new Label
                {
                    Content = tag.ToString()
                };
                newlabel.Style = (Style)FindResource("MyLabel");

                var newbutton = new Button
                {
                    Content = "X",
                    Width = 20,
                    Height = 20,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                };
                newbutton.Style = (Style)FindResource("MyButton");
                newbutton.Click += Close_Click;

                newstackpanel.Children.Add(newlabel);
                newstackpanel.Children.Add(newbutton);
                newborder.Child = newstackpanel;
                TagStack.Children.Add(newborder);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var panel = button.Parent as StackPanel;
            var border = panel.Parent as Border;
            var label = panel.Children[0] as Label;

            var tag = Service.Tags.Where(t => t.Text.Equals(label.Content)).First();
            TagStack.Children.Remove(border);
            _selectedTags.Remove(tag);
        }

        private void ImportBack_Click(object sender, RoutedEventArgs e)
        {
            ShowPrevious();
        }

        private void ImportNext_Click(object sender, RoutedEventArgs e)
        {
            ShowNext();
        }
    }
}

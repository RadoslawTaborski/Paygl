using DataBaseWithBusinessLogicConnector.Entities;
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
    /// Interaction logic for ManualyAddView.xaml
    /// </summary>
    public partial class ManuallyAddView : UserControl
    {
        private Operation _operation;
        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequence> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<Operation> _observableOperations;
        private ObservableRangeCollection<Tag> _observableTags;
        private ObservableRangeCollection<TransactionType> _observableTransactionType;
        private ObservableRangeCollection<TransferType> _observableTransferType;

        public ManuallyAddView()
        {
            InitializeComponent();
            Background = Brushes.Azure;

            LoadAttributes();
            LoadOperations();

            SetEditableControls();
            SetOperationValues();
        }

        private void SetEditableControls()
        {
            udAmount.Value = 0.00M;
            _observableFrequencies = new ObservableRangeCollection<Frequence>(Service.Frequencies);
            this.cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            this.cbImportance.ItemsSource = _observableImportances;
            _observableTransactionType = new ObservableRangeCollection<TransactionType>(Service.TransactionTypes);
            this.cbTransaction.ItemsSource = _observableTransactionType;
            _observableTransferType = new ObservableRangeCollection<TransferType>(Service.TransferTypes);
            this.cbTransfer.ItemsSource = _observableTransferType;
            _observableOperations = new ObservableRangeCollection<Operation>();
            _observableOperations.Add(null);
            _observableOperations.AddRange(Service.Operations);
            this.cbRelated.ItemsSource = _observableOperations;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            this.cbTags.ItemsSource = _observableTags;
        }

        private void SetOperationValues()
        {
            CalendarBorder.Visibility = Visibility.Hidden;
            calDate.SelectedDate = DateTime.Now;

            _operation = new Operation(null, null, Service.User, "", 0M, null, null, null, null, DateTime.Now, "");

            ResetEditableControls();

            tbNewDescription.Text = "";
            lDate.Content = _operation.Date.ToString("dd.MM.yyyy");
            udAmount.Value = _operation.Amount;
            cbFrequent.SelectedItem = _operation.Frequence;
            cbImportance.SelectedItem = _operation.Importance;
            cbTransaction.SelectedItem = Service.TransactionTypes[1];
            cbTransfer.SelectedItem = Service.TransferTypes[0];
            foreach (var tag in _operation.Tags)
            {
                SetTagLabel(tag.Tag);
            }

            UserEditableControlsVisibility(Visibility.Visible);
        }

        private void ResetEditableControls()
        {
            tbNewDescription.Text = "";
            udAmount.Value = 0.00M;
            _selectedTags = new List<Tag>();
            cbTransaction.SelectedItem = Service.TransactionTypes[1];
            cbTransfer.SelectedItem = Service.TransferTypes[0];
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
            cbTransaction.Visibility = v;
            cbTransfer.Visibility = v;
            cbRelated.Visibility = v;
            tbNewDescription.Visibility = v;
            udAmount.Visibility = v;
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

        private void ManualClear_Click(object sender, RoutedEventArgs e)
        {
            ResetEditableControls();
        }

        private void ManualAccept_Click(object sender, RoutedEventArgs e)
        {
            _operation.SetImportance(cbImportance.SelectedItem as Importance);
            _operation.SetFrequence(cbFrequent.SelectedItem as Frequence);
            _operation.SetTransaction(cbTransaction.SelectedItem as TransactionType);
            _operation.SetTransfer(cbTransfer.SelectedItem as TransferType);
            foreach (var item in _selectedTags)
            {
                _operation.AddTag(item);
            }
            _operation.SetParent(cbRelated.SelectedItem as Operation);
            _operation.ChangeDescription(tbNewDescription.Text);
            _operation.SetShortDescription(tbNewDescription.Text);
            _operation.SetAmount(udAmount.Value);

            try
            {
                Service.UpdateOperationComplex(_operation);
                AddObservableOperation(_operation);
                ResetEditableControls();
            }catch(Exception ex)
            {
                var dialog = new MessageBox("Komunikat", ex.Message);
                dialog.ShowDialog();
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

                var newstackpanel = new StackPanel();
                newstackpanel.Orientation = Orientation.Horizontal;

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

        private void BtnCalendar_Click(object sender, RoutedEventArgs e)
        {
            CalendarBorder.Visibility = Visibility.Visible;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CalendarBorder.Visibility = Visibility.Hidden;
            if (calDate.SelectedDate.HasValue)
            {
                lDate.Content = calDate.SelectedDate.Value.ToString("dd.MM.yyyy");
            }
            CalendarBorder.Visibility = Visibility.Hidden;
        }
    }
}

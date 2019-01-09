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
    public partial class ManuallyOperationsView : UserControl
    {
        private Operation _operation;
        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequence> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<OperationsGroup> _observableGroups;
        private ObservableRangeCollection<Tag> _observableTags;
        private ObservableRangeCollection<TransactionType> _observableTransactionType;
        private ObservableRangeCollection<TransferType> _observableTransferType;

        public ManuallyOperationsView()
        {
            InitializeComponent();
            Background = Brushes.Azure;

            LoadAttributes();
            LoadOperationsGroup();

            SetEditableControls();
            SetOperationValues();
            SetRelatedControlsAttributesVisibility(Visibility.Hidden);
            SetUserEditableControlsVisibility(Visibility.Visible);
        }

        private void SetEditableControls()
        {
            _upDownAmount.Value = 0.00M;
            _observableFrequencies = new ObservableRangeCollection<Frequence>(Service.Frequencies);
            _cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            _cbImportance.ItemsSource = _observableImportances;
            _observableTransactionType = new ObservableRangeCollection<TransactionType>(Service.TransactionTypes);
            _cbTransaction.ItemsSource = _observableTransactionType;
            _observableTransferType = new ObservableRangeCollection<TransferType>(Service.TransferTypes);
            _cbTransfer.ItemsSource = _observableTransferType;
            _observableGroups = new ObservableRangeCollection<OperationsGroup>();
            _observableGroups.Add(null);
            _observableGroups.AddRange(Service.OperationsGroups);
            _cbRelated.ItemsSource = _observableGroups;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            _cbTags.ItemsSource = _observableTags;
        }

        private void SetOperationValues()
        {
            _borderCalendar.Visibility = Visibility.Hidden;
            _calDate.SelectedDate = DateTime.Now;

            _operation = new Operation(null, null, Service.User, "", 0M, null, null, null, null, DateTime.Now, "");

            ResetEditableControls();

            _tbNewDescription.Text = "";
            _labDate.Content = _operation.Date.ToString("dd.MM.yyyy");
            _upDownAmount.Value = _operation.Amount;
            _cbFrequent.SelectedItem = _operation.Frequence;
            _cbImportance.SelectedItem = _operation.Importance;
            _cbTransaction.SelectedItem = Service.TransactionTypes[1];
            _cbTransfer.SelectedItem = Service.TransferTypes[0];
            foreach (var tag in _operation.Tags)
            {
                SetTagLabel(tag.Tag);
            }

            SetUserEditableControlsVisibility(Visibility.Visible);
        }

        private void ResetEditableControls()
        {
            _tbNewDescription.Text = "";
            _upDownAmount.Value = 0.00M;
            _selectedTags = new List<Tag>();
            _cbTransaction.SelectedItem = Service.TransactionTypes[1];
            _cbTransfer.SelectedItem = Service.TransferTypes[0];
            _cbRelated.SelectedItem = null;
            _cbTags.SelectedItem = null;
            _cbFrequent.SelectedItem = null;
            _cbImportance.SelectedItem = null;
            _spTags.Children.Clear();
        }

        private void ResetRelatedAttributes()
        {
            _labFrequence.Content = "";
            _labImportance.Content = "";
            _labTags.Content = "";
        }

        private void SetUserEditableControlsVisibility(Visibility v)
        {
            _cbFrequent.Visibility = v;
            _cbImportance.Visibility = v;
            _cbTags.Visibility = v;
            _cbTransaction.Visibility = v;
            _cbTransfer.Visibility = v;
            _cbRelated.Visibility = v;
            _tbNewDescription.Visibility = v;
            _upDownAmount.Visibility = v;
        }

        private void SetRelatedControlsAttributesVisibility(Visibility v)
        {
            _labFrequence.Visibility = v;
            _labImportance.Visibility = v;
            _labTags.Visibility = v;

            var v2 = v == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;

            _cbFrequent.Visibility = v2;
            _cbImportance.Visibility = v2;
            _spTags.Visibility = v2;
            _cbTags.Visibility = v2;
        }

        private void SetRelatedAttributes()
        {
            var group = _cbRelated.SelectedItem as OperationsGroup;

            _labFrequence.Content = group.Frequence;
            _labImportance.Content = group.Importance;
            foreach (var item in group.Tags)
            {
                _labTags.Content += item.Tag + "; ";
            }
        }

        private void AddObservableOperation(OperationsGroup group)
        {
            _observableGroups.Add(group);
        }

        private void LoadAttributes()
        {
            Service.LoadAttributes();
        }

        private void LoadOperationsGroup()
        {
            Service.LoadOperationsGroups();
        }

        private void BtnManualClear_Click(object sender, RoutedEventArgs e)
        {
            ResetEditableControls();
        }

        private void BtnManualAccept_Click(object sender, RoutedEventArgs e)
        {
            _operation.SetTransaction(_cbTransaction.SelectedItem as TransactionType);
            _operation.SetTransfer(_cbTransfer.SelectedItem as TransferType);
            _operation.SetDate(DateTime.ParseExact(_labDate.Content.ToString(), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));
            _operation.SetParent(_cbRelated.SelectedItem as OperationsGroup);
            _operation.SetDescription(_tbNewDescription.Text);
            _operation.SetShortDescription(_tbNewDescription.Text);
            _operation.SetAmount(_upDownAmount.Value);

            if (_operation.Parent != null)
            {
                _operation.SetImportance(_operation.Parent.Importance);
                _operation.SetFrequence(_operation.Parent.Frequence);
                _operation.RemoveAllTags();
                foreach (var item in _operation.Parent.Tags)
                {
                    _operation.AddTag(item.Tag);
                }
            }
            else
            {
                _operation.SetImportance(_cbImportance.SelectedItem as Importance);
                _operation.SetFrequence(_cbFrequent.SelectedItem as Frequence);
                _operation.RemoveAllTags();
                foreach (var item in _selectedTags)
                {
                    _operation.AddTag(item);
                }
            }

            try
            {
                Service.UpdateOperationComplex(_operation);
                ResetEditableControls();
                SetEditableControls();
                SetOperationValues();
            }
            catch(Exception ex)
            {
                var dialog = new MessageBox("Komunikat", ex.Message);
                dialog.ShowDialog();
            }
        }

        private void CbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = _cbTags.SelectedItem as Tag;

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
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(@"..\img\x-icon.png", UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    Width = 20,
                    Height = 20,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                };
                newbutton.Style = (Style)FindResource("MyButton");
                newbutton.Click += BtnClose_Click;

                newstackpanel.Children.Add(newlabel);
                newstackpanel.Children.Add(newbutton);
                newborder.Child = newstackpanel;
                _spTags.Children.Add(newborder);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var panel = button.Parent as StackPanel;
            var border = panel.Parent as Border;
            var label = panel.Children[0] as Label;

            var tag = Service.Tags.Where(t => t.Text.Equals(label.Content)).First();
            _spTags.Children.Remove(border);
            _selectedTags.Remove(tag);
        }

        private void BtnCalendar_Click(object sender, RoutedEventArgs e)
        {
            _borderCalendar.Visibility = Visibility.Visible;
        }

        private void CalDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _borderCalendar.Visibility = Visibility.Hidden;
            if (_calDate.SelectedDate.HasValue)
            {
                _labDate.Content = _calDate.SelectedDate.Value.ToString("dd.MM.yyyy");
            }
            _borderCalendar.Visibility = Visibility.Hidden;
        }

        private void CbRelated_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cbRelated.SelectedItem != null)
            {
                ResetRelatedAttributes();
                SetRelatedAttributes();
                SetRelatedControlsAttributesVisibility(Visibility.Visible);
            }
            else
            {
                ResetRelatedAttributes();
                SetRelatedControlsAttributesVisibility(Visibility.Hidden);
            }
        }
    }
}

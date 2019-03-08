using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using Paygl.Models;
using PayglService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ManualyAddView.xaml
    /// </summary>
    public partial class ManuallyOperationsView : IRepresentative
    {
        private Operation _operation;
        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequence> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<OperationsGroup> _observableGroups;
        private ObservableRangeCollection<Tag> _observableTags;
        private ObservableRangeCollection<TransactionType> _observableTransactionType;
        private ObservableRangeCollection<TransferType> _observableTransferType;

        public string RepresentativeName { get; set; } = Properties.strings.addManualyRN;

        public ManuallyOperationsView()
        {
            InitializeComponent();
            ViewsMemory.AddedGroup += AddedGroupEvent;
            ViewsMemory.AddedParameter += AddedParameterEvent;
            Background = Brushes.Azure;

            LoadAttributes();
            LoadOperationsGroup();
            var operation = new Operation(null, null, Service.User, "", 0M, null, null, null, null, DateTime.Now, "");

            EditMode(false);
            SetEditableControls();
            SetOperationValues(operation);
            SetRelatedControlsAttributesVisibility(Visibility.Hidden);
            SetUserEditableControlsVisibility(Visibility.Visible);
        }

        public ManuallyOperationsView(Operation operation)
        {
            InitializeComponent();
            ViewsMemory.AddedGroup += AddedGroupEvent;
            ViewsMemory.AddedParameter += AddedParameterEvent;
            Background = Brushes.Azure;

            LoadAttributes();
            LoadOperationsGroup();

            SetEditableControls();
            SetUserEditableControlsVisibility(Visibility.Visible);
            SetOperationValues(operation);
            if (operation.Parent != null)
            {
                ResetRelatedAttributes();
                SetRelatedAttributes(operation.Parent);
                SetRelatedControlsAttributesVisibility(Visibility.Visible);
            }
            else
            {
                SetRelatedControlsAttributesVisibility(Visibility.Hidden);
            }
            EditMode(true);
        }

        private void AddedParameterEvent(IParameter added)
        {
            throw new NotImplementedException();
        }

        private void AddedGroupEvent(OperationsGroup added)
        {
            _observableGroups.Add(added);
        }

        private void EditMode(bool value)
        {
            if (value)
            {
                RepresentativeName = $"{Properties.strings.editManualyRN} {_operation.ShortDescription}";
                _btnManualClear.Visibility = Visibility.Hidden;
                _btnManualAccept.Visibility = Visibility.Hidden;
                _tbDescription.Visibility = Visibility.Visible;
                _labDescription.Visibility = Visibility.Visible;
                _btnEditAccept.Visibility = Visibility.Visible;
            }
            else
            {
                RepresentativeName = Properties.strings.addManualyRN;
                _tbDescription.Visibility = Visibility.Hidden;
                _labDescription.Visibility = Visibility.Hidden;
                _btnManualClear.Visibility = Visibility.Visible;
                _btnManualAccept.Visibility = Visibility.Visible;
                _btnEditAccept.Visibility = Visibility.Hidden;
            }
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
            _observableGroups = new ObservableRangeCollection<OperationsGroup> {null};
            _observableGroups.AddRange(Service.OperationsGroups);
            _cbRelated.ItemsSource = _observableGroups;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            _cbTags.ItemsSource = _observableTags;
        }

        private void SetOperationValues(Operation operation)
        {
            _borderCalendar.Visibility = Visibility.Hidden;
            _calDate.SelectedDate = DateTime.Now;

            _operation = operation;

            ResetEditableControls();

            _tbDescription.Text = _operation.Description;
            _tbNewDescription.Text = _operation.Description;
            _labDate.Content = _operation.Date.ToString(Properties.strings.dateFormat);
            _upDownAmount.Value = _operation.Amount;
            if (_operation.Frequence != null)
            {
                _cbFrequent.SelectedItem = _observableFrequencies.First(f => f.Text == _operation.Frequence.Text);
            }
            if (_operation.Importance!=null)
            {
                _cbImportance.SelectedItem = _observableImportances.First(f => f.Text == _operation.Importance.Text);
            }
            if (_operation.TransactionType != null)
            {
                _cbTransaction.SelectedItem = _observableTransactionType.First(f => f.Text == _operation.TransactionType.Text);
            }
            if (_operation.TransferType != null)
            {
                _cbTransfer.SelectedItem = _observableTransferType.First(f => f.Text == _operation.TransferType.Text);
            }
            if (_operation.Parent != null)
            {
                var tmp = _observableGroups.First(f => f != null && f.Id == _operation.Parent.Id);
                _cbRelated.SelectedItem = tmp;
            }
            foreach (var tag in _operation.Tags)
            {
                if (!tag.IsMarkForDeletion && !tag.IsDirty)
                {
                    SetTagLabel(tag.Tag);
                }
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

            var v2 = v == Visibility.Hidden ?Visibility.Visible:Visibility.Hidden;

            _cbFrequent.Visibility = v2;
            _cbImportance.Visibility = v2;
            _spTags.Visibility = v2;
            _cbTags.Visibility = v2;
        }

        private void SetRelatedAttributes()
        {
            var group = _cbRelated.SelectedItem as OperationsGroup;
            SetRelatedAttributes(group);
        }

        private void SetRelatedAttributes(OperationsGroup group)
        {
            _labFrequence.Content = group.Frequence;
            _labImportance.Content = group.Importance;
            foreach (var item in group.Tags)
            {
                _labTags.Content += item.Tag + "; ";
            }
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
            _operation.SetDate(DateTime.ParseExact(_labDate.Content.ToString(), Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture));
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
                var operation=new Operation(null, null, Service.User, "", 0M, null, null, null, null, DateTime.Now, "");
                SetEditableControls();
                SetOperationValues(operation);
            }
            catch(Exception ex)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, ex.Message);
                dialog.ShowDialog();
            }
        }

        private void CbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cbTags.SelectedItem is Tag selected)
            {
                SetTagLabel(selected);
            }
        }

        private bool IsExistInSelectedTags(Tag newTag)
        {
            foreach (var item in _selectedTags)
            {
                if (item.Text == newTag.Text)
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
                var newBorder = new Border {Style = (Style) FindResource("MyBorder")};

                var newStackPanel = new StackPanel {Orientation = Orientation.Horizontal};

                var newLabel = new Label {Content = tag.ToString(), Style = (Style) FindResource("MyLabel")};

                var newButton = new ButtonWithObject
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri(@"..\img\x-icon.png", UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    Object = tag,
                    Width = 20,
                    Height = 20,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Style = (Style) FindResource("MyButton"),
                };
                newButton.Click += BtnClose_Click;

                newStackPanel.Children.Add(newLabel);
                newStackPanel.Children.Add(newButton);
                newBorder.Child = newStackPanel;
                _spTags.Children.Add(newBorder);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonWithObject;
            var panel = button?.Parent as StackPanel;
            var border = panel?.Parent as Border;

            var tag = button?.Object as Tag;
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
                _labDate.Content = _calDate.SelectedDate.Value.ToString(Properties.strings.dateFormat);
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

        public override string ToString()
        {
            return "ManuallyOperationsView";
        }

        private void _btnEditAccept_Click(object sender, RoutedEventArgs e)
        {
            _operation.SetTransaction(_cbTransaction.SelectedItem as TransactionType);
            _operation.SetTransfer(_cbTransfer.SelectedItem as TransferType);
            _operation.SetDate(DateTime.ParseExact(_labDate.Content.ToString(), Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture));
            _operation.SetParent(_cbRelated.SelectedItem as OperationsGroup);
            _operation.SetDescription(_tbNewDescription.Text);
            _operation.SetShortDescription(_tbNewDescription.Text);
            _operation.SetAmount(_upDownAmount.Value);

            if (_operation.Parent != null)
            {
                _operation.SetImportance(_operation.Parent.Importance);
                _operation.SetFrequence(_operation.Parent.Frequence);
                UpdateOperationTags(_operation.Parent.Tags);
            }
            else
            {
                _operation.SetImportance(_cbImportance.SelectedItem as Importance);
                _operation.SetFrequence(_cbFrequent.SelectedItem as Frequence);
                UpdateOperationTags();
            }

            try
            {
                Service.UpdateOperationComplex(_operation);
                var dialog = new MessageBox(Properties.strings.messageBoxStatement,Properties.strings.messageBoxModificationSuccess);
                dialog.ShowDialog();
                ResetEditableControls();
                SetEditableControls();
                SetOperationValues(_operation);
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, ex.Message);
                dialog.ShowDialog();
            }
        }

        private void UpdateOperationTags()
        {
            _operation.Tags.ForEach(t => t.IsMarkForDeletion = true);
            foreach (var item in _selectedTags)
            {
                _operation.AddTag(item);
            }
        }

        private void UpdateOperationTags(List<RelTag> parentTag)
        {
            _operation.Tags.ForEach(t => t.IsMarkForDeletion = true);
            foreach (var relTag in parentTag)
            {
                _operation.AddTag(relTag.Tag);
            }
        }
    }
}

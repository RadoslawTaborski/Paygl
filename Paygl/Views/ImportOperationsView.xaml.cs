using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using Paygl.Models;
using PayglService.cs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using Orientation = System.Windows.Controls.Orientation;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportOperationsView : IRepresentative
    {
        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequence> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<OperationsGroup> _observableGroups;
        private ObservableRangeCollection<Tag> _observableTags;

        public string RepresentativeName { get; set; } = Properties.strings.importRN;

        public ImportOperationsView()
        {
            InitializeComponent();
            ViewsMemory.AddedGroup += AddedGroupEvent;
            ViewsMemory.AddedParameter += AddedParameterEvent;
            Background = Brushes.Azure;

            LoadAttributes();
            LoadOperationsGroup();

            SetStandardEditableControls();
            SetCloneEditableControls();

            SetCloneOptionsVisibility(Visibility.Hidden);
            SetRelatedControlsAttributesVisibility(Visibility.Hidden);
            SetUserStandardEditableControlsVisibility(Visibility.Hidden);
            SetNavigateButtonVisibility(Visibility.Hidden);

            ViewsMemory.AddImportingOperations(Service.Import());
            if (ViewsMemory.CurrentOperation() == null) return;

            SetNavigateButtonVisibility(Visibility.Visible);
            Show(ViewsMemory.CurrentOperation());
        }

        private void AddedParameterEvent(IParameter added)
        {
            throw new NotImplementedException();
        }

        private void AddedGroupEvent(OperationsGroup added)
        {
            _observableGroups.Add(added);
        }

        private void SetStandardEditableControls()
        {
            _observableFrequencies = new ObservableRangeCollection<Frequence>(Service.Frequencies);
            _cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            _cbImportance.ItemsSource = _observableImportances;
            _observableGroups = new ObservableRangeCollection<OperationsGroup> {null};
            _observableGroups.AddRange(Service.OperationsGroups);
            _cbRelated.ItemsSource = _observableGroups;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            _cbTags.ItemsSource = _observableTags;
        }

        private void SetCloneEditableControls()
        {
            _upDownClone.Value = decimal.Zero;
        }

        private void SetRelatedAttributes()
        {
            var group = _cbRelated.SelectedItem as OperationsGroup;

            _labFrequence.Content = group?.Frequence;
            _labImportance.Content = group?.Importance;
            if (group?.Tags == null) return;
            foreach (var item in group.Tags)
            {
                _labTags.Content += item.Tag + "; ";
            }
        }

        private void ResetStandardEditableControls()
        {
            _selectedTags = new List<Tag>();
            _cbRelated.SelectedItem = null;
            _cbTags.SelectedItem = null;
            _cbFrequent.SelectedItem = null;
            _cbImportance.SelectedItem = null;
            _spTags.Children.Clear();
        }

        private void ResetAllControls()
        {
            ResetStandardEditableControls();
            ResetRelatedAttributes();
            ResetCloneOptions();
            _tbDescription.Text = "";
            _tbNewDescription.Text = "";
            _labAmount.Content = "";
            _labDate.Content = "";
            _labTransaction.Content = "";
            _labTransfer.Content = "";
        }

        private void ResetCloneOptions()
        {
            _upDownClone.Value = 0.00m;
        }

        private void ResetRelatedAttributes()
        {
            _labFrequence.Content = "";
            _labImportance.Content = "";
            _labTags.Content = "";
        }

        private void SetNavigateButtonVisibility(Visibility v)
        {
            _btnBack.Visibility = v;
            _btnNext.Visibility = v;
            _btnImportIgnore.Visibility = v;
            _btnImportClone.Visibility = v;
            _btnImportAccept.Visibility = v;
        }

        private void SetUserStandardEditableControlsVisibility(Visibility v)
        {
            _cbFrequent.Visibility = v;
            _cbImportance.Visibility = v;
            _cbTags.Visibility = v;
            _cbRelated.Visibility = v;
            _tbNewDescription.Visibility = v;
            _spTags.Visibility = v;
        }

        private void SetCloneOptionsVisibility(Visibility v)
        {
            _upDownClone.Visibility = v;
            btnClone.Visibility = v;
            btnCloneCancel.Visibility = v;

            var v2 = v == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            _btnBack.Visibility = v2;
            _btnNext.Visibility = v2;
            _btnImportIgnore.Visibility = v2;
            _spTags.Visibility = v2;

            SetUserStandardEditableControlsVisibility(v2);
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

        private void LoadAttributes()
        {
            Service.LoadAttributes();
        }

        private void LoadOperationsGroup()
        {
            Service.LoadOperationsGroups();
        }

        private void Show(Operation operation)
        {
            Frequence frequence = null;
            if (operation!=null)
            {
                ResetStandardEditableControls();

                _tbDescription.Text = operation.Description;
                _tbNewDescription.Text = operation.ShortDescription;
                _labDate.Content = operation.Date.ToString(Properties.strings.dateFormat);
                _labAmount.Content = operation.Amount;
                if (operation.Frequence != null)
                {
                    frequence = _observableFrequencies.First(f => f.Text == operation.Frequence.Text);
                }
                Importance importance = null;
                if (operation.Frequence != null)
                {
                    importance = _observableImportances.First(i => i.Text == operation.Importance.Text);
                }
                _cbFrequent.SelectedItem =  frequence;
                _cbImportance.SelectedItem = importance;
                _labTransaction.Content = operation.TransactionType.Text;
                _labTransfer.Content = operation.TransferType.Text;
                foreach (var tag in operation.Tags)
                {
                    SetTagLabel(tag.Tag);
                }

                SetUserStandardEditableControlsVisibility(Visibility.Visible);
                SetCloneOptionsVisibility(Visibility.Hidden);
            }
            else
            {
                ResetAllControls();
                SetCloneOptionsVisibility(Visibility.Hidden);
                SetNavigateButtonVisibility(Visibility.Hidden);
                SetUserStandardEditableControlsVisibility(Visibility.Hidden);
            }
        }

        private void ShowNext()
        {
            Show(ViewsMemory.NextOperation());
        }

        private void ShowPrevious()
        {
            Show(ViewsMemory.PreviousOperation());
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
                var newBorder = new Border
                {
                    Style = (Style)FindResource("MyBorder")
                };

                var newStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                var newLabel = new Label {Content = tag.ToString(), Style = (Style) FindResource("MyLabel")};

                var newButton = new Button
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
                    Style = (Style) FindResource("MyButton"),
                };
                newButton.Click += BtnRemoveTag_Click;

                newStackPanel.Children.Add(newLabel);
                newStackPanel.Children.Add(newButton);
                newBorder.Child = newStackPanel;
                _spTags.Children.Add(newBorder);
            }
        }

        public override string ToString()
        {
            return "ImportOperationsView";
        }

        #region events
        private void BtnImportAccept_Click(object sender, RoutedEventArgs e)
        {
            var operation = ViewsMemory.CurrentOperation();
            var tmp = _tbDescription.Text;
            operation.SetParent(_cbRelated.SelectedItem as OperationsGroup);
            operation.SetDescription(_tbNewDescription.Text);
            operation.SetShortDescription(_tbNewDescription.Text);

            if (operation.Parent != null)
            {
                operation.SetImportance(operation.Parent.Importance);
                operation.SetFrequence(operation.Parent.Frequence);
                operation.RemoveAllTags();
                foreach (var item in operation.Parent.Tags)
                {
                    operation.AddTag(item.Tag);
                }
            }
            else
            {
                operation.SetImportance(_cbImportance.SelectedItem as Importance);
                operation.SetFrequence(_cbFrequent.SelectedItem as Frequence);
                operation.RemoveAllTags();
                foreach (var item in _selectedTags)
                {
                    operation.AddTag(item);
                }
            }

            try
            {
                Service.UpdateOperationComplex(operation);
                ViewsMemory.RemoveImportingOperation(operation);
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, ex.Message);
                operation.SetDescription(tmp);
                dialog.ShowDialog();
            }
            Show(ViewsMemory.CurrentOperation());
        }

        private void BtnImportIgnore_Click(object sender, RoutedEventArgs e)
        {
            ViewsMemory.RemoveImportingOperation(ViewsMemory.CurrentOperation());
            Show(ViewsMemory.CurrentOperation());
        }

        private void CbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cbTags.SelectedItem is Tag selected)
            {
                SetTagLabel(selected);
            }
        }

        private void BtnRemoveTag_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var panel = button?.Parent as StackPanel;
            var border = panel?.Parent as Border;
            var label = panel?.Children[0] as Label;

            var tag = Service.Tags.First(t => t.Text.Equals(label?.Content));
            _spTags.Children.Remove(border);
            var tag2 = _selectedTags.First(t => t.Text == tag.Text);
            _selectedTags.Remove(tag2);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ShowPrevious();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            ShowNext();
        }

        private void BtnImportClone_Click(object sender, RoutedEventArgs e)
        {
            SetCloneOptionsVisibility(Visibility.Visible);
        }

        private void BtnImportCloneAccept_Click(object sender, RoutedEventArgs e)
        {
            var currentOperation = ViewsMemory.CurrentOperation();
            SetCloneOptionsVisibility(Visibility.Hidden);
            if (_upDownClone.Value.HasValue && _upDownClone.Value.Value >= decimal.Zero)
            {
                var difference = currentOperation.Amount - _upDownClone.Value.Value;
                currentOperation.SetAmount(_upDownClone.Value.Value);
                _labAmount.Content = currentOperation.Amount.ToString(CultureInfo.CurrentCulture);

                var operation = new Operation(null, null, Service.User, "", 0M, null, null, null, null, DateTime.Now, "");
                operation.SetDescription(currentOperation.Description);
                operation.SetShortDescription(currentOperation.ShortDescription);
                operation.SetTransaction(currentOperation.TransactionType);
                operation.SetTransfer(currentOperation.TransferType);
                operation.SetDate(currentOperation.Date);
                operation.SetFrequence(currentOperation.Frequence);
                operation.SetImportance(currentOperation.Importance);
                operation.SetTags(currentOperation.Tags);
                operation.SetTransfer(currentOperation.TransferType);

                if (difference >= decimal.Zero)
                {
                    operation.SetAmount(difference);
                    operation.SetTransaction(currentOperation.TransactionType);
                }
                else
                {
                    operation.SetAmount(-1 * difference);
                    var anotherTransaction = currentOperation.TransactionType.Text == Service.TransactionTypes[0].Text ? Service.TransactionTypes[1] : Service.TransactionTypes[0];
                    operation.SetTransaction(anotherTransaction);
                }

                ViewsMemory.InsertOperation(currentOperation, operation);
            }
        }

        private void BtnImportCloneCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCloneOptionsVisibility(Visibility.Hidden);
        }

        private void CbRelated_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cbRelated.SelectedItem != null)
            {
                ResetRelatedAttributes();
                SetRelatedAttributes();
                SetRelatedControlsAttributesVisibility(Visibility.Visible);
            } else
            {
                ResetRelatedAttributes();
                SetRelatedControlsAttributesVisibility(Visibility.Hidden);
            }
        }
        #endregion

        private void _btnGroup_Click(object sender, RoutedEventArgs e)
        {
            var uc =new AddGroupsView();
            ViewManager.AddUserControl(uc);
            ViewManager.OpenUserControl(uc);
        }

        private void _btnManual_Click(object sender, RoutedEventArgs e)
        {
            var uc = new ManuallyOperationsView();
            ViewManager.AddUserControl(uc);
            ViewManager.OpenUserControl(uc);
        }
    }
}

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
    /// Interaction logic for AddGroupsView.xaml
    /// </summary>
    public partial class AddGroupsView : IRepresentative
    {
        private OperationsGroup _group;
        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequency> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<Tag> _observableTags;

        public string RepresentativeName { get; set; } = Properties.strings.addGroupRN;

        public AddGroupsView()
        {
            InitializeComponent();
            ViewsMemory.AddedParameter += AddedParameterEvent;
            Background = Brushes.Azure;

            LoadAttributes();
            _group = new OperationsGroup(null, Service.User, "", null, null, DateTime.Now);

            SetEditableControls();
            SetOperationsGroupValues(_group);
            EditMode(false);
        }

        public AddGroupsView(OperationsGroup group)
        {
            InitializeComponent();
            ViewsMemory.AddedParameter += AddedParameterEvent;
            Background = Brushes.Azure;

            LoadAttributes();
            _group = group;

            SetEditableControls();
            SetOperationsGroupValues(_group);
            EditMode(true);
        }

        private void EditMode(bool value)
        {
            if (value)
            {
                RepresentativeName = $"{Properties.strings.editGroupRN} {_group.Description}";
                _btnManualClear.Visibility = Visibility.Hidden;
                _btnManualAccept.Visibility = Visibility.Hidden;
                _btnEditAccept.Visibility = Visibility.Visible;
            }
            else
            {
                RepresentativeName = Properties.strings.addGroupRN;
                _btnManualClear.Visibility = Visibility.Visible;
                _btnManualAccept.Visibility = Visibility.Visible;
                _btnEditAccept.Visibility = Visibility.Hidden;
            }
        }

        private void AddedParameterEvent(IParameter added)
        {
            throw new NotImplementedException();
        }

        private void SetOperationsGroupValues(OperationsGroup group)
        {
            _borderCalendar.Visibility = Visibility.Hidden;
            _calDate.SelectedDate = DateTime.Now;

            _group = group;

            ResetEditableControls();

            _tbNewDescription.Text = _group.Description;
            _labDate.Content = _group.Date.ToString(Properties.strings.dateFormat);
            if (_group.Frequency != null)
            {
                _cbFrequent.SelectedItem = _observableFrequencies.First(f => f.Text == _group.Frequency.Text);
            }
            if (_group.Importance != null)
            {
                _cbImportance.SelectedItem = _observableImportances.First(f => f.Text == _group.Importance.Text);
            }
            foreach (var tag in _group.Tags)
            {
                if (!tag.IsMarkForDeletion && !tag.IsDirty)
                {
                    SetTagLabel(tag.Tag);
                }
            }

            UserEditableControlsVisibility(Visibility.Visible);
        }

        private void SetEditableControls()
        {
            _observableFrequencies = new ObservableRangeCollection<Frequency>(Service.Frequencies);
            _cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            _cbImportance.ItemsSource = _observableImportances;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            _cbTags.ItemsSource = _observableTags;
        }

        private void ResetEditableControls()
        {
            _tbNewDescription.Text = "";
            _selectedTags = new List<Tag>();
            _cbTags.SelectedItem = null;
            _cbFrequent.SelectedItem = null;
            _cbImportance.SelectedItem = null;
            _spTags.Children.Clear();
        }

        private void UserEditableControlsVisibility(Visibility v)
        {
            _cbFrequent.Visibility = v;
            _cbImportance.Visibility = v;
            _cbTags.Visibility = v;
            _tbNewDescription.Visibility = v;
        }

        private void LoadAttributes()
        {
            Service.LoadAttributes();
        }

        private void BtnManualClear_Click(object sender, RoutedEventArgs e)
        {
            ResetEditableControls();
        }

        private void BtnManualAccept_Click(object sender, RoutedEventArgs e)
        {
            _group.SetImportance(_cbImportance.SelectedItem as Importance);
            _group.SetFrequence(_cbFrequent.SelectedItem as Frequency);
            _group.SetDate(DateTime.ParseExact(_labDate.Content.ToString(), Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture));
            UpdateOperationsGroupTags();
            _group.SetDescription(_tbNewDescription.Text);

            try
            {
                Service.UpdateOperationsGroupComplex(_group);
                ViewsMemory.AddedGroup?.Invoke(_group);
                ResetEditableControls();
                SetEditableControls();
                var group = new OperationsGroup(null, Service.User, "", null, null, DateTime.Now);
                SetOperationsGroupValues(group);
            }
            catch (Exception ex)
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
                if (item.Equals(newTag))
                {
                    return true;
                }
            }
            return false;
        }

        private void SetTagLabel(Tag tag)
        {
            if (IsExistInSelectedTags(tag)) return;

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
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Object = tag,
                Style = (Style) FindResource("MyButton"),
            };
            newButton.Click += BtnClose_Click;

            newStackPanel.Children.Add(newLabel);
            newStackPanel.Children.Add(newButton);
            newBorder.Child = newStackPanel;
            _spTags.Children.Add(newBorder);
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

        public override string ToString()
        {
            return "AddGroupView";
        }

        private void _btnEditAccept_Click(object sender, RoutedEventArgs e)
        {
            _group.SetImportance(_cbImportance.SelectedItem as Importance);
            _group.SetFrequence(_cbFrequent.SelectedItem as Frequency);
            _group.SetDate(DateTime.ParseExact(_labDate.Content.ToString(), Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture));
            UpdateOperationsGroupTags();
            _group.SetDescription(_tbNewDescription.Text);

            try
            {
                Service.UpdateOperationsGroupComplex(_group);
                ViewsMemory.AddedGroup?.Invoke(_group);
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.messageBoxModificationSuccess);
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, ex.Message);
                dialog.ShowDialog();
            }
        }

        private void UpdateOperationsGroupTags()
        {
            _group.Tags.ForEach(t => t.IsMarkForDeletion = true);
            foreach (var item in _selectedTags)
            {
                _group.AddTag(item);
            }
        }
    }
}

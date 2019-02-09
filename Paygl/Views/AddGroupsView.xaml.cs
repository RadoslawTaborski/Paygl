using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using Paygl.Models;
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
    /// Interaction logic for AddGroupsView.xaml
    /// </summary>
    public partial class AddGroupsView : UserControl, IRepresentative
    {
        private OperationsGroup _group;
        private List<Tag> _selectedTags;

        private ObservableRangeCollection<Frequence> _observableFrequencies;
        private ObservableRangeCollection<Importance> _observableImportances;
        private ObservableRangeCollection<Tag> _observableTags;

        public string RepresentativeName { get; set; } = "Dodaj grupę";

        public AddGroupsView()
        {
            InitializeComponent();
            ViewsMemory.AddedParameter += AddedParameterEvent;
            Background = Brushes.Azure;

            LoadAttributes();

            SetEditableControls();
            SetOperationValues();
        }

        private void AddedParameterEvent(IParameter added)
        {
            throw new NotImplementedException();
        }

        private void SetEditableControls()
        {
            _observableFrequencies = new ObservableRangeCollection<Frequence>(Service.Frequencies);
            _cbFrequent.ItemsSource = _observableFrequencies;
            _observableImportances = new ObservableRangeCollection<Importance>(Service.Importances);
            _cbImportance.ItemsSource = _observableImportances;
            _observableTags = new ObservableRangeCollection<Tag>(Service.Tags);
            _cbTags.ItemsSource = _observableTags;
        }

        private void SetOperationValues()
        {
            _borderCalendar.Visibility = Visibility.Hidden;
            _calDate.SelectedDate = DateTime.Now;

            _group = new OperationsGroup(null, Service.User, "", null, null, DateTime.Now);

            ResetEditableControls();

            _tbNewDescription.Text = "";
            _labDate.Content = _group.Date.ToString("dd.MM.yyyy");
            _cbFrequent.SelectedItem = _group.Frequence;
            _cbImportance.SelectedItem = _group.Importance;
            foreach (var tag in _group.Tags)
            {
                SetTagLabel(tag.Tag);
            }

            UserEditableControlsVisibility(Visibility.Visible);
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
            _group.SetImportance(_cbImportance.SelectedItem as Importance);
            _group.SetFrequence(_cbFrequent.SelectedItem as Frequence);
            _group.SetDate(DateTime.ParseExact(_labDate.Content.ToString(), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));
            foreach (var item in _selectedTags)
            {
                _group.AddTag(item);
            }
            _group.SetDescription(_tbNewDescription.Text);

            try
            {
                Service.UpdateOperationsGroupComplex(_group);
                ViewsMemory.AddedGroup?.Invoke(_group);
                ResetEditableControls();
                SetEditableControls();
                SetOperationValues();
            }
            catch (Exception ex)
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

        public override string ToString()
        {
            return "AddGroupView";
        }
    }
}

using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using Paygl.Models;
using PayglService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ShowOperations.xaml
    /// </summary>
    public partial class AnalysisView : UserControl, IRepresentative
    {
        private AnalysisViewItem _selectedView;

        private const int HEIGHT = 27;
        public const int VIEWBAR_BUTTON_WIDTH = 100;

        public string RepresentativeName { get; set; } = "Analiza";

        private List<AnalysisViewItem> _views;

        public AnalysisView()
        {
            InitializeComponent();
            _views = new List<AnalysisViewItem>();
            ViewsMemory.ChangeInFilters += ChangeInSettings;
            ViewsMemory.ChangeInAnalysisManager += ChangeInSettings;

            _borderCalendarFrom.Visibility = Visibility.Hidden;
            _borderCalendarTo.Visibility = Visibility.Hidden;

            Service.LoadAttributes();
            Service.LoadOperationsGroups();
            Service.LoadOperations();

            if (Service.Operations.Count != 0)
            {
                var endDate = Service.Operations.Last().Date;
                _tbFrom.Text = endDate.AddMonths(-1).AddDays(1).ToString("dd.MM.yyyy");
                _tbTo.Text = endDate.ToString("dd.MM.yyyy");
            }
            else
            {
                _tbFrom.Text = DateTime.Now.ToString("dd.MM.yyyy");
                _tbTo.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }

            ChangeInSettings();
        }

        private void ChangeInSettings()
        {
            _views.Clear();
            _viewBarStockPanel.Children.Clear();
            var filtersGroups = ViewsMemory.FiltersGroups;
            foreach (var filtersGroup in filtersGroups)
            {
                if (filtersGroup.Visibility)
                {
                    AddUserControl(new AnalysisViewItem(filtersGroup.Name, filtersGroup, _tbFrom.Text, _tbTo.Text));
                }
            }
            if (_views.Count > 0)
            {
                OpenUserControl(_views[0]);
            }
        }

        private void _btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            _selectedView.Show(_tbFrom.Text, _tbTo.Text);
        }

        private void _btnCalendarFrom_Click(object sender, RoutedEventArgs e)
        {
            _borderCalendarFrom.Visibility = Visibility.Visible;
        }

        private void _btnCalendarTo_Click(object sender, RoutedEventArgs e)
        {
            _borderCalendarTo.Visibility = Visibility.Visible;
        }

        private void _calDateFrom_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _borderCalendarFrom.Visibility = Visibility.Hidden;
            if (_calDateFrom.SelectedDate.HasValue)
            {
                _tbFrom.Text = _calDateFrom.SelectedDate.Value.ToString("dd.MM.yyyy");
            }
            _borderCalendarFrom.Visibility = Visibility.Hidden;
        }

        private void _calDateTo_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _borderCalendarTo.Visibility = Visibility.Hidden;
            if (_calDateTo.SelectedDate.HasValue)
            {
                _tbTo.Text = _calDateTo.SelectedDate.Value.ToString("dd.MM.yyyy");
            }
            _borderCalendarTo.Visibility = Visibility.Hidden;
        }

        private void _btnBack_Click(object sender, RoutedEventArgs e)
        {
            var endDate = DateTime.ParseExact(_tbTo.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            _tbFrom.Text = endDate.AddDays(1).AddMonths(-2).ToString("dd.MM.yyyy");
            _tbTo.Text = endDate.AddDays(1).AddMonths(-1).AddDays(-1).ToString("dd.MM.yyyy");
        }

        private void _btnNext_Click(object sender, RoutedEventArgs e)
        {
            var endDate = DateTime.ParseExact(_tbTo.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            _tbFrom.Text = endDate.AddDays(1).ToString("dd.MM.yyyy");
            _tbTo.Text = endDate.AddDays(1).AddMonths(1).AddDays(-1).ToString("dd.MM.yyyy");
        }

        private ButtonWithObject CreateViewBarButton(string name, string content, int width, object insideObject, bool isSelected, RoutedEventHandler operation)
        {
            var brush = (Brush)FindResource("MyAzure");
            var style = (Style)FindResource("MyButton");
            if (isSelected)
            {
                brush = (Brush)FindResource("MyWhite");
                style = (Style)FindResource("MyButtonBarView");
            }

            var grid = new Grid
            {
                Width = VIEWBAR_BUTTON_WIDTH,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush)FindResource("Transparent"),
            };

            var label = new Label
            {
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush)FindResource("Transparent"),
                Foreground = brush,
            };
            grid.Children.Add(label);

            var button = new ButtonWithObject
            {
                Name = name,
                Content = grid,
                Object = insideObject,
                Context = label,
            };
            button.Click += operation;
            if (!isSelected)
            {
                button.MouseEnter += MouseEnter_Event;
                button.MouseLeave += MouseLeave_Event;
            }
            button.Width = width;
            button.Style = style;

            return button;
        }

        private void MouseLeave_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            (button.Context as Label).Foreground = (Brush)FindResource("MyAzure");
        }

        private void MouseEnter_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            (button.Context as Label).Foreground = (Brush)FindResource("MyDarkGrey");
        }

        public void AddUserControl(AnalysisViewItem uc)
        {
            if (_views.Any(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative).RepresentativeName))
            {
                return;
            }
            _views.Add(uc);
        }

        public void OpenUserControl(UserControl uc)
        {
            _selectedView = _views.Where(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative).RepresentativeName).FirstOrDefault();
            brdMain.Child = _selectedView;
            UpdateViewBar(_selectedView);
        }

        public void RemoveUserControl(UserControl uc)
        {
            var userControl = _views.Where(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative).RepresentativeName).First();
            _views.Remove(userControl);
        }

        private void UpdateViewBar(UserControl selected)
        {
            _viewBarStockPanel.Children.Clear();
            Button btnView;

            foreach (var item in _views)
            {
                if ((item as IRepresentative).RepresentativeName == (selected as IRepresentative).RepresentativeName)
                {
                    btnView = CreateViewBarButton($"btn{item.ToString()}", (item as IRepresentative).RepresentativeName, VIEWBAR_BUTTON_WIDTH, item, true, BtnView_Click);
                }
                else
                {
                    btnView = CreateViewBarButton($"btn{item.ToString()}", (item as IRepresentative).RepresentativeName, VIEWBAR_BUTTON_WIDTH, item, false, BtnView_Click);
                }
                _viewBarStockPanel.Children.Add(btnView);
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject).Object as UserControl;
            OpenUserControl(uc);
        }

        public override string ToString()
        {
            return "AnalysisView";
        }
    }
}


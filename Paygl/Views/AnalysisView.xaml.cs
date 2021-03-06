﻿using Paygl.Models;
using PayglService.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ShowOperations.xaml
    /// </summary>
    public partial class AnalysisView : IRepresentative
    {
        private AnalysisViewItem _selectedView;

        public const int ViewbarButtonWidth = 100;

        public string RepresentativeName { get; set; } = Properties.strings.analyseRN;

        private readonly List<AnalysisViewItem> _views;

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
                _tbFrom.Text = endDate.AddMonths(-1).AddDays(1).ToString(Properties.strings.dateFormat);
                _tbTo.Text = endDate.ToString(Properties.strings.dateFormat);
            }
            else
            {
                _tbFrom.Text = DateTime.Now.ToString(Properties.strings.dateFormat);
                _tbTo.Text = DateTime.Now.ToString(Properties.strings.dateFormat);
            }
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
            _views.ForEach(v=>v.Show(_tbFrom.Text, _tbTo.Text));
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
                _tbFrom.Text = _calDateFrom.SelectedDate.Value.ToString(Properties.strings.dateFormat);
            }
            _borderCalendarFrom.Visibility = Visibility.Hidden;
        }

        private void _calDateTo_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _borderCalendarTo.Visibility = Visibility.Hidden;
            if (_calDateTo.SelectedDate.HasValue)
            {
                _tbTo.Text = _calDateTo.SelectedDate.Value.ToString(Properties.strings.dateFormat);
            }
            _borderCalendarTo.Visibility = Visibility.Hidden;
        }

        private void _btnBack_Click(object sender, RoutedEventArgs e)
        {
            var endDate = DateTime.ParseExact(_tbTo.Text, Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            _tbFrom.Text = endDate.AddDays(1).AddMonths(-2).ToString(Properties.strings.dateFormat);
            _tbTo.Text = endDate.AddDays(1).AddMonths(-1).AddDays(-1).ToString(Properties.strings.dateFormat);
        }

        private void _btnNext_Click(object sender, RoutedEventArgs e)
        {
            var endDate = DateTime.ParseExact(_tbTo.Text, Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            _tbFrom.Text = endDate.AddDays(1).ToString(Properties.strings.dateFormat);
            _tbTo.Text = endDate.AddDays(1).AddMonths(1).AddDays(-1).ToString(Properties.strings.dateFormat);
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
                Width = width,
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
            ((Label) button.Context).Foreground = (Brush)FindResource("MyAzure");
        }

        private void MouseEnter_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            ((Label) button.Context).Foreground = (Brush)FindResource("MyDarkGrey");
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
            _selectedView = _views.FirstOrDefault(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            brdMain.Child = _selectedView;
            UpdateViewBar(_selectedView);
        }

        public void RemoveUserControl(UserControl uc)
        {
            var userControl = _views.First(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            _views.Remove(userControl);
        }

        private void UpdateViewBar(UserControl selected)
        {
            _viewBarStockPanel.Children.Clear();

            var barWidth = _viewBarStockPanel.ActualWidth;
            var defaultButtonWidth = ViewbarButtonWidth;
            if (_views.Count * defaultButtonWidth > barWidth)
            {
                defaultButtonWidth = (int)barWidth / _views.Count;
            }

            foreach (var item in _views)
            {
                MyButton btnView;
                if (((IRepresentative) item).RepresentativeName == (selected as IRepresentative)?.RepresentativeName)
                {
                    btnView = CreateViewBarButton($"btn{item}", ((IRepresentative) item).RepresentativeName, defaultButtonWidth, item, true, BtnView_Click);
                }
                else
                {
                    btnView = CreateViewBarButton($"btn{item}", ((IRepresentative) item).RepresentativeName, defaultButtonWidth, item, false, BtnView_Click);
                }
                _viewBarStockPanel.Children.Add(btnView);
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject)?.Object as UserControl;
            OpenUserControl(uc);
        }

        public override string ToString()
        {
            return "AnalysisView";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeInSettings();
        }
    }
}


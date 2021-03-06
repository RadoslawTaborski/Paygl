﻿using Paygl.Models;
using PayglService.cs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PayglService.Models;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for FiltersManager.xaml
    /// </summary>
    public partial class FiltersManager : IRepresentative
    {
        private const int RefHeight = 27;
        public string RepresentativeName { get; set; } = Properties.strings.filtersRN;

        public FiltersManager()
        {
            InitializeComponent();
            LoadFilters();
            ViewsMemory.ChangeInFilters += ChangeInFilters;
        }

        private void ChangeInFilters()
        {
            LoadFilters();
        }

        private void LoadFilters()
        {
            _spDisplay.Children.Clear();

            foreach (var elem in ViewsMemory.Filters)
            {
                _spDisplay.Children.Add(FilterToStackPanel(elem));
            }
        }

        private UIElement FilterToStackPanel(Filter filter)
        {
            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 0),
            };

            var borderGroup = new Border
            {
                Style = (Style)FindResource("MyBorderMedium"),
                BorderThickness = new Thickness(1, 1, 1, 1),
                Height = RefHeight,
            };
            borderGroup.Child = FilterHeaderToStackPanel(filter);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, filter, borderGroup.Child, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInFilter));

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var editTextBox = new TextBox
            {
                Style = (Style)FindResource("MyTextBox"),
                Text = $"{filter.Description}: {filter.Query}",
                FontSize = 13,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                IsEnabled = false,
            };

            var button = new ButtonWithObject
            {
                Style = (Style)FindResource("MyButton"),
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\edit-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                },
                Height = 20,
                Width = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Object = filter,
            };

            button.Click += OpenEditMode;

            var button2 = new ButtonWithObject
            {
                Style = (Style)FindResource("MyButton"),
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\x-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                },
                Height = 20,
                Width = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Object = filter,
            };

            button2.Click += RemoveFilter;

            stackPanel.Children.Add(button2);
            stackPanel.Children.Add(button);

            stackPanel.Children.Add(editTextBox);
            result.Children.Add(stackPanel);

            result.Children[1].Visibility = Visibility.Collapsed;

            return result;
        }

        private void ClickInFilter(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonWithObject;
            if (button?.Parent is StackPanel parent)
            {
                parent.Children[1].Visibility = parent.Children[1].Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        private void OpenEditMode(object sender, RoutedEventArgs e)
        {
            var button = (sender as ButtonWithObject);
            var filter = button?.Object as Filter;
            var view = new ShowOperations(filter);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }

        private UIElement FilterHeaderToStackPanel(Filter filter)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight + 3,
            };

            var borderDescription = CreateBorderWithLabel($"{filter.Description}");

            resultStackPanel.Children.Add(borderDescription);

            return resultStackPanel;
        }

        private void RemoveFilter(object sender, RoutedEventArgs e)
        {
            var filter = (sender as ButtonWithObject)?.Object as Filter;
            ViewsMemory.Filters.Remove(filter);
            Service.SetSettings(ViewsMemory.Filters);
            Service.SaveSettings();

            LoadFilters();

            e.Handled = true;
        }

        private Border CreateBorderWithLabel(string text)
        {
            return new Border
            {
                Child = new Label
                {
                    Content = text,
                    Style = (Style)FindResource("MyLabel"),
                    Margin = new Thickness(0, -5, 0, 0),
                    Height = RefHeight,
                    VerticalAlignment = VerticalAlignment.Stretch,
                },
                BorderBrush = (SolidColorBrush)FindResource("MyLight"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        private ButtonWithObject CreateButtonWithBorderContent(Border border, object objectForButton, object context, string colorName, Thickness margin, RoutedEventHandler method)
        {
            var result = new ButtonWithObject
            {
                Content = border,
                Margin = margin,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Style = (Style)FindResource("MyButtonLeft"),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Object = objectForButton,
                Context = context,
                Background = (Brush)FindResource(colorName),
            };

            result.Click += method;

            return result;
        }

        public override string ToString()
        {
            return "FiltersManager";
        }
    }
}

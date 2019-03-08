using Paygl.Models;
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
    /// Interaction logic for AnalysisManager.xaml
    /// </summary>
    public partial class AnalysisManager : IRepresentative
    {
        private const int RefHeight = 27;
        public string RepresentativeName { get; set; } = Properties.strings.analysisManagerRN;

        public AnalysisManager()
        {
            InitializeComponent();
            LoadFiltersGroups();
            ViewsMemory.EditedAnalysisView += ChangeInFiltersGroups;
        }

        private void ChangeInFiltersGroups()
        {
            LoadFiltersGroups();
        }

        private void LoadFiltersGroups()
        {
            _spDisplay.Children.Clear();

            foreach (var elem in ViewsMemory.FiltersGroups)
            {
                _spDisplay.Children.Add(GroupToStackPanel(elem));
            }
        }

        private UIElement GroupToStackPanel(FiltersGroup groups)
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
            borderGroup.Child = GroupHeaderToStackPanel(groups);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, groups, borderGroup.Child, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInGroup));

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            foreach (var item in groups.Items)
            {
                switch (item.Key)
                {
                    case FiltersGroup key:
                        stackPanel.Children.Add(GroupToBorder(key));
                        break;
                    case Filter filter:
                        stackPanel.Children.Add(FilterToBorder(filter));
                        break;
                }
            }
            
            result.Children.Add(stackPanel);

            result.Children[1].Visibility = Visibility.Collapsed;

            return result;
        }

        private UIElement GroupToBorder(FiltersGroup group)
        {
            var border = new Border
            {
                Style = (Style)FindResource("MyBorderMedium"),
                BorderThickness = new Thickness(0, 0, 0, 0),
                Height = RefHeight - 5,
                Margin = new Thickness(10, 0, 0, 0),
            };

            var button = new Button
            {
                Style = (Style)FindResource("MyButtonLeft"),
                Content = group.Name,
                Height = RefHeight - 5,
            };

            border.Child = button;

            return border;
        }

        private UIElement FilterToBorder(Filter filter)
        {
            var border = new Border
            {
                Style = (Style)FindResource("MyBorderMedium"),
                BorderThickness = new Thickness(0, 0, 0, 0),
                Height = RefHeight - 5,
                Margin = new Thickness(10, 0, 0, 0),
            };

            var button = new Button
            {
                Style = (Style)FindResource("MyButtonLeft"),
                Content = filter.Description,
                Height = RefHeight - 5,
            };

            border.Child = button;

            return border;
        }

        private void ClickInGroup(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is CheckBoxWithObject) return;

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
            var group = button?.Object as FiltersGroup;
            var view = new EditFiltersGroup(group);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }

        private UIElement GroupHeaderToStackPanel(FiltersGroup group)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight + 3,
            };

            var checkbox = new CheckBoxWithObject
            {
                Object = group,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 20,
                Width = 20,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                IsChecked = group.Visibility,
            };

            checkbox.Checked += Checkbox_Changed;
            checkbox.Unchecked += Checkbox_Changed;

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
                Object = group,
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
                Object = group,
            };

            button2.Click += RemoveGroup;

            resultStackPanel.Children.Add(button2);
            resultStackPanel.Children.Add(button);
            resultStackPanel.Children.Add(checkbox);

            var borderDescription = CreateBorderWithLabel($"{group.Name}");

            resultStackPanel.Children.Add(borderDescription);

            return resultStackPanel;
        }

        private void Checkbox_Changed(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBoxWithObject)sender;
            (checkbox.Object as FiltersGroup)?.SetVisibility(checkbox.IsChecked != null && checkbox.IsChecked.Value);
            Service.SetSettings(ViewsMemory.FiltersGroups);
            Service.SaveSettings();
            ViewsMemory.ChangeInAnalysisManager?.Invoke();
            e.Handled = true;
        }

        private void RemoveGroup(object sender, RoutedEventArgs e)
        {
            var group = (sender as ButtonWithObject)?.Object as FiltersGroup;
            ViewsMemory.FiltersGroups.Remove(group);
            Service.SetSettings(ViewsMemory.FiltersGroups);
            Service.SaveSettings();
            ViewsMemory.ChangeInAnalysisManager?.Invoke();

            LoadFiltersGroups();

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
            return "AnalysisManager";
        }

        private void _btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var view = new EditFiltersGroup();
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);
        }
    }
}

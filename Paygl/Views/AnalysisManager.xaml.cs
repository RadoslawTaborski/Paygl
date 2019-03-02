using Paygl.Models;
using PayglService.cs;
using PayglService.Models;
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
    /// Interaction logic for AnalysisManager.xaml
    /// </summary>
    public partial class AnalysisManager : UserControl, IRepresentative
    {
        private const int HEIGHT = 27;
        public string RepresentativeName { get; set; } = "Widoki";

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
                Height = HEIGHT,
            };
            borderGroup.Child = GroupHeaderToStackPanel(groups, result);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, groups, borderGroup.Child, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInGroup));

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            foreach(var item in groups.Filters)
            {                
                stackPanel.Children.Add(FilterToBorder(item));
            }
            
            result.Children.Add(stackPanel);

            result.Children[1].Visibility = Visibility.Collapsed;

            return result;
        }

        private UIElement FilterToBorder(Filter filter)
        {
            var border = new Border
            {
                Style = (Style)FindResource("MyBorderMedium"),
                BorderThickness = new Thickness(0, 0, 0, 0),
                Height = HEIGHT - 5,
                Margin = new Thickness(10, 0, 0, 0),
            };

            var button = new Button
            {
                Style = (Style)FindResource("MyButtonLeft"),
                Content = filter.Description,
                Height = HEIGHT - 5,
            };

            border.Child = button;

            return border;
        }

        private void ClickInGroup(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonWithObject;
            var parameter = button.Object;
            var stackPanel = button.Context as StackPanel;
            var parent = button.Parent as StackPanel;
            parent.Children[1].Visibility = parent.Children[1].Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OpenEditMode(object sender, RoutedEventArgs e)
        {
            var button = (sender as ButtonWithObject);
            var group = button.Object as FiltersGroup;
            var view = new EditFiltersGroup(group);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }

        private UIElement GroupHeaderToStackPanel(FiltersGroup groups, StackPanel main)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT + 3,
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
                Object = groups,
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
                Object = groups,
            };

            button2.Click += RemoveGroup;

            resultStackPanel.Children.Add(button2);
            resultStackPanel.Children.Add(button);

            var borderDescription = CreateBorderWithLabel($"{groups.Name}");

            resultStackPanel.Children.Add(borderDescription);

            return resultStackPanel;
        }

        private void RemoveGroup(object sender, RoutedEventArgs e)
        {
            var group = (sender as ButtonWithObject).Object as FiltersGroup;
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
                    Height = HEIGHT,
                    VerticalAlignment = VerticalAlignment.Stretch,
                },
                BorderBrush = (SolidColorBrush)FindResource("MyLight"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = HEIGHT,
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

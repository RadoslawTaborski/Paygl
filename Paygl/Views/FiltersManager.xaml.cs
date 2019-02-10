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
    /// Interaction logic for FiltersManager.xaml
    /// </summary>
    public partial class FiltersManager : UserControl, IRepresentative
    {
        private const int HEIGHT = 27;
        public string RepresentativeName { get; set; } = "Filtry";

        public FiltersManager()
        {
            InitializeComponent();

            var queries = Service.ReadQuery();

            var filters = new List<Filter>();

            foreach (var item in queries)
            {
                var newFilter = new Filter(item.Key, item.Value);
                filters.Add(newFilter);
            }

            foreach (var item in filters)
            {
                _spDisplay.Children.Add(FilterToStackPanel(item));
            }
        }

        private UIElement FilterToStackPanel(Filter group)
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
            borderGroup.Child = FilterHeaderToStackPanel(group, result);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, group, borderGroup.Child, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInFilter));

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
                Text = $"{group.Description}: {group.Query.ToString()}",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0),
            };
            stackPanel.Children.Add(editTextBox);
            result.Children.Add(stackPanel);

            result.Children[1].Visibility = Visibility.Collapsed;

            return result;
        }

        private void ClickInFilter(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("click edit");
            var button = sender as ButtonWithObject;
            var parameter = button.Object;
            var stackPanel = button.Context as StackPanel;
            var parent = button.Parent as StackPanel;
            parent.Children[1].Visibility = parent.Children[1].Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            stackPanel.Children[1].Visibility = stackPanel.Children[1].Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AcceptQuery(object sender, RoutedEventArgs e)
        {
            var button = (sender as ButtonWithObject);
            var filter = button.Object as Filter;
            var stackPanel = button.Context as StackPanel;
            var text = ((stackPanel.Children[1] as StackPanel).Children[0] as TextBox).Text;

            var substrings = text.Split(':');
            if (substrings.Count() != 2)
            {
                var dialog = new MessageBox("Komunikat", "Zły format zapytania");
                dialog.ShowDialog();
                return;
            }
            try
            {
                if (substrings[1].IndexOf("TransferType") == -1)
                {

                    filter.SetQuery(Analyzer.Analyzer.StringToQuery(substrings[1]));
                }
                else
                {
                    filter.SetQuery(Analyzer.Analyzer.StringToQuery(substrings[1], true));
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox("Komunikat", "Zły format zapytania");
                dialog.ShowDialog();
            }
            filter.SetDescription(substrings[0]);
            button.Visibility = Visibility.Collapsed;
            stackPanel.Children[1].Visibility = Visibility.Collapsed;

            e.Handled = true;
        }

        private UIElement FilterHeaderToStackPanel(Filter group, StackPanel main)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT + 3,
            };

            var borderDescription = CreateBorderWithLabel($"{group.Description}");

            var button = new ButtonWithObject
            {
                Style = (Style)FindResource("MyButton"),
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\confirm.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                },
                Height = 20,
                Width = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Object = group,
                Context = main,
            };

            button.Click += AcceptQuery;

            resultStackPanel.Children.Add(borderDescription);
            resultStackPanel.Children.Add(button);
            resultStackPanel.Children[1].Visibility = Visibility.Collapsed;

            return resultStackPanel;
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
            return "FiltersManager";
        }
    }
}

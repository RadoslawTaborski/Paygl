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
    /// Interaction logic for EditFiltersGroup.xaml
    /// </summary>
    public partial class EditFiltersGroup : UserControl, IRepresentative
    {
        private const int HEIGHT = 27;
        public string RepresentativeName { get; set; } = "";
        List<CheckBoxWithObject> checkboxes = new List<CheckBoxWithObject>();
        private FiltersGroup _filtersGroup;
        private int _counter = 0;

        public EditFiltersGroup()
        {
            InitializeComponent();
            var group = new FiltersGroup("");
            RepresentativeName = "Dodawanie widoku";
            Init(group);
        }

        public EditFiltersGroup(FiltersGroup group)
        {
            InitializeComponent();
            RepresentativeName = $"Edycja: {group.Name}";
            Init(group);
        }

        public void Init(FiltersGroup group)
        {
            ViewsMemory.ChangeInFilters += ChangeInFilters;
            _filtersGroup = group;
            LoadFiltersAndFiltersGroups();
            _lblName.Text = group.Name;
            foreach (var item in group.Items)
            {
                if (item.Key is FiltersGroup)
                {
                    var tmp = checkboxes.Where(c => ((KeyValuePair<IFilter, int>)c.Object).Key is FiltersGroup && ((FiltersGroup)((KeyValuePair<IFilter, int>)c.Object).Key).Name == ((FiltersGroup)item.Key).Name).FirstOrDefault();
                    if (tmp != null)
                    {
                        tmp.IsChecked = true;
                        tmp.Content = item.Value.ToString();
                    }
                }
                else if (item.Key is Filter)
                {
                    var tmp = checkboxes.Where(c => ((KeyValuePair<IFilter,int>)c.Object).Key is Filter && ((Filter)((KeyValuePair<IFilter, int>)c.Object).Key).Description == ((Filter)item.Key).Description).FirstOrDefault();
                    if (tmp != null)
                    {
                        tmp.IsChecked = true;
                        tmp.Content = item.Value.ToString();
                    }
                }
            }
        }

        private void ChangeInFilters()
        {
            LoadFiltersAndFiltersGroups();
        }

        private void LoadFiltersAndFiltersGroups()
        {
            checkboxes.Clear();
            _spDisplay.Children.Clear();

            foreach (var elem in ViewsMemory.FiltersGroups)
            {
                if (elem != _filtersGroup) //TODO: && elem.ChildGroups.Count()==0 maybe it is not necessary
                {
                    _spDisplay.Children.Add(FiltersGroupToStackPanel(new KeyValuePair<IFilter, int>(elem, 0)));
                }
            }

            foreach (var elem in ViewsMemory.Filters)
            {
                _spDisplay.Children.Add(FilterToStackPanel(new KeyValuePair<IFilter, int>(elem, 0)));
            }
        }

        private UIElement FiltersGroupToStackPanel(KeyValuePair<IFilter, int> groupKV)
        {
            var group = (FiltersGroup)groupKV.Key;
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
            borderGroup.Child = FiltersGroupHeaderToStackPanel(groupKV, result);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, group, borderGroup.Child, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInFilter));

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            foreach (var item in group.Items)
            {
                if (item.Key is Filter)
                {
                    stackPanel.Children.Add(FilterToBorder((Filter)item.Key));
                }
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

        private UIElement FilterToStackPanel(KeyValuePair<IFilter, int> filterKV)
        {
            var filter = (Filter)filterKV.Key;
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
            borderGroup.Child = FilterHeaderToStackPanel(filterKV, result);
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
                Text = $"{filter.Description}: {filter.Query.ToString()}",
                FontSize = 13,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0),
                IsEnabled = false,
            };

            stackPanel.Children.Add(editTextBox);
            result.Children.Add(stackPanel);

            result.Children[1].Visibility = Visibility.Collapsed;

            return result;
        }

        private void ClickInFilter(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is CheckBoxWithObject))
            {
                var button = sender as ButtonWithObject;
                var parameter = button.Object;
                var stackPanel = button.Context as StackPanel;
                var parent = button.Parent as StackPanel;
                parent.Children[1].Visibility = parent.Children[1].Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void OpenEditMode(object sender, RoutedEventArgs e)
        {
            var button = (sender as ButtonWithObject);
            var filter = button.Object as Filter;
            var view = new ShowOperations(filter);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }

        private UIElement FilterHeaderToStackPanel(KeyValuePair<IFilter,int> filter, StackPanel main)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT,
            };

            var checkbox = new CheckBoxWithObject
            {
                Object = filter,
                Content = filter.Value.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
            };

            checkbox.Checked += Checkbox_Checked;
            checkbox.Unchecked += Checkbox_Unchecked;
            var border = CreateBorderWithLabel(((Filter)filter.Key).Description);
            checkboxes.Add(checkbox);

            resultStackPanel.Children.Add(checkbox);
            resultStackPanel.Children.Add(border);

            return resultStackPanel;
        }

        private UIElement FiltersGroupHeaderToStackPanel(KeyValuePair<IFilter,int> group, StackPanel main)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT,
            };

            var checkbox = new CheckBoxWithObject
            {
                Object = group,
                Content = group.Value.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
            };

            checkbox.Checked += Checkbox_Checked;
            checkbox.Unchecked += Checkbox_Unchecked;
            var border = CreateBorderWithLabel(((FiltersGroup)group.Key).Name);
            checkboxes.Add(checkbox);

            resultStackPanel.Children.Add(checkbox);
            resultStackPanel.Children.Add(border);

            return resultStackPanel;
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            _counter--;
            var checkbox = (CheckBoxWithObject)sender;
            checkbox.Content = 0.ToString();
            e.Handled = true;
        }

        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            _counter++;
            var checkbox = (CheckBoxWithObject)sender;
            checkbox.Content = _counter.ToString();
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
            return "FiltersManager";
        }

        private void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            _filtersGroup.SetName(_lblName.Text);
            _filtersGroup.Items.Clear();
            foreach (var item in checkboxes)
            {
                if (item.IsChecked.Value == true)
                {
                    var filter = ((KeyValuePair<IFilter,int>)item.Object).Key;
                    _filtersGroup.AddFilter(new KeyValuePair<IFilter, int>(filter, int.Parse((string)item.Content)));
                }
            }

            bool alreadyExist = ViewsMemory.FiltersGroups.Contains(_filtersGroup);
            if (!alreadyExist)
            {
                ViewsMemory.FiltersGroups.Add(_filtersGroup);
            }

            ViewManager.RemoveUserControl(this);
            Service.SetSettings(ViewsMemory.FiltersGroups);
            Service.SaveSettings();
            ViewsMemory.EditedAnalysisView?.Invoke();
            ViewsMemory.ChangeInAnalysisManager?.Invoke();
        }
    }
}

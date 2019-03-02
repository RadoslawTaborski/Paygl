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
            foreach(var item in group.Filters)
            {
                var tmp = checkboxes.Where(c => c.Object is Filter && ((Filter)c.Object).Description == item.Description).FirstOrDefault();
                if(tmp != null)
                {
                    tmp.IsChecked = true;
                }
            }
            foreach (var item in group.ChildGroups)
            {
                var tmp = checkboxes.Where(c => c.Object is FiltersGroup && ((FiltersGroup)c.Object).Name == item.Name).FirstOrDefault();
                if (tmp != null)
                {
                    tmp.IsChecked = true;
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
                if (elem != _filtersGroup && elem.ChildGroups.Count()==0)
                {
                    _spDisplay.Children.Add(FiltersGroupToStackPanel(elem));
                }
            }

            foreach (var elem in ViewsMemory.Filters)
            {
                _spDisplay.Children.Add(FilterToStackPanel(elem));
            }
        }

        private UIElement FiltersGroupToStackPanel(FiltersGroup group)
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
            borderGroup.Child = FiltersGroupHeaderToStackPanel(group, result);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, group, borderGroup.Child, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInFilter));

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 0)
            };

            foreach (var item in group.Filters)
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
                Height = HEIGHT,
            };
            borderGroup.Child = FilterHeaderToStackPanel(filter, result);
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

        private UIElement FilterHeaderToStackPanel(Filter filter, StackPanel main)
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
                VerticalAlignment=VerticalAlignment.Center,
            };

            checkbox.Checked += Checkbox_Changed;
            checkbox.Unchecked += Checkbox_Changed;
            var border = CreateBorderWithLabel(filter.Description);
            checkboxes.Add(checkbox);

            resultStackPanel.Children.Add(checkbox);
            resultStackPanel.Children.Add(border);

            return resultStackPanel;
        }

        private UIElement FiltersGroupHeaderToStackPanel(FiltersGroup group, StackPanel main)
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
                VerticalAlignment = VerticalAlignment.Center,
            };

            checkbox.Checked += Checkbox_Changed;
            checkbox.Unchecked += Checkbox_Changed;
            var border = CreateBorderWithLabel(group.Name);
            checkboxes.Add(checkbox);

            resultStackPanel.Children.Add(checkbox);
            resultStackPanel.Children.Add(border);

            return resultStackPanel;
        }

        private void Checkbox_Changed(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBoxWithObject)sender;
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
            _filtersGroup.Filters.Clear();
            foreach(var item in checkboxes)
            {
                if (item.IsChecked.Value==true)
                {
                    if (item.Object is Filter)
                    {
                        var filter = (Filter)item.Object;
                        _filtersGroup.Filters.Add(filter);
                    }
                    else if(item.Object is FiltersGroup)
                    {
                        var group = (FiltersGroup)item.Object;
                        _filtersGroup.ChildGroups.Add(group);
                    }
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

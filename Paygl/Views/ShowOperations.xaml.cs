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

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ShowOperations.xaml
    /// </summary>
    public partial class ShowOperations : UserControl
    {
        private List<Operation> _operations;
        private const int HEIGHT = 25;
        public ShowOperations()
        {
            InitializeComponent();

            Service.LoadAttributes();
            Service.LoadOperationsGroups();
            Service.LoadOperations();

            _borderCalendarFrom.Visibility = Visibility.Hidden;
            _borderCalendarTo.Visibility = Visibility.Hidden;

            _tbFrom.Text = Service.Operations.First().Date.ToString("dd.MM.yyyy");
            _tbTo.Text = Service.Operations.Last().Date.ToString("dd.MM.yyyy");
        }

        private bool OperationHasTag(Operation operation, Tag tag)
        {
            foreach (var item in operation.Tags)
            {
                if (item.Tag.Text == tag.Text)
                {
                    return true;
                }
            }

            return false;
        }

        private bool OperationHasFrequence(Operation operation, Frequence frequence)
        {

            if (operation.Frequence.Text == frequence.Text)
            {
                return true;
            }

            return false;
        }

        private void _btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt1 = DateTime.Parse(_tbFrom.Text);
            DateTime dt2 = DateTime.Parse(_tbTo.Text);
            _operations = Service.Operations.Where(o => o.Parent == null && o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();

            var queries = Service.ReadQuery();

            //var groups = new List<OperationsGroup>(Service.OperationsGroups);
            var groups = new List<Group>();

            foreach (var elem in Service.OperationsGroups)
            {
                elem.UpdateAmount(Service.TransactionTypes);
            }

            foreach (var item in queries)
            {
                var operations = Analyzer.Analyzer.FilterOperations(_operations.OfType<IOperation>().ToList<IOperation>(), item.Value);
                var operationsGroups = Analyzer.Analyzer.FilterOperations(Service.OperationsGroups.OfType<IOperation>().ToList<IOperation>().Where(o => o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList(), item.Value);

                var newGroup = new Group(item.Key);
                newGroup.AddRange(operations);
                newGroup.AddRange(operationsGroups);

                groups.Add(newGroup);
            }

            _spDisplay.Children.Clear();
            foreach (var item in groups)
            {
                item.UpdateAmount();
                _spDisplay.Children.Add(GroupToStackPanel(item));
            }

            _labSum.Content = SumGroups(groups);
        }

        private UIElement GroupToStackPanel(Group group)
        {
            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 20),
            };

            var borderGroup = new Border
            {
                Style = (Style)FindResource("MyBorderMedium"),
                BorderThickness = new Thickness(1, 1, 1, 1),
                Height = HEIGHT + 10,
            };
            borderGroup.Child = GroupHeaderToStackPanel(group);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, group, result, "MyMediumGrey", new Thickness(0,0,0,0), ClickInGroup));

            return result;
        }

        private void ClickInGroup(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("click group");
            var button = sender as ButtonWithObject;
            var group = (button.Object as Group);
            var context = (button.Context as StackPanel);
            if (context.Children.Count == 1)
            {
                foreach (var item in group.Operations)
                {
                    if (item is OperationsGroup)
                    {
                        var i = item as OperationsGroup;
                        var childStackPanel = new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Margin = new Thickness(0, 0, 0, 5),
                        };
                        var border = new Border
                        {
                            Style = (Style)FindResource("MyBorderMedium"),
                            BorderThickness = new Thickness(1, 1, 1, 1),
                            Height = HEIGHT,
                        };
                        border.Child = IOperationToStackPanel(i);
                        childStackPanel.Children.Add(CreateButtonWithBorderContent(border, item, childStackPanel, "MyMediumGrey", new Thickness(25, 0, 0, 0), ClickInOperationsGroup));

                        context.Children.Add(childStackPanel);
                    }
                    if (item is Operation)
                    {
                        var i = item as Operation;
                        var border = new Border
                        {
                            Style = (Style)FindResource("MyBorderMedium"),
                            BorderThickness = new Thickness(1, 1, 1, 1),
                            Height = HEIGHT,
                            Child = IOperationToStackPanel(i),
                        };
                        context.Children.Add(CreateButtonWithBorderContent(border, item, null, "MyMediumGrey", new Thickness(25, 0, 0, 0), ClickInOperation));
                    }
                }
            }
            else
            {
                System.Collections.IList list = context.Children;
                for (int i1 = 0; i1 < list.Count; i1++)
                {
                    if (i1 != 0)
                    {
                        object item = list[i1];
                        var i = item as UIElement;
                        if (i.Visibility == Visibility.Visible)
                        {
                            i.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            i.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void ClickInOperationsGroup(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("click operations group");
            var button = sender as ButtonWithObject;
            var group = (button.Object as OperationsGroup);
            var context = (button.Context as StackPanel);
            if (context.Children.Count == 1)
            {
                foreach (var elem in group.Operations)
                {
                    var border2 = new Border
                    {
                        Style = (Style)FindResource("MyBorderMedium"),
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        Height = HEIGHT,
                    };
                    border2.Child = IOperationToStackPanel(elem);
                    context.Children.Add(CreateButtonWithBorderContent(border2, elem, null, "MyMediumGrey", new Thickness(50, 0, 0, 0), ClickInOperation));
                }
            } else
            {
                System.Collections.IList list = context.Children;
                for (int i1 = 0; i1 < list.Count; i1++)
                {
                    if (i1 != 0)
                    {
                        object item = list[i1];
                        var i = item as UIElement;
                        if (i.Visibility == Visibility.Visible)
                        {
                            i.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            i.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void ClickInOperation(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("click operation");
            var parameter = ((sender as ButtonWithObject).Object as Operation);
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
                Object=objectForButton,
                Context = context,
                Background = (Brush)FindResource(colorName),
            };

            result.Click += method;

            return result;
        }

        private UIElement IOperationToStackPanel(IOperation operation)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT,
            };

            var borderDate = CreateBorderWithLabel($"{operation.Date.ToString("dd.MM.yyyy")}:");
            var borderAmount = CreateBorderWithLabel($"{operation.Amount}");
            var borderDescription = CreateBorderWithLabel($"{operation.Description}");
            var borderTransactionType = CreateBorderWithLabel($"{operation.TransactionType}");
            var borderImportance = CreateBorderWithLabel($"{operation.Importance}");
            var borderFrequence = CreateBorderWithLabel($"{operation.Frequence}");

            resultStackPanel.Children.Add(borderDate);
            resultStackPanel.Children.Add(borderAmount);
            resultStackPanel.Children.Add(borderDescription);
            resultStackPanel.Children.Add(borderTransactionType);
            resultStackPanel.Children.Add(borderImportance);
            resultStackPanel.Children.Add(borderFrequence);
            foreach (var item in operation.Tags)
            {
                resultStackPanel.Children.Add(CreateBorderWithLabel($"{item}"));
            }

            return resultStackPanel;
        }

        private UIElement GroupHeaderToStackPanel(Group group)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT + 5,
            };

            var borderAmount = CreateBorderWithLabel($"{group.Amount}");
            var borderDescription = CreateBorderWithLabel($"{group.Description}");

            resultStackPanel.Children.Add(borderDescription);
            resultStackPanel.Children.Add(borderAmount);

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
                },
                BorderBrush = (SolidColorBrush)FindResource("MyLight"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = HEIGHT,
            };
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

        private decimal SumGroups(List<Group> groups)
        {
            var result = decimal.Zero;
            var operations = new List<Operation>();

            foreach (var item in groups)
            {
                foreach (var elem in item.Operations)
                {
                    if (elem is OperationsGroup)
                    {
                        var group = elem as OperationsGroup;
                        foreach (var operation in group.Operations)
                        {
                            operations.Add(operation);
                        }
                    }
                    if (elem is Operation)
                    {
                        operations.Add(elem as Operation);
                    }
                }
            }

            operations = operations.ToArray().Distinct().ToList();

            foreach (var operation in operations)
            {
                if (operation.TransactionType.Text == "przychód")
                {
                    result += operation.Amount;
                }
                if (operation.TransactionType.Text == "wydatek")
                {
                    result -= operation.Amount;
                }
            }

            return result;
        }
    }
}

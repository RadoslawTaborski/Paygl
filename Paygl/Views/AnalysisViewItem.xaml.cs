using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using Paygl.Models;
using PayglService.cs;
using PayglService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ShowOperations.xaml
    /// </summary>
    public partial class AnalysisViewItem : UserControl, IRepresentative
    {
        private List<Operation> _operations;
        private List<OperationsGroup> _operationsGroup;
        private List<Filter> _filters;

        private const int HEIGHT = 27;

        public string RepresentativeName { get; set; }

        public AnalysisViewItem(string name, List<Filter> filters, string from, string to)
        {
            InitializeComponent();
            _filters = filters;
            RepresentativeName = name;

            Show(from, to);
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

        public void Show(string from, string to)
        {
            DateTime dt1 = DateTime.Parse(from);
            DateTime dt2 = DateTime.Parse(to);
            _operations = Service.Operations.Where(o => o.Parent == null && o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();
            _operationsGroup = Service.OperationsGroups.Where(o => o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();
            var ioperations = new List<IOperation>();
            ioperations.AddRange(_operations);
            ioperations.AddRange(_operationsGroup);

            var groups = new List<Group>();

            foreach (var elem in _operationsGroup)
            {
                elem.UpdateAmount(Service.TransactionTypes);
            }

            foreach (var item in _filters)
            {
                var newGroup = new Group(item.Description, item.Query, ioperations);
                newGroup.FilterOperations();

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
                Style = (Style)FindResource("MyBorder2"),
                BorderThickness = new Thickness(1, 1, 1, 1),
                Height = HEIGHT + 5,
            };
            borderGroup.Child = GroupHeaderToStackPanel(group, result);
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, group, result, "MyLightGrey", new Thickness(0, 0, 0, 0), ClickInGroup));

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
                            Margin = new Thickness(0, 0, 0, 0),
                        };
                        var border = new Border
                        {
                            Style = (Style)FindResource("MyBorder2"),
                            BorderThickness = new Thickness(1, 1, 1, 1),
                            Height = HEIGHT,
                        };
                        border.Child = IOperationToStackPanel(i);
                        childStackPanel.Children.Add(CreateButtonWithBorderContent(border, item, childStackPanel, "MyLightGrey", new Thickness(25, 0, 0, 0), ClickInOperationsGroup));

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
                for (int i1 = 2; i1 < list.Count; i1++)
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
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Object = objectForButton,
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
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            var borderDate = CreateBorderWithLabel($"{operation.Date.ToString("dd.MM.yyyy")}:");
            var borderAmount = CreateBorderWithLabel($"{operation.Amount}");
            var borderDescription = CreateBorderWithLabel($"{operation.Description}");
            var borderTransactionType = CreateBorderWithLabel($"{operation.TransactionType}");
            var borderImportance = CreateBorderWithLabel($"{operation.Importance}");
            var borderFrequence = CreateBorderWithLabel($"{operation.Frequence}");
            var button = new ButtonWithObject()
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\edit-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch
                },
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Object = operation,
            };

            button.Click += Edit_Click;

            resultStackPanel.Children.Add(borderDate);
            resultStackPanel.Children.Add(borderAmount);
            resultStackPanel.Children.Add(borderDescription);
            resultStackPanel.Children.Add(borderTransactionType);
            resultStackPanel.Children.Add(borderImportance);
            resultStackPanel.Children.Add(borderFrequence);
            if (operation is Operation)
            {
                var borderTransferType = CreateBorderWithLabel($"{(operation as Operation).TransferType}");
                resultStackPanel.Children.Add(borderTransferType);
            }
            foreach (var item in operation.Tags)
            {
                resultStackPanel.Children.Add(CreateBorderWithLabel($"{item}"));
            }
            resultStackPanel.Children.Add(button);

            return resultStackPanel;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("click edit");
            var button = sender as ButtonWithObject;
            var parameter = button.Object;

            if (parameter is Operation)
            {
                var view = new ManuallyOperationsView(parameter as Operation);
                ViewManager.AddUserControl(view);
                ViewManager.OpenUserControl(view);
            }
            else if (parameter is OperationsGroup)
            {
                var view = new AddGroupsView(parameter as OperationsGroup);
                ViewManager.AddUserControl(view);
                ViewManager.OpenUserControl(view);
            }
            else if (parameter is Group)
            {
                var stackPanel = button.Context as StackPanel;
                stackPanel.Children[0].Visibility = stackPanel.Children[0].Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            e.Handled = true;
        }

        private UIElement GroupHeaderToStackPanel(Group group, StackPanel main)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = HEIGHT + 3,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            var borderAmount = CreateBorderWithLabel($"{group.Amount}");
            var borderDescription = CreateBorderWithLabel($"{group.Filter.Description}");
            var button = new ButtonWithObject()
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\edit-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch
                },
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Object = group,
                Context = main,
            };

            button.Click += Edit_Click;

            resultStackPanel.Children.Add(borderDescription);
            resultStackPanel.Children.Add(borderAmount);
            resultStackPanel.Children.Add(button);

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
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                },
                BorderBrush = (SolidColorBrush)FindResource("MyLight"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = HEIGHT,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
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

        public override string ToString()
        {
            return "AnalysisView";
        }
    }
}


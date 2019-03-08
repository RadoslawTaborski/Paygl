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
using System.Windows.Media.Imaging;
using PayglService.Models;

namespace Paygl.Views
{
    /// <summary>
    /// Interaction logic for ShowOperations.xaml
    /// </summary>
    public partial class AnalysisViewItem : IRepresentative
    {
        private List<Operation> _operations;
        private List<OperationsGroup> _operationsGroup;
        private readonly FiltersGroup _group;

        private const int RefHeight = 27;

        public string RepresentativeName { get; set; }

        public AnalysisViewItem(string name, FiltersGroup group, string from, string to)
        {
            InitializeComponent();
            _group = group;
            RepresentativeName = name;

            Show(from, to);
        }

        public void Show(string from, string to)
        {
            var dt1 = DateTime.Parse(from);
            var dt2 = DateTime.Parse(to);
            _operations = Service.Operations.Where(o => o.Parent == null && o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();
            _operationsGroup = Service.OperationsGroups.Where(o => o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();
            var ioperations = new List<IOperation>();
            ioperations.AddRange(_operations);
            ioperations.AddRange(_operationsGroup);

            var groups = new List<Group>();
            var multiGroups = new List<Groups>();

            foreach (var elem in _operationsGroup)
            {
                elem.UpdateAmount(Service.TransactionTypes);
            }

            _group.Items.Sort((x, y) => x.Value.CompareTo(y.Value));
            foreach (var item in _group.Items)
            {
                switch (item.Key)
                {
                    case FiltersGroup key:
                    {
                        var newGroup = new Groups(key, ioperations);
                        newGroup.FilterOperations();

                        multiGroups.Add(newGroup);
                        break;
                    }
                    case Filter filter:
                    {
                        var newGroup = new Group(filter, ioperations);
                        newGroup.FilterOperations();

                        groups.Add(newGroup);
                        break;
                    }
                }
            }

            _spDisplay.Children.Clear();

            foreach(var item in multiGroups)
            {
                item.UpdateAmount();
                _spDisplay.Children.Add(GroupsToStackPanel(item, new Thickness(0,0,0,10),RefHeight+3));
            }

            foreach (var item in groups)
            {
                item.UpdateAmount();
                _spDisplay.Children.Add(GroupToStackPanel(item, new Thickness(0,0,0,10), RefHeight+3));
            }

            _labSum.Content = SumGroups(groups, multiGroups);
        }

        private UIElement GroupsToStackPanel(Groups groups, Thickness margin, int height)
        {
            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = margin,
            };

            var borderGroup = new Border
            {
                Style = (Style) FindResource("MyBorder2"),
                BorderThickness = new Thickness(1, 1, 1, 1),
                Height = height,
                Child = GroupsHeaderToStackPanel(groups),
            };
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, groups, result, "MyLightGrey", new Thickness(0, 0, 0, 0), ClickInGroups));

            return result;
        }

        private UIElement GroupToStackPanel(Group group, Thickness margin, int height)
        {
            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = margin,
            };

            var borderGroup = new Border
            {
                Style = (Style) FindResource("MyBorder2"),
                BorderThickness = new Thickness(1, 1, 1, 1),
                Height = height,
                Child = GroupHeaderToStackPanel(group, result),
            };
            result.Children.Add(CreateButtonWithBorderContent(borderGroup, group, result, "MyLightGrey", new Thickness(0, 0, 0, 0), ClickInGroup));

            return result;
        }

        private void ClickInGroups(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonWithObject;
            var groups = (button?.Object as Groups);
            var context = (button?.Context as StackPanel);
            if (context != null && context.Children.Count == 1)
            {
                if (groups?.ListOfGroups == null) return;
                foreach (var item in groups.ListOfGroups)
                {
                    context.Children.Add(GroupToStackPanel(item, new Thickness(25, 0, 0, 0), RefHeight));
                }
            }
            else
            {
                System.Collections.IList list = context?.Children;
                if (list == null) return;

                for (var i1 = 1; i1 < list.Count; i1++)
                {
                    var item = list[i1];
                    if (item is UIElement i)
                    {
                        i.Visibility = i.Visibility == Visibility.Visible
                            ? Visibility.Collapsed
                            : Visibility.Visible;
                    }
                }
            }
        }

        private void ClickInGroup(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonWithObject;
            var group = (button?.Object as Group);
            var context = (button?.Context as StackPanel);
            if (context != null && context.Children.Count == 1)
            {
                if (group?.Operations == null) return;

                foreach (var item in group.Operations)
                {
                    switch (item)
                    {
                        case OperationsGroup i1:
                        {
                            var childStackPanel = new StackPanel
                            {
                                Orientation = Orientation.Vertical,
                                Margin = new Thickness(0, 0, 0, 0),
                            };
                            var border = new Border
                            {
                                Style = (Style) FindResource("MyBorder2"),
                                BorderThickness = new Thickness(1, 1, 1, 1),
                                Height = RefHeight,
                                Child = OperationToStackPanel(i1),
                            };
                            childStackPanel.Children.Add(CreateButtonWithBorderContent(border, item,
                                childStackPanel, "MyLightGrey", new Thickness(25, 0, 0, 0),
                                ClickInOperationsGroup));

                            context.Children.Add(childStackPanel);
                            break;
                        }
                        case Operation i:
                        {
                            var border = new Border
                            {
                                Style = (Style) FindResource("MyBorderMedium"),
                                BorderThickness = new Thickness(1, 1, 1, 1),
                                Height = RefHeight,
                                Child = OperationToStackPanel(i),
                            };
                            context.Children.Add(CreateButtonWithBorderContent(border, item, null, "MyMediumGrey",
                                new Thickness(25, 0, 0, 0), ClickInOperation));
                            break;
                        }
                    }
                }
            }
            else
            {
                System.Collections.IList list = context?.Children;
                if (list == null) return;
                for (var i1 = 1; i1 < list.Count; i1++)
                {
                    var item = list[i1];
                    if (item is UIElement i)
                    {
                        i.Visibility = i.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
            }
        }

        private void ClickInOperationsGroup(object sender, RoutedEventArgs e)
        {
            var button = sender as ButtonWithObject;
            var group = (button?.Object as OperationsGroup);
            var context = (button?.Context as StackPanel);
            if (context != null && context.Children.Count == 1)
            {
                if (group?.Operations == null) return;
                foreach (var elem in group.Operations)
                {
                    var border2 = new Border
                    {
                        Style = (Style) FindResource("MyBorderMedium"),
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        Height = RefHeight,
                        Child = OperationToStackPanel(elem),
                    };
                    context.Children.Add(CreateButtonWithBorderContent(border2, elem, null, "MyMediumGrey",
                        new Thickness(50, 0, 0, 0), ClickInOperation));
                }
            }
            else
            {
                System.Collections.IList list = context?.Children;
                if (list == null) return;
                for (var i1 = 0; i1 < list.Count; i1++)
                {
                    if (i1 == 0) continue;
                    var item = list[i1];
                    if (item is UIElement i)
                    {
                        i.Visibility = i.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
            }
        }

        private void ClickInOperation(object sender, RoutedEventArgs e)
        {
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

        private UIElement OperationToStackPanel(IOperation operation)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            var borderDate = CreateBorderWithLabel($"{operation.Date.ToString(Properties.strings.dateFormat)}:");
            var borderAmount = CreateBorderWithLabel($"{operation.Amount}");
            var borderDescription = CreateBorderWithLabel($"{operation.Description}");
            var borderTransactionType = CreateBorderWithLabel($"{operation.TransactionType}");
            var borderImportance = CreateBorderWithLabel($"{operation.Importance}");
            var borderFrequence = CreateBorderWithLabel($"{operation.Frequency}");
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
            if (operation is Operation operation1)
            {
                var borderTransferType = CreateBorderWithLabel($"{operation1.TransferType}");
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
            var button = sender as ButtonWithObject;
            var parameter = button?.Object;

            switch (parameter)
            {
                case Operation operation:
                {
                    var view = new ManuallyOperationsView(operation);
                    ViewManager.AddUserControl(view);
                    ViewManager.OpenUserControl(view);
                    break;
                }
                case OperationsGroup group:
                {
                    var view = new AddGroupsView(group);
                    ViewManager.AddUserControl(view);
                    ViewManager.OpenUserControl(view);
                    break;
                }
                case Group _:
                {
                    if (button.Context is StackPanel stackPanel)
                    {
                        stackPanel.Children[0].Visibility = stackPanel.Children[0].Visibility == Visibility.Visible
                            ? Visibility.Collapsed
                            : Visibility.Visible;
                    }

                    break;
                }
            }

            e.Handled = true;
        }

        private UIElement GroupsHeaderToStackPanel(Groups groups)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight + 3,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            var borderAmount = CreateBorderWithLabel($"{groups.Amount}");
            var borderDescription = CreateBorderWithLabel($"{groups.Group.Name}");

            resultStackPanel.Children.Add(borderDescription);
            resultStackPanel.Children.Add(borderAmount);

            return resultStackPanel;
        }

        private UIElement GroupHeaderToStackPanel(Group group, StackPanel main)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight + 3,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            var button = new ButtonWithObject
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
                VerticalAlignment = VerticalAlignment.Top,
                Object = group,
                Context = main,
            };
            button.Click += Edit_Click;

            var borderAmount = CreateBorderWithLabel($"{group.Amount}");
            var borderDescription = CreateBorderWithLabel($"{group.Filter.Description}");

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
                    Height = RefHeight,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                },
                BorderBrush = (SolidColorBrush)FindResource("MyLight"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        private decimal SumGroups(List<Group> groups, List<Groups> multiGroups)
        {
            var result = decimal.Zero;
            var operations = new List<Operation>();
            var groupsCopy = new List<Group>(groups);

            foreach(var item in multiGroups)
            {
                groupsCopy.AddRange(item.ListOfGroups);
            }

            foreach (var item in groupsCopy)
            {
                foreach (var elem in item.Operations)
                {
                    switch (elem)
                    {
                        case OperationsGroup group:
                        {
                            foreach (var operation in group.Operations)
                            {
                                operations.Add(operation);
                            }

                            break;
                        }
                        case Operation item1:
                            operations.Add(item1);
                            break;
                    }
                }
            }

            operations = operations.ToArray().Distinct().ToList();

            foreach (var operation in operations)
            {
                if (operation.TransactionType.Text == Properties.strings.transactionTypeIncome)
                {
                    result += operation.Amount;
                }
                if (operation.TransactionType.Text == Properties.strings.transactionTypeExpense)
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


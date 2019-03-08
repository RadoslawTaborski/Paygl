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
    public partial class ShowOperations : IRepresentative
    {
        private List<Operation> _operations;
        private List<OperationsGroup> _operationsGroup;

        private const int RefHeight = 27;

        public string RepresentativeName { get; set; } = Properties.strings.searchingRN;

        public ShowOperations()
        {
            InitializeComponent();

            Service.LoadAttributes();
            Service.LoadOperationsGroups();
            Service.LoadOperations();

            _borderCalendarFrom.Visibility = Visibility.Hidden;
            _borderCalendarTo.Visibility = Visibility.Hidden;

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

            SetVisibilityForSave(Visibility.Hidden);
            Show("");
        }

        public ShowOperations(Filter filter)
        {
            InitializeComponent();
            RepresentativeName = $"{Properties.strings.editFilterRN} {filter.Description}";

            Service.LoadAttributes();
            Service.LoadOperationsGroups();
            Service.LoadOperations();

            _borderCalendarFrom.Visibility = Visibility.Hidden;
            _borderCalendarTo.Visibility = Visibility.Hidden;

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

            SetVisibilityForSave(Visibility.Hidden);
            _tbQuery.Text = filter.Query;
            _tbName.Text = filter.Description;
            Show(filter.Query);
        }

        private void SetVisibilityForSave(Visibility v)
        {
            _labName.Content = "";
            _labName.Visibility = v;
            _tbName.Visibility = v;
            _btnSave.Visibility = v;
            _btnSaveAs.Visibility = v;

            if (v != Visibility.Visible) return;
            var filter = ViewsMemory.Filters.FirstOrDefault(f => f.Description == _tbName.Text);
            _btnSave.IsEnabled = filter != null;

            if (string.IsNullOrWhiteSpace(_tbName.Text))
            {
                _btnSaveAs.IsEnabled = false;
            }
        }

        private void Show(string query)
        {
            var dt1 = DateTime.Parse(_tbFrom.Text);
            var dt2 = DateTime.Parse(_tbTo.Text);
            _operations = Service.Operations.Where(o => o.Parent == null && o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();
            _operationsGroup = Service.OperationsGroups.Where(o => o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList();
            var ioperations = new List<IOperation>();
            ioperations.AddRange(_operations);
            ioperations.AddRange(_operationsGroup);

            var queryWithName = new KeyValuePair<string, string>("", query);

            foreach (var elem in _operationsGroup)
            {
                elem.UpdateAmount(Service.TransactionTypes);
            }
            var filter = new Filter(queryWithName.Key, queryWithName.Value);
            var group = new Group(filter, ioperations);
            group.FilterOperations();

            _spDisplay.Children.Clear();
            group.UpdateAmount();
            _spDisplay.Children.Add(GroupToStackPanel(group));

            _labSum.Content = SumGroups(group);
        }

        private void _btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var text = _tbQuery.Text;
            try
            {
                Show(text);
                SetVisibilityForSave(Visibility.Visible);
            }
            catch (Exception)
            {
                SetVisibilityForSave(Visibility.Hidden);
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.messageBoxWrongQuery);
                dialog.ShowDialog();
            }
        }

        private UIElement GroupToStackPanel(Group group)
        {
            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 20),
            };

            ShowOperationsGroup(group, result);

            return result;
        }

        private void ShowOperationsGroup(Group group, StackPanel context)
        {
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
                        childStackPanel.Children.Add(CreateButtonWithBorderContent(border, item, childStackPanel, "MyLightGrey", new Thickness(0, 0, 0, 0), ClickInOperationsGroup));

                        context.Children.Add(childStackPanel);
                        break;
                    }
                    case Operation i:
                    {
                        var border = new Border
                        {
                            Style = (Style)FindResource("MyBorderMedium"),
                            BorderThickness = new Thickness(1, 1, 1, 1),
                            Height = RefHeight,
                            Child = OperationToStackPanel(i),
                        };
                        context.Children.Add(CreateButtonWithBorderContent(border, item, null, "MyMediumGrey", new Thickness(0, 0, 0, 0), ClickInOperation));
                        break;
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
                        new Thickness(25, 0, 0, 0), ClickInOperation));
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
                    var i = item as UIElement;
                    if (i != null && i.Visibility == Visibility.Visible)
                    {
                        i.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (i != null) i.Visibility = Visibility.Visible;
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

        private decimal SumGroups(Group group)
        {
            var result = decimal.Zero;
            var operations = new List<Operation>();

            foreach (var elem in group.Operations)
            {
                switch (elem)
                {
                    case OperationsGroup operationsGroup:
                    {
                        foreach (var operation in operationsGroup.Operations)
                        {
                            operations.Add(operation);
                        }

                        break;
                    }
                    case Operation item:
                        operations.Add(item);
                        break;
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
            return "ShowOperationsView";
        }

        private void _btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Show(_tbQuery.Text);
                SetVisibilityForSave(Visibility.Hidden);
            }
            catch (Exception)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.messageBoxWrongQuery);
                dialog.ShowDialog();
            }

            ViewsMemory.Filters.Add(new Filter(_tbName.Text, _tbQuery.Text));

            Service.SetSettings(ViewsMemory.Filters);
            Service.SaveSettings();
            ViewsMemory.ChangeInFilters?.Invoke();
        }

        private void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Show(_tbQuery.Text);
                SetVisibilityForSave(Visibility.Hidden);
            }
            catch (Exception)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.messageBoxWrongQuery);
                dialog.ShowDialog();
            }

            var filter = ViewsMemory.Filters.First(f => f.Description == _tbName.Text);

            filter.SetDescription(_tbName.Text);
            filter.SetQuery(_tbQuery.Text);
           

            Service.SetSettings(ViewsMemory.Filters);
            Service.SaveSettings();
            ViewsMemory.ChangeInFilters?.Invoke();
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

        private void _tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_tbName.Text))
            {
                _btnSaveAs.IsEnabled = false;
            }
            else
            {
                _btnSaveAs.IsEnabled = !ViewsMemory.Filters.Select(f => f.Description).Contains(_tbName.Text);
            }

            _btnSave.IsEnabled = ViewsMemory.Filters.Select(f => f.Description).Contains(_tbName.Text);
        }

        private void _tbQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetVisibilityForSave(Visibility.Hidden);
        }
    }
}

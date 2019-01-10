using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
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
    /// Interaction logic for ShowOperations.xaml
    /// </summary>
    public partial class ShowOperations : UserControl
    {
        private List<Operation> _operations;
        public ShowOperations()
        {
            InitializeComponent();

            Service.LoadAttributes();
            Service.LoadOperationsGroups();
            Service.LoadOperations();
            DateTime dt1 = DateTime.Parse("01/12/2018");
            DateTime dt2 = DateTime.Parse("30/12/2018");
            _operations = Service.Operations.Where(o => o.Parent == null && o.Date.Date<=dt2.Date && o.Date.Date >= dt1.Date).ToList();

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
                var operationsGroups= Analyzer.Analyzer.FilterOperations(Service.OperationsGroups.OfType<IOperation>().ToList<IOperation>().Where(o=> o.Date.Date <= dt2.Date && o.Date.Date >= dt1.Date).ToList(), item.Value);

                var newGroup = new Group(item.Key);
                newGroup.AddRange(operations);
                newGroup.AddRange(operationsGroups);

                groups.Add(newGroup);
            }

            rtbText.Document.Blocks.Clear();

            foreach (var item in groups)
            {
                item.UpdateAmount();
                rtbText.Document.Blocks.Add(new Paragraph(new Run(DisplayGroup(item)))); ;
            }
        }

        private string DisplayGroup(Group group)
        {
            var text = "";
            text = $"{group.Description}: {group.Amount}";
            text += Environment.NewLine;
            foreach (var item in group.Operations)
            {
                if (item is Operation){
                    text += DisplayOperation(item as Operation);
                }
                if (item is OperationsGroup)
                {
                    text += DisplayGroup(item as OperationsGroup);
                }
            }

            return text;
        }

        private string DisplayGroup(OperationsGroup group)
        {
            var text = "";
            text = $"->{group.Date.ToString("dd.MM.yyyy")}: {group.Amount} - {group.Description}: {group.TransactionType} | {group.Importance} | {group.Frequence} | ";
            foreach (var item in group.Tags)
            {
                text += $"{item}; ";
            }
            text += Environment.NewLine;
            foreach (var item in group.Operations)
            {
                text += "----";
                text += DisplayOperation(item);
            }

            return text;
        }

        private string DisplayOperation(Operation operation)
        {
            var result = $"->{operation.Date.ToString("dd.MM.yyyy")}: {operation.Amount} - {operation.Description}: {operation.TransactionType} | {operation.TransferType} | {operation.Importance} | {operation.Frequence} | ";
            foreach (var item in operation.Tags)
            {
                result += $"{item}; ";
            }
            result += System.Environment.NewLine;

            return result;
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
    }
}

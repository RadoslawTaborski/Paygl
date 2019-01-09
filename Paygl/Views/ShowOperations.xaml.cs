using DataBaseWithBusinessLogicConnector.Entities;
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
        public ShowOperations()
        {
            InitializeComponent();

            Service.LoadAttributes();
            Service.LoadOperationsGroups();
            Service.LoadOperations();
            var queries = Service.ReadQuery();

            //var groups = new List<OperationsGroup>(Service.OperationsGroups);
            var groups = new List<OperationsGroup>();

            foreach(var item in queries)
            {
                var operations = Analyzer.Analyzer.FilterOperations(Service.Operations, item.Value);
                var newGroup = new OperationsGroup(null, Service.User, item.Key, null, null, DateTime.Now);
                newGroup.SetOperations(operations);
                groups.Add(newGroup);
            }

            rtbText.Document.Blocks.Clear();

            foreach (var item in groups)
            {
                item.UpdateAmount();
                rtbText.Document.Blocks.Add(new Paragraph(new Run(DisplayGroup(item)))); ;
            }
        }

        private string DisplayGroup(OperationsGroup group)
        {
            var text = "";
            text = $"{group.Date.ToString("dd.MM.yyyy")} - {group.Amount} - {group.Description}: {group.Importance} | {group.Frequence} | ";
            foreach (var item in group.Tags)
            {
                text += $"{item}; ";
            }
            text += System.Environment.NewLine;
            foreach (var item in group.Operations)
            {
                text += "->";
                text += DisplayOperation(item);
            }

            return text;
        }

        private string DisplayOperation(Operation operation)
        {
            var result = $"{operation.Date.ToString("dd.MM.yyyy")} - {operation.Amount} - {operation.Description}: {operation.TransactionType} | {operation.TransferType} | {operation.Importance} | {operation.Frequence} | ";
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

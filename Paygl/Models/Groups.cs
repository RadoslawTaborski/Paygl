using DataBaseWithBusinessLogicConnector.Interfaces;
using PayglService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paygl.Models
{
    public class Groups
    {
        public List<Group> ListOfGroups { get; private set; }
        public decimal Amount { get; private set; }
        public FiltersGroup Group { get; private set; }

        public Groups(FiltersGroup group, List<IOperation> operations)
        {
            ListOfGroups = new List<Group>();
            Group = group;
            foreach(var item in group.Filters)
            {
                ListOfGroups.Add(new Group(item, operations));
            }
        }

        public void FilterOperations()
        {
            foreach(var item in ListOfGroups)
            {
                item.FilterOperations();
            }
        }

        public void UpdateAmount()
        {
            Amount = decimal.Zero;
            foreach (var item in ListOfGroups)
            {
                item.UpdateAmount();
                Amount += item.Amount;
            }
        }
    }
}

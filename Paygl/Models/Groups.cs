using DataBaseWithBusinessLogicConnector.Interfaces;
using System.Collections.Generic;
using PayglService.Models;

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
            Group.Items.Sort((x, y) => x.Value.CompareTo(y.Value));
            foreach (var item in group.Items)
            {
                if (item.Key is Filter key)
                {
                    ListOfGroups.Add(new Group(key, operations));
                }
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

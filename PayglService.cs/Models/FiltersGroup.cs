using System;
using System.Collections.Generic;
using PayglService.Models;

namespace PayglService.Models
{
    [Serializable]
    public class FiltersGroup: IFilter
    {
        public string Name { get; private set; }
        public bool Visibility { get; set; }
        public List<KeyValuePair<IFilter,int>> Items { get; private set; }

        public FiltersGroup(string name)
        {
            Name = name;
            Visibility = false;
            Items = new List<KeyValuePair<IFilter, int>>();
        }

        public void AddFilters(List<KeyValuePair<IFilter, int>> filters)
        {
            Items.AddRange(filters);
        }

        public void AddFilter(KeyValuePair<IFilter, int> filter)
        {
            Items.Add(filter);
        }

        public void AddChildGroup(KeyValuePair<IFilter, int> group)
        {
            Items.Add(group);
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetVisibility(bool visibility)
        {
            Visibility = visibility;
        }
    }
}

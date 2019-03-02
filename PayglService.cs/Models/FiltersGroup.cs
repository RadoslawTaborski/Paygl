using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayglService.Models
{
    [Serializable]
    public class FiltersGroup
    {
        public string Name { get; private set; }
        public List<Filter> Filters { get; private set; }
        public bool Visibility { get; set; }
        private List<FiltersGroup> _childGroups;
        public List<FiltersGroup> ChildGroups
        {
            get
            {
                if (_childGroups == null)
                {
                    _childGroups = new List<FiltersGroup>();
                }
                return _childGroups;
            }
            private set
            {
                _childGroups = value;
            }
        }

        public FiltersGroup(string name)
        {
            Name = name;
            Visibility = false;
            Filters = new List<Filter>();
            _childGroups = new List<FiltersGroup>();
        }

        public void AddFilters(List<Filter> filters)
        {
            Filters.AddRange(filters);
        }

        public void AddFilter(Filter filter)
        {
            Filters.Add(filter);
        }

        public void AddChildGroup(FiltersGroup group)
        {
            ChildGroups.Add(group);
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

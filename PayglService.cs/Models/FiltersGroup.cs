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

        public FiltersGroup(string name)
        {
            Name = name;
            Filters = new List<Filter>();
        }

        public void AddFilters(List<Filter> filters)
        {
            Filters.AddRange(filters);
        }

        public void AddFilter(Filter filter)
        {
            Filters.Add(filter);
        }
    }
}

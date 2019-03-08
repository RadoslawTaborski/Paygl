using System;
using System.Collections.Generic;

namespace PayglService.Models
{
    [Serializable]
    public class Settings2
    {
        public List<Filter> Filters = new List<Filter>();
        public List<FiltersGroup> FiltersGroups = new List<FiltersGroup>();
    }
}

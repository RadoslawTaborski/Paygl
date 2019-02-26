using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayglService.Models
{
    [Serializable]
    public class Filter
    {
        public string Description { get; private set; }
        public string Query { get; private set; }

        public Filter(string description, string query)
        {
            Description = description;
            Query = query;
        }

        public void SetQuery(string query)
        {
            Query = query;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}

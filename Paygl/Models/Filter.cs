using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paygl.Models
{
    public class Filter
    {
        public string Description { get; private set; }
        public QueryNode Query { get; private set; }

        public Filter(string description, QueryNode query)
        {
            Description = description;
            Query = query;
        }

        public void SetQuery(QueryNode query)
        {
            Query = query;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}

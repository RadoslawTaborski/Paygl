using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class Query
    {
        public List<QueryItem> queries;
        public List<string> conjunctions;

        public Query()
        {
            queries = new List<QueryItem>();
            conjunctions = new List<string>();
        }
    }
}

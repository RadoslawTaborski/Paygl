using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class QueryItem
    {

        public QueryItem(string left, string operation, List<string> right)
        {
            Left = left;
            Operation = operation;
            Right = right;
        }

        public string Left { get; private set; }
        public string Operation { get; private set; }
        public List<string> Right { get; private set; }
    }
}

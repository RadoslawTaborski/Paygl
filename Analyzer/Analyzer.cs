using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Analyzer
{
    public static class Analyzer
    {
        private static List<string> _boolOperations;
        private static List<string> _setOperations;

        public static bool AnalyzeSyntax(string query)
        {
            var result = false;

            return result;
        }

        static Analyzer()
        {
            _boolOperations = new List<string>();
            foreach (PropertyInfo prop in typeof(BooleanOperations).GetProperties())
            {
                _boolOperations.Add(prop.GetValue(null).ToString());
            }

            _setOperations = new List<string>();
            foreach (PropertyInfo prop in typeof(SetOperations).GetProperties())
            {
                _setOperations.Add(prop.GetValue(null).ToString());
            }
        }

        public static List<IOperation> FilterOperations(List<IOperation> all, QueryNode node)
        {
            node.Filter(all);
            return node.Result;
        }

        private static void SplitQuery(string query, ref QueryNode node)
        {
            var subqueries = new List<string>();

            var bracketsCounter = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var c in query)
            {
                if (c == '(')
                {
                    bracketsCounter++;
                    if (bracketsCounter == 1)
                    {
                        if (sb.Length != 0)
                        {
                            subqueries.Add(sb.ToString());
                        }
                        sb = new StringBuilder();
                        continue;
                    }
                }
                if (c == ')')
                {
                    bracketsCounter--;
                    if (bracketsCounter == 0)
                    {
                        if (sb.Length != 0)
                        {
                            subqueries.Add(sb.ToString());
                        }
                        sb = new StringBuilder();
                        continue;
                    }
                }
                sb.Insert(sb.Length, c);
            }
            if (sb.Length != 0)
            {
                subqueries.Add(sb.ToString());
            }

            foreach (var item in subqueries)
            {
                var itemWithoutWhiteSpace = item.Trim();
                if ((itemWithoutWhiteSpace.Count(f => f == '(') == 0 && IsSingleQuery(itemWithoutWhiteSpace)) || subqueries.Count == 1)
                {
                    if (!_boolOperations.Contains(itemWithoutWhiteSpace))
                    {
                        var _boolOperationsCopy = new List<string>(_boolOperations);
                        for(var i=0; i < _boolOperationsCopy.Count; ++i)
                        {
                            _boolOperationsCopy[i] = $"{_boolOperationsCopy[i]} ";
                        }
                        var (substrings, separators) = itemWithoutWhiteSpace.SplitWithSeparators(_boolOperationsCopy.ToArray());
                        for (int i = 0; i < substrings.Count; i++)
                        {
                            string substring = substrings[i];
                            if (i != 0)
                            {
                                node.Items.Add(new QueryLeafOperation(separators[i - 1]));
                            }
                            if (substring != "")
                            {
                                var (substrings1, separators1) = substring.SplitWithSeparators(_setOperations.ToArray());
                                if (substrings1.Count != 2 && separators1.Count != 1)
                                {
                                    throw new Exception("Wrong query");
                                }
                                if (!KeyWords.List.Contains(substrings1[0]))
                                {
                                    throw new Exception("Wrong query");
                                }
                                var right = new List<string>();
                                substrings1[1] = substrings1[1].Replace("[", "");
                                substrings1[1] = substrings1[1].Replace("]", "");
                                var rights = substrings1[1].Split(',');
                                foreach (var elem in rights)
                                {
                                    var tmp = elem;
                                    tmp = tmp.Substring(tmp.IndexOf("\"") + 1);
                                    tmp = tmp.Substring(0, tmp.IndexOf("\""));
                                    right.Add(tmp);
                                }
                                node.Items.Add(new QueryLeaf(substrings1[0], separators1[0], right, node.OnlyOperations));
                            }
                        }
                    }
                    else
                    {
                        node.Items.Add(new QueryLeafOperation(itemWithoutWhiteSpace));
                    }

                    continue;
                }
                var newNode = new QueryNode(node.OnlyOperations);
                node.Items.Add(newNode);
                SplitQuery(item, ref newNode);
            }
        }

        private static bool IsSingleQuery(string query)
        {
            var (substrings, separators) = query.SplitWithSeparators(_boolOperations.ToArray());
            substrings.RemoveAll(item => item == "");
            if (substrings.Count > 1)
            {
                return false;
            }

            return true;
        }

        public static QueryNode StringToQuery(string query, bool onlyOperations=false)
        {
            var root = new QueryNode(onlyOperations);
            SplitQuery(query, ref root);
            return root;
        }
    }
}

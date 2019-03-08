using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public static class Analyzer
    {
        private static readonly List<string> BoolOperations;
        private static readonly List<string> SetOperations;

        static Analyzer()
        {
            BoolOperations = new List<string>();
            foreach (var prop in typeof(BooleanOperations).GetProperties())
            {
                BoolOperations.Add(prop.GetValue(null).ToString());
            }

            SetOperations = new List<string>();
            foreach (var prop in typeof(SetOperations).GetProperties())
            {
                SetOperations.Add(prop.GetValue(null).ToString());
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
            var sb = new StringBuilder();
            foreach (var c in query)
            {
                switch (c)
                {
                    case '(':
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

                        break;
                    }
                    case ')':
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

                        break;
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
                    if (!BoolOperations.Contains(itemWithoutWhiteSpace))
                    {
                        var boolOperationsCopy = new List<string>(BoolOperations);
                        for(var i=0; i < boolOperationsCopy.Count; ++i)
                        {
                            boolOperationsCopy[i] = $"{boolOperationsCopy[i]} ";
                        }
                        var (substrings, separators) = itemWithoutWhiteSpace.SplitWithSeparators(boolOperationsCopy.ToArray());
                        for (int i = 0; i < substrings.Count; i++)
                        {
                            string substring = substrings[i];
                            if (i != 0)
                            {
                                node.Items.Add(new QueryLeafOperation(separators[i - 1]));
                            }
                            if (substring != "")
                            {
                                var (substrings1, separators1) = substring.SplitWithSeparators(SetOperations.ToArray());
                                if (substrings1.Count != 2 && separators1.Count != 1)
                                {
                                    throw new Exception(Properties.strings.ExWrongQuery);
                                }
                                if (!KeyWords.List.Contains(substrings1[0]))
                                {
                                    throw new Exception(Properties.strings.ExWrongQuery);
                                }
                                var right = new List<string>();
                                substrings1[1] = substrings1[1].Replace("[", "");
                                substrings1[1] = substrings1[1].Replace("]", "");
                                var rights = substrings1[1].Split(',');
                                foreach (var elem in rights)
                                {
                                    var tmp = elem;
                                    tmp = tmp.Substring(tmp.IndexOf("\"", StringComparison.Ordinal) + 1);
                                    tmp = tmp.Substring(0, tmp.IndexOf("\"", StringComparison.Ordinal));
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
            var (substrings, _) = query.SplitWithSeparators(BoolOperations.ToArray());
            substrings.RemoveAll(item => item == "");
            return substrings.Count <= 1;
        }

        public static QueryNode StringToQuery(string query, bool onlyOperations=false)
        {
            var root = new QueryNode(onlyOperations);
            SplitQuery(query, ref root);
            return root;
        }
    }
}

using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Analyzer
{
    public static class Analyzer
    {
        public static bool AnalyzeSyntax(string query)
        {
            var result = false;

            return result;
        }

        public static Query StringToQuery(string query)
        {
            var result = new Query();

            var boolOperations = new List<string>();
            foreach (PropertyInfo prop in typeof(BooleanOperations).GetProperties())
            {
                boolOperations.Add(prop.GetValue(null).ToString());
            }

            var setOperations = new List<string>();
            foreach (PropertyInfo prop in typeof(SetOperations).GetProperties())
            {
                setOperations.Add(prop.GetValue(null).ToString());
            }

            var (substrings, separators) = query.SplitWithSeparators(boolOperations.ToArray());
            result.conjunctions = separators;
            foreach (var item in substrings)
            {
                var (substrings1, separators1) = item.SplitWithSeparators(setOperations.ToArray());
                if (substrings1.Count != 2 && separators1.Count != 1)
                {
                    throw new Exception("Wrong query");
                }
                if (!KeyWords.List.Contains(substrings1[0]))
                {
                    throw new Exception("Wrong query");
                }
                var right = new List<string>();
                substrings1[1] = substrings1[1].Replace("(", "");
                substrings1[1] = substrings1[1].Replace(")", "");
                var rights = substrings1[1].Split(',');
                foreach (var elem in rights)
                {
                    var tmp = elem;
                    tmp = tmp.Substring(tmp.IndexOf("\"") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("\""));
                    right.Add(tmp);
                }
                result.queries.Add(new QueryItem(substrings1[0], separators1[0], right));
            }

            return result;
        }

        public static List<IOperation> FilterOperations(List<IOperation> operations, Query query)
        {
            var result = new List<IOperation>(operations);
            result = FilterOperations(result, query.queries[0]);
            for (int i = 1; i < query.queries.Count; i++)
            {
                switch (query.conjunctions[i-1])
                {
                    case string w when w == BooleanOperations.Conjunction:
                        result = FilterOperations(result, query.queries[i]);
                        break;
                    case string w when w == BooleanOperations.Disjunction:
                        result.AddRange(FilterOperations(result, query.queries[i]));
                        result.ToArray().Distinct().ToList();
                        break;
                    default:
                        throw new Exception($"Operations {query.conjunctions[i - 1]} not exist");
                }
            }

            return result;
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static List<IOperation> FilterOperations(List<IOperation> operations, QueryItem query)
        {
            var result = new List<IOperation>(operations);
            foreach (var key in KeyWords.List)
            {
                if (!query.Left.Equals(key))
                {
                    continue;
                }
                if (!key.Equals("Tags"))
                {
                    switch (query.Operation)
                    {
                        case string w when w == SetOperations.In:
                            result = result.Where(o => query.Right.Contains((GetPropValue(o, KeyWords.OperationProperty[key]) as IParameter).Text)).ToList();
                            break;
                        case string w when w == SetOperations.NotIn:
                            result = result.Where(o => !query.Right.Contains((GetPropValue(o, KeyWords.OperationProperty[key]) as IParameter).Text)).ToList();
                            break;
                        default:
                            throw new Exception($"Operations is wrong for {query.Operation}");
                    }
                }
                else
                {
                    var tags = result.Select(r => (GetPropValue(r, KeyWords.OperationProperty[key]) as RelTag).Tag.Text);
                    switch (query.Operation)
                    {
                        case string w when w == SetOperations.NotIn:
                            result = result.Where(o => query.Right.Intersect(o.Tags.Select(x => x.Tag.Text)).Count() == 0).ToList();
                            break;
                        case string w when w == SetOperations.AllIn:
                            result = result.Where(o => query.Right.Intersect(o.Tags.Select(x => x.Tag.Text)).Count() == query.Right.Count()).ToList();
                            break;
                        case string w when w == SetOperations.OneIn:
                            result = result.Where(o => query.Right.Intersect(o.Tags.Select(x => x.Tag.Text)).Count() > 0).ToList();
                            break;
                        default:
                            throw new Exception($"Operations is wrong for {query.Operation}");
                    }
                }
            }
            return result;
        }
    }
}

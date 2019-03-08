using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Analyzer
{
    public class QueryLeaf: IQueryItem
    {
        public string Left { get; private set; }
        public string Operation { get; private set; }
        public List<string> Right { get; private set; }
        public bool OnlyOperations { get; }

        public List<IOperation> Result { get; private set; }

        public QueryLeaf(string left, string operation, List<string> right, bool onlyOperations)
        {
            Left = left;
            Operation = operation;
            Right = right;
            OnlyOperations = onlyOperations;
        }

        public void SetRight(List<string> right)
        {
            Right = right;
        }

        public void SetLeft(string left)
        {
            Left = left;
        }

        public void SetOperation(string operation)
        {
            Operation = operation;
        }

        public void Filter(List<IOperation> all)
        {
            var result = new List<IOperation>(all);
            if (OnlyOperations)
            {
                var onlyOperations= result.Where(r => (r is Operation)).Select(r => (r as Operation)).ToList();
                onlyOperations.AddRange(result.Where(r => (r is OperationsGroup)).SelectMany(r => (r as OperationsGroup)?.Operations).ToList());
                result = new List<IOperation>();
                result.AddRange(onlyOperations);
            }
            foreach (var key in KeyWords.List)
            {
                if (!Left.Equals(key))
                {
                    continue;
                }
                if (!key.Equals(Properties.noTranslable.tags) && !key.Equals(Properties.noTranslable.name) && !key.Equals(Properties.noTranslable.transferType))
                {
                    switch (Operation)
                    {
                        case string w when w == SetOperations.In:
                            result = result.Where(o => Right.Contains((GetPropValue(o, KeyWords.OperationProperty[key]) as IParameter)?.Text)).ToList();
                            break;
                        case string w when w == SetOperations.NotIn:
                            result = result.Where(o => !Right.Contains((GetPropValue(o, KeyWords.OperationProperty[key]) as IParameter)?.Text)).ToList();
                            break;
                        default:
                            throw new Exception(string.Format(Properties.strings.ExOperationsIsWrong,Operation));
                    }
                }
                else if(key.Equals(Properties.noTranslable.tags))
                {
                    switch (Operation)
                    {
                        case string w when w == SetOperations.NotIn:
                            result = result.Where(o => !Right.Intersect(o.Tags.Select(x => x.Tag.Text)).Any()).ToList();
                            break;
                        case string w when w == SetOperations.AllIn:
                            result = result.Where(o => Right.Intersect(o.Tags.Select(x => x.Tag.Text)).Count() == Right.Count()).ToList();
                            break;
                        case string w when w == SetOperations.OneIn:
                            result = result.Where(o => Right.Intersect(o.Tags.Select(x => x.Tag.Text)).Any()).ToList();
                            break;
                        default:
                            throw new Exception(string.Format(Properties.strings.ExOperationsIsWrong, Operation));
                    }
                }
                else if (key.Equals(Properties.noTranslable.name))
                {
                    var text = RightsToRegex();
                    var regex = new Regex(text);

                    switch (Operation)
                    {
                        case string w when w == SetOperations.NotLike:
                            result = result.Where(o => !regex.Match(o.Description).Success).ToList();
                            break;
                        case string w when w == SetOperations.Like:
                            result = result.Where(o => regex.Match(o.Description).Success).ToList();
                            break;
                        default:
                            throw new Exception(string.Format(Properties.strings.ExOperationsIsWrong, Operation));
                    }
                }
                else if (key.Equals(string.Format(Properties.strings.ExOperationsIsWrong, Operation)))
                {
                    switch (Operation)
                    {
                        case string w when w == SetOperations.In:
                            result = result.Where(o => o is Operation && Right.Contains((GetPropValue(o, KeyWords.OperationProperty[key]) as IParameter)?.Text)).ToList();
                            break;
                        case string w when w == SetOperations.NotIn:
                            result = result.Where(o => o is Operation && !Right.Contains((GetPropValue(o, KeyWords.OperationProperty[key]) as IParameter)?.Text)).ToList();
                            break;
                        default:
                            throw new Exception(string.Format(Properties.strings.ExOperationsIsWrong, Operation));
                    }
                }
            }
            Result= result;
        }

        private string RightsToRegex()
        {
            var result = "(";

            foreach(var item in Right)
            {
                var tmp = item.Replace("%", ".*");
                result += "^" + tmp + "|";
            }
            result = result.Substring(0, result.Length - 1);
            result += ")";

            return result;
        }

        private object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        public override string ToString()
        {
            var rights = "[";
            foreach (var item in Right)
            {
                rights += $"\"{item}\",";
            }
            rights = rights.Substring(0, rights.Length - 1);
            rights += "]";
            var result = $"({Left} {Operation} {rights})";

            return result;
        }
    }
}

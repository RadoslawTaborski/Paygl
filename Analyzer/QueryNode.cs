using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class QueryNode: IQueryItem
    {
        public List<IQueryItem> Items { get; private set; }
        public List<IOperation> Result { get; private set; }
        public bool OnlyOperations { get; }

        public QueryNode(bool onlyOperations)
        {
            OnlyOperations = onlyOperations;
            Items = new List<IQueryItem>();
        }

        public void AddItem(IQueryItem item)
        {
            Items.Add(item);
        }

        public void Filter(List<IOperation> all)
        {
            foreach(var item in Items)
            {
                if (item is QueryLeafOperation)
                {
                    continue;
                }
                item.Filter(all);
            }
            if (Items.Count > 0)
            {
                var result = Items[0].Result;
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    if (item is QueryLeafOperation)
                    {
                        var operation = item as QueryLeafOperation;
                        switch (operation.Operation)
                        {
                            case string w when w == BooleanOperations.Conjunction:
                                result = result.Intersect(Items[i + 1].Result).ToList();
                                break;
                            case string w when w == BooleanOperations.Disjunction:
                                result.AddRange(Items[i + 1].Result);
                                result.ToArray().Distinct().ToList();
                                break;
                            default:
                                throw new Exception($"Operations {operation.Operation} not exist");
                        }
                    }
                }
                Result = result;
            }
            else
            {
                Result = all;
            }


        }

        public override string ToString()
        {
            var result = "";
            foreach(var item in Items)
            {
                result += $"{item} ";
            }
            return result;
        }
    }
}

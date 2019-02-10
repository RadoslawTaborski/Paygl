using Analyzer;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paygl.Models
{
    public class Group
    {
        public Filter Filter { get; private set; }
        public decimal Amount { get; private set; }
        public List<IOperation> AllOperations{get; private set;}

        public List<IOperation> Operations { get; private set; }

        public Group(string description, QueryNode query, List<IOperation> all)
        {
            Filter = new Filter(description, query);
            AllOperations = all;
            Operations = new List<IOperation>();
        }

        public void FilterOperations()
        {
            Operations = Analyzer.Analyzer.FilterOperations(AllOperations, Filter.Query);
            Operations.Sort((x, y) => x.Date.CompareTo(y.Date));
        }

        public void UpdateAmount()
        {
            Amount = decimal.Zero;
            foreach(var item in Operations)
            {
                if (item.TransactionType.Text =="przychód")
                {
                    Amount += item.Amount;
                }
                else
                {
                    Amount -= item.Amount;
                }
            }
        }

        public void SetQuery(QueryNode query)
        {
            Filter.SetQuery(query);
        }
    }
}

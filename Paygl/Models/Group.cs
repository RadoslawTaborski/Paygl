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
        public string Description { get; private set; }
        public QueryNode Query { get; private set; }
        public decimal Amount { get; private set; }
        public List<IOperation> AllOperations{get; private set;}

        public List<IOperation> Operations { get; private set; }

        public Group(string description, QueryNode query, List<IOperation> all)
        {
            Description = description;
            Query = query;
            AllOperations = all;
            Operations = new List<IOperation>();
        }

        public void FilterOperations()
        {
            Operations = Analyzer.Analyzer.FilterOperations(AllOperations, Query);
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
            Query = query;
        }
    }
}

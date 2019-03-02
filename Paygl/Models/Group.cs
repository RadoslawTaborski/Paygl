using Analyzer;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using PayglService.Models;
using System;
using System.Collections.Generic;
using PayglService.Models;
using System.Text;

namespace Paygl.Models
{
    public class Group
    {
        public Filter Filter { get; private set; }
        public decimal Amount { get; private set; }
        public List<IOperation> AllOperations{get; private set;}

        public List<IOperation> Operations { get; private set; }

        public Group(Filter filter, List<IOperation> operations)
        {
            Filter = filter;
            AllOperations = operations;
            Operations = new List<IOperation>();
        }

        public void FilterOperations()
        {
            var query = Analyzer.Analyzer.StringToQuery(Filter.Query);
            Operations = Analyzer.Analyzer.FilterOperations(AllOperations, query);
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

        public void SetQuery(string query)
        {
            Filter.SetQuery(query);
        }
    }
}

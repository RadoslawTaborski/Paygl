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
        public decimal Amount { get; private set; }

        public List<IOperation> Operations { get; private set; }

        public Group(string description)
        {
            Description = description;
            Operations = new List<IOperation>();
        }

        public void AddRange(List<IOperation> operations)
        {
            Operations.AddRange(operations);
        }

        public void Add(IOperation operation)
        {
            Operations.Add(operation);
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
    }
}

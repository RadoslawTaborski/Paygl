using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class OperationDetails : IEntity
    {
        public int Id { get; private set; }
        public Operation Operation { get; private set; }
        public string Name { get; private set; }
        public double Quantity { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsDirty { get; private set; }

        public OperationDetails(int id, Operation operation, string name, double quantity, decimal amount)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Amount = amount;
            Operation = operation;
        }
    }
}

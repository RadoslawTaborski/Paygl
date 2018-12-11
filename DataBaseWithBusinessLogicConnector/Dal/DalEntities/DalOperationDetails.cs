using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalOperationDetails
    {
        public int Id { get; private set; }
        public int OperationId { get; private set; }
        public string Name { get; private set; }
        public double Quantity { get; private set; }
        public decimal Amount { get; private set; }


        public DalOperationDetails(int id, int operationId, string name, double quantity, decimal amount)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Amount = amount;
            OperationId = operationId;
        }
    }
}

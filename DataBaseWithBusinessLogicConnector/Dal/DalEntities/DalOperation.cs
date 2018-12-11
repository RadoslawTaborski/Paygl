using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalOperation
    {
        public int Id { get; private set; }
        public int ParentId { get; private set; }
        public int UserId { get; private set; }
        public decimal Amount { get; private set; }
        //TODO: enumy do tabeli
        public int FrequentId { get; private set; }
        public int ImportanceId { get; private set; }
        public string Date { get; private set; }
        public string ReceiptPath { get; private set; }


        public DalOperationDetails(int id, int operationId, string name, double quantity, decimal amount)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Amount = amount;
            ParentId = operationId;
        }
    }
}

using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalOperation : IDalEntity
    {
        public int Id { get; private set; }
        public int ParentId { get; private set; }
        public int UserId { get; private set; }
        public decimal Amount { get; private set; }
        public int TransactionTypeId { get; private set; }
        public int TransferTypeId { get; private set; }
        public int FrequenceId { get; private set; }
        public int ImportanceId { get; private set; }
        public string Date { get; private set; }
        public string ReceiptPath { get; private set; }


        public DalOperation(int id, int parentId, int userId, decimal amount, int transactionTypeId, int transferTypeId, int frequenceId, int importanceId, string date, string receiptPath)
        {
            Id = id;
            ParentId = parentId;
            UserId = userId;
            Amount = amount;
            TransactionTypeId = transactionTypeId;
            TransferTypeId = transferTypeId;
            FrequenceId = frequenceId;
            ImportanceId = importanceId;
            Date = date;
            ReceiptPath = receiptPath;
        }
    }
}

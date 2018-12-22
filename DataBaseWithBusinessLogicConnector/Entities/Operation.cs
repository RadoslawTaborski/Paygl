using DataBaseWithBusinessLogicConnector.Interfaces;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Operation : IEntity
    {
        public int Id { get; private set; }
        public Operation Parent { get; private set; }
        public User User { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public TransferType TransferType { get; private set; }
        public Frequence Frequence { get; private set; }
        public Importance Importance { get; private set; }
        public DateTime Date { get; private set; }
        public string ReceiptPath { get; private set; }
        public List<RelTag> Tags { get; private set; }
        public List<OperationDetails> DetailsList { get; private set; }
        public bool IsDirty { get; private set; }

        public Operation(int id, Operation parent, User user, decimal amount, TransactionType transactionType, TransferType transferType, Frequence frequence, Importance importance, DateTime date, string receiptPath)
        {
            Id = id;
            Parent = parent;
            User = user;
            Amount = amount;
            TransactionType = transactionType;
            TransferType = transferType;
            Frequence = frequence;
            Importance = importance;
            Date = date;
            ReceiptPath = receiptPath;
            Tags = new List<RelTag>();
            DetailsList = new List<OperationDetails>();
        }

        public void SetDetailsList(IEnumerable<OperationDetails> detailsCollection)
        {
            DetailsList = detailsCollection.ToList();
        }

        public void SetTags(IEnumerable<RelTag> tags)
        {
            Tags = tags.ToList();
        }
    }
}
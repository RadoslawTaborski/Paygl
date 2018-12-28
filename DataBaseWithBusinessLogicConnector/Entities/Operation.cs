using DataBaseWithBusinessLogicConnector.Interfaces;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Operation : IEntity
    {
        public int? Id { get; private set; }
        public OperationsGroup Parent { get; private set; }
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
        public bool IsDirty { get; set; }
        public string Description { get; private set; }
        public string ShortDescription { get; private set; }

        public Operation(int? id, OperationsGroup parent, User user, string description, decimal amount, TransactionType transactionType, TransferType transferType, Frequence frequence, Importance importance, DateTime date, string receiptPath)
        {
            Id = id;
            Parent = parent;
            User = user;
            Description = description;
            ShortDescription = description;
            Amount = amount;
            TransactionType = transactionType;
            TransferType = transferType;
            Frequence = frequence;
            Importance = importance;
            Date = date;
            ReceiptPath = receiptPath;
            Tags = new List<RelTag>();
            DetailsList = new List<OperationDetails>();
            IsDirty = true;

            if (parent != null)
            {
                parent.AddOperation(this);
            }
        }

        public void SetDetailsList(IEnumerable<OperationDetails> detailsCollection)
        {
            DetailsList = detailsCollection.ToList();
        }

        public void SetTags(IEnumerable<RelTag> tags)
        {
            Tags = tags.ToList();
        }

        public void SetParent(OperationsGroup parent)
        {
            if (Parent != null)
            {
                Parent.RemoveOperation(this);
            }
            Parent = parent;
            Parent.AddOperation(this);
        }

        public void SetShortDescription(string newDescription)
        {
            ShortDescription = newDescription;
        }

        public (RelTag, RelOperation) AddTag(Tag tag)
        {
            var relTag = new RelTag(null, tag, Id);
            var relOperation = new RelOperation(null, this, tag.Id);
            Tags.Add(relTag);

            tag.AddOperation(relOperation);

            return (relTag, relOperation);
        }

        public void RemoveTag(RelTag tag)
        {
            Tags.Remove(tag);
            tag.Tag.RemoveOperation(tag.Tag.Operations.Where(o => o.Operation == this).First());
        }

        public void SetFrequence(Frequence frequence)
        {
            Frequence = frequence;
        }

        public void SetImportance(Importance importance)
        {
            Importance = importance;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Date.ToString("dd.MM.yyyy") + " " + Description;
        }

        public void SetDescription(string text)
        {
            Description = text;
        }

        public void SetTransaction(TransactionType transactionType)
        {
            TransactionType = transactionType;
        }

        public void SetTransfer(TransferType transferType)
        {
            TransferType = transferType;
        }

        public void SetDate(DateTime date)
        {
            Date = date;
        }

        public void SetAmount(decimal? value)
        {
            if (value.HasValue)
            {
                Amount = value.Value;
            }
        }

        public void RemoveAllTags()
        {
            Tags.Clear();
        }
    }
}
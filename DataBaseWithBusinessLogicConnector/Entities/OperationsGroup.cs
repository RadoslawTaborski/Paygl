using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class OperationsGroup : IEntity, IOperation
    {
        public int? Id { get; private set; }
        public User User { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public Frequence Frequence { get; private set; }
        public Importance Importance { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateTime Date { get; private set; }
        public List<RelTag> Tags { get; private set; }
        public List<Operation> Operations { get; private set; }
        public bool IsDirty { get; set; }

        public OperationsGroup(int? id, User user, string description, Frequence frequence, Importance importance, DateTime date)
        {
            Id = id;
            User = user;
            Description = description;
            Amount = decimal.Zero;
            Frequence = frequence;
            Importance = importance;
            Date = date;
            Tags = new List<RelTag>();
            Operations = new List<Operation>();
            IsDirty = true;
        }

        public void SetOperations(IEnumerable<Operation> operations)
        {
            Operations = operations.ToList();
        }

        public void AddOperation(Operation operation)
        {
            Operations.Add(operation);
        }

        public void RemoveOperation(Operation operation)
        {
            Operations.Remove(operation);
        }

        public void RemoveAllOperations()
        {
            Operations.Clear();
        }

        public void SetTags(IEnumerable<RelTag> tags)
        {
            Tags = tags.ToList();
        }

        public void AddTag(Tag tag)
        {
            var relTag = new RelTag(null, tag, Id);
            Tags.Add(relTag);
        }

        public void RemoveTag(RelTag tag)
        {
            Tags.Remove(tag);
        }

        public void RemoveAllTags()
        {
            Tags.Clear();
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

        public void SetDate(DateTime date)
        {
            Date = date;
        }

        public void UpdateAmount(List<TransactionType> types)
        {
            foreach(var item in Operations)
            {
                if (item.TransactionType.Text == "przychód")
                {
                    Amount += item.Amount;
                } else
                {
                    Amount -= item.Amount;
                }
            }
            if (Amount < 0)
            {
                TransactionType = types.Where(t => t.Text == "wydatek").First();
                Amount = Math.Abs(Amount);
            }
            else
            {
                TransactionType = types.Where(t => t.Text == "przychód").First();
            }
        }
    }
}

using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class OperationsGroup : IEntity, IOperation
    {
        public int? Id { get; private set; }
        public User User { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public Frequency Frequency { get; private set; }
        public Importance Importance { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateTime Date { get; private set; }
        public List<RelTag> Tags { get; private set; }
        public List<Operation> Operations { get; private set; }
        public bool IsDirty { get; set; }

        public OperationsGroup(int? id, User user, string description, Frequency frequency, Importance importance, DateTime date)
        {
            Id = id;
            User = user;
            Description = description;
            Amount = decimal.Zero;
            Frequency = frequency;
            Importance = importance;
            Date = date;
            Tags = new List<RelTag>();
            Operations = new List<Operation>();
            IsDirty = true;
        }

        public void SetOperations(IEnumerable<Operation> operations)
        {
            Operations = operations.ToList();
            IsDirty = true;
        }

        public void AddOperation(Operation operation)
        {
            Operations.Add(operation);
            IsDirty = true;
        }

        public void RemoveOperation(Operation operation)
        {
            Operations.Remove(operation);
            IsDirty = true;
        }

        public void RemoveAllOperations()
        {
            Operations.Clear();
            IsDirty = true;
        }

        public void SetTags(IEnumerable<RelTag> tags)
        {
            Tags = tags.ToList();
            IsDirty = true;
        }

        public void AddTag(Tag tag)
        {
            if (!Tags.Any(t => t.Tag.Text == tag.Text))
            {
                var relTag = new RelTag(null, tag, Id);
                Tags.Add(relTag);
            }
            else
            {
                Tags.Where(t => t.Tag.Text == tag.Text).First().IsMarkForDeletion = false;
            }
            IsDirty = true;
        }

        public void RemoveTag(RelTag tag)
        {
            Tags.Remove(tag);
            IsDirty = true;
        }

        public void RemoveAllTags()
        {
            Tags.Clear();
            IsDirty = true;
        }

        public void SetFrequence(Frequency frequency)
        {
            Frequency = frequency;
            IsDirty = true;
        }

        public void SetImportance(Importance importance)
        {
            Importance = importance;
            IsDirty = true;
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
            IsDirty = true;
        }

        public void SetDate(DateTime date)
        {
            Date = date;
            IsDirty = true;
        }

        public void UpdateAmount(List<TransactionType> types)
        {
            Amount = decimal.Zero;
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

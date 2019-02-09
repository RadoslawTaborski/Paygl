using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Tag : IEntity, IParameter
    {
        public int? Id { get; private set; }
        public string Text { get; private set; }
        public List<RelOperation> Operations { get; private set; }
        public bool IsDirty { get; set; }

        public Tag(int? id, string text)
        {
            Id = id;
            Text = text;
            Operations = new List<RelOperation>();
            IsDirty = true;
        }

        public void SetOperations(IEnumerable<RelOperation> operations)
        {
            Operations = operations.ToList();
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }

        public void AddOperation(RelOperation relOperation)
        {
            Operations.Add(relOperation);
        }

        public void RemoveOperation(RelOperation relOperation)
        {
            relOperation.IsMarkForDeletion = true;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

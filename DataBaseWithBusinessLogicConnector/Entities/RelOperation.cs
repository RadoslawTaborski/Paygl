using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class RelOperation : IEntity
    {
        public int? Id { get; private set; }
        public Operation Operation { get; private set; }
        public int? TagId { get; private set; }
        public bool IsDirty { get; set; }

        public RelOperation(int? id, Operation operation, int? tagId)
        {
            Id = id;
            Operation = operation;
            TagId = tagId;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }
    }
}

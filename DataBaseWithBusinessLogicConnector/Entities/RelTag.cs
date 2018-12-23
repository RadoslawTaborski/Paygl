using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class RelTag: IEntity
    {
        public int? Id { get; private set; }
        public Tag Tag { get; private set; }
        public int? OperationId { get; private set; }
        public bool IsDirty { get; set; }

        public RelTag(int? id, Tag tag, int? operationId)
        {
            Id = id;
            Tag = tag;
            OperationId = operationId;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }
    }
}

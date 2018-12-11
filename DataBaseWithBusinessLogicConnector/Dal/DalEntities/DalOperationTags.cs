using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalOperationTags
    {
        public int Id { get; private set; }
        public int OperationId { get; private set; }
        public int TagId { get; private set; }

        public DalOperationTags(int id, int operationId, int tagId)
        {
            Id = id;
            OperationId = operationId;
            TagId = tagId;
        }
    }
}

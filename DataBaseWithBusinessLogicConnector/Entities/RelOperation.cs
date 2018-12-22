using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class RelOperation : IEntity
    {
        public int Id { get; private set; }
        public Operation Operation { get; private set; }
        public bool IsDirty { get; private set; }

        public RelOperation(int id, Operation operation)
        {
            Id = id;
            Operation = operation;
        }
    }
}

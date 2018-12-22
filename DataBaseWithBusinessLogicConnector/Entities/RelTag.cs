using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class RelTag: IEntity
    {
        public int Id { get; private set; }
        public Tag Tag { get; private set; }
        public bool IsDirty { get; private set; }

        public RelTag(int id, Tag tag)
        {
            Id = id;
            Tag = tag;
        }
    }
}

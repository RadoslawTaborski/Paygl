using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Language : IEntity
    {
        public int Id { get; private set; }
        public string ShortName { get; private set; }
        public string FullName { get; private set; }
        public bool IsDirty { get; private set; }

        public Language(int id, string shortName, string fullName)
        {
            Id = id;
            ShortName = shortName;
            FullName = fullName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalLanguage
    {
        public int Id { get; private set; }
        public string ShortName { get; private set; }
        public string FullName { get; private set; }

        public DalLanguage(int id, string shortName, string fullName)
        {
            Id = id;
            ShortName = shortName;
            FullName = fullName;
        }
    }
}

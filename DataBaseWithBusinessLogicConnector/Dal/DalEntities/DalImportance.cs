using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalImportance
    {
        public int Id { get; private set; }
        public string Text { get; private set; }

        public DalImportance(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalTag
    {
        public int Id { get; private set; }
        public string Text { get; private set; }

        public DalTag(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

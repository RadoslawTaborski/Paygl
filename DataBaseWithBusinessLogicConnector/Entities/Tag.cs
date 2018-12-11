using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Tag
    {
        public int Id { get; private set; }
        public string Text { get; private set; }

        public Tag(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

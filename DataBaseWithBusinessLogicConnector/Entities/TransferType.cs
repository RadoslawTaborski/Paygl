using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class TransferType
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public Language Language { get; private set; }

        public TransferType(int id, string text, Language language)
        {
            Id = id;
            Text = text;
            Language = language;
        }
    }
}

using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public Language Language { get; private set; }
        public List<Operation> Operations { get; private set; }
        public bool IsDirty { get; private set; }

        public Tag(int id, string text, Language language)
        {
            Id = id;
            Text = text;
            Language = language;
            Operations = new List<Operation>();
        }
    }
}

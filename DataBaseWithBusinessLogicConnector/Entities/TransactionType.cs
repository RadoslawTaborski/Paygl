﻿using DataBaseWithBusinessLogicConnector.Interfaces;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class TransactionType : IEntity, IParameter
    {
        public int? Id { get; private set; }
        public string Text { get; private set; }
        public bool IsDirty { get; set; }

        public TransactionType(int? id, string text)
        {
            Id = id;
            Text = text;
            IsDirty = true;
        }

        public void UpdateId(int? id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

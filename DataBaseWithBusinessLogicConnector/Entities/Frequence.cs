﻿using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Frequence : IEntity
    {
        public int? Id { get; private set; }
        public string Text { get; private set; }
        public bool IsDirty { get; set; }

        public Frequence(int? id, string text)
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

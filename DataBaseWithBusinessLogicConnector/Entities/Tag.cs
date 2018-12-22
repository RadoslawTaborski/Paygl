﻿using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public List<RelOperation> Operations { get; private set; }
        public bool IsDirty { get; private set; }

        public Tag(int id, string text)
        {
            Id = id;
            Text = text;
            Operations = new List<RelOperation>();
        }

        public void SetOperations(IEnumerable<RelOperation> operations)
        {
            Operations = operations.ToList();
        }
    }
}

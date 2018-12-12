﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Entities
{
    public class Frequence
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public Language Language { get; private set; }

        public Frequence(int id, string text, Language language)
        {
            Id = id;
            Text = text;
            Language = language;
        }
    }
}

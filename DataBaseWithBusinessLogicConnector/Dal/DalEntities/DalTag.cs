using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalTag : IDalEntity
    {
        public int? Id { get; private set; }
        public string Text { get; private set; }
        public int? LanguageId { get; private set; }

        public DalTag(int? id, string text, int? languageId)
        {
            Id = id;
            Text = text;
            LanguageId = languageId;
        }
    }
}

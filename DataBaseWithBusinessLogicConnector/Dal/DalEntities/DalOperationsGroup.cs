using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.DalEntities
{
    public class DalOperationsGroup: IDalEntity
    {
        public int? Id { get; private set; }
        public int? UserId { get; private set; }
        public string Description { get; private set; }
        public int? FrequenceId { get; private set; }
        public int? ImportanceId { get; private set; }
        public string Date { get; private set; }


        public DalOperationsGroup(int? id, int? userId, string description, int? frequenceId, int? importanceId, string date)
        {
            Id = id;
            UserId = userId;
            Description = description;
            FrequenceId = frequenceId;
            ImportanceId = importanceId;
            Date = date;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Interfaces
{
    public interface IEntity
    {
        int? Id { get; }
        bool IsDirty { get; set; }

        void UpdateId(int? id);
    }
}

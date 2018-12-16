using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Interfaces
{
    public interface IEntity
    {
        bool IsDirty { get; }
    }
}

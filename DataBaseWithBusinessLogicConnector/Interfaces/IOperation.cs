using DataBaseWithBusinessLogicConnector.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Interfaces
{
    public interface IOperation
    {
        decimal Amount { get; }
        TransactionType TransactionType { get; }
        Frequence Frequence { get; }
        Importance Importance { get; }
        DateTime Date { get; }
        List<RelTag> Tags { get; }
        string Description { get; }
    }
}

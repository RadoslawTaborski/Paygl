using DataBaseWithBusinessLogicConnector.Entities;
using System;
using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Interfaces
{
    public interface IOperation
    {
        decimal Amount { get; }
        TransactionType TransactionType { get; }
        Frequency Frequency { get; }
        Importance Importance { get; }
        DateTime Date { get; }
        List<RelTag> Tags { get; }
        string Description { get; }
    }
}

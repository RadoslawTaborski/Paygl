using System;
using System.Collections.Generic;
using System.Text;
using DataBaseWithBusinessLogicConnector.Interfaces;

namespace Analyzer
{
    public class QueryLeafOperation: IQueryItem
    {
        public string Operation { get; private set; }
        public List<IOperation> Result => throw new NotImplementedException();
        public bool OnlyOperations => throw new NotImplementedException();

        public QueryLeafOperation(string operation)
        {
            Operation = operation;
        }

        public void SetOperation(string operation)
        {
            Operation = operation;
        }

        public override string ToString()
        {
            return Operation;
        }

        public void Filter(List<IOperation> all)
        {
            throw new NotImplementedException();
        }
    }
}

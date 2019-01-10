using DataBaseWithBusinessLogicConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayglService.cs.Helpers
{
    public class Group
    {
        public List<IOperation> Operations { get; private set; }

        public Group()
        {
            Operations = new List<IOperation>();
        }

        public void AddRange(List<IOperation> operations)
        {
            Operations.AddRange(operations);
        }

        public void Add(IOperation operation)
        {
            Operations.Add(operation);
        }
    }
}

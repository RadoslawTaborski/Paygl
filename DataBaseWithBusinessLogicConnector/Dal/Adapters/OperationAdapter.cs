using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationAdapter : IAdapter<DalOperation>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalOperation> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public DalOperation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalOperation entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalOperation entity)
        {
            throw new NotImplementedException();
        }

        private static class Queries
        {
            const string select = "";
            const string insert = "";
            const string delete = "";
            const string update = "";
        }
    }
}

using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationTagAdapter : IAdapter<DalOperationTags>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalOperationTags> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public DalOperationTags GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalOperationTags entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalOperationTags entity)
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

using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationDetailsAdapter : IAdapter<DalOperationDetails>
    {
        private DbConnector _connection;

        public OperationDetailsAdapter(DbConnector connection)
        {
            _connection = connection;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalOperationDetails> GetAll(string filter = "")
        {
            throw new NotImplementedException();
        }

        public DalOperationDetails GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalOperationDetails entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalOperationDetails entity)
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

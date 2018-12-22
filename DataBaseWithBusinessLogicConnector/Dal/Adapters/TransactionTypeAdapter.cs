using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class TransactionTypeAdapter : IAdapter<DalTransactionType>
    {
        private DbConnector _connection;

        public TransactionTypeAdapter(DbConnector connection)
        {
            _connection = connection;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalTransactionType> GetAll(string filter = "")
        {
            throw new NotImplementedException();
        }

        public DalTransactionType GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalTransactionType entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalTransactionType entity)
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

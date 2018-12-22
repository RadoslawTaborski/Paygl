using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class ImportanceAdapter : IAdapter<DalImportance>
    {
        private DbConnector _connection;

        public ImportanceAdapter(DbConnector connection)
        {
            _connection = connection;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalImportance> GetAll(string filter = "")
        {
            throw new NotImplementedException();
        }

        public DalImportance GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalImportance entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalImportance entity)
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
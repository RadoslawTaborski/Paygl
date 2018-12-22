using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class LanguageAdapter : IAdapter<DalLanguage>
    {
        private DbConnector _connection;

        public LanguageAdapter(DbConnector connection)
        {
            _connection = connection;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalLanguage> GetAll(string filter = "")
        {
            throw new NotImplementedException();
        }

        public DalLanguage GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalLanguage entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalLanguage entity)
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

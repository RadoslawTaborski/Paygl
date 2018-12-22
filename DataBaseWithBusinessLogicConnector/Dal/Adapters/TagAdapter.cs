using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class TagAdapter : IAdapter<DalTag>
    {
        private DbConnector _connection;

        public TagAdapter(DbConnector connection)
        {
            _connection = connection;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalTag> GetAll(string filter = "")
        {
            throw new NotImplementedException();
        }

        public DalTag GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalTag entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalTag entity)
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

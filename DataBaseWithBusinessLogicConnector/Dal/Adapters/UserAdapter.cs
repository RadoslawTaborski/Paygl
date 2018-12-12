using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class UserAdapter : IAdapter<DalUser>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalUser> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public DalUser GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalUser entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalUser entity)
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

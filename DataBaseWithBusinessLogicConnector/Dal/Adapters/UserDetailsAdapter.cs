using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class UserDetailsAdapter : IAdapter<DalUserDetails>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalUserDetails> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public DalUserDetails GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalUserDetails entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalUserDetails entity)
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

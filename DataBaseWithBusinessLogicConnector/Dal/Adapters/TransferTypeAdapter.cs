using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class TransferTypeAdapter : IAdapter<DalTransferType>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalTransferType> GetAll(string filter)
        {
            throw new NotImplementedException();
        }

        public DalTransferType GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalTransferType entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, DalTransferType entity)
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

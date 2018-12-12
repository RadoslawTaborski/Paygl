using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TransactionTypeMapper : IMapper<TransactionType, DalTransactionType>
    {
        public IEnumerable<TransactionType> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalTransactionType> dataEntities)
        {
            throw new NotImplementedException();
        }

        public TransactionType ConvertToBusinessLogicEntity(DalTransactionType dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalTransactionType> ConvertToDALEntitiesCollection(IEnumerable<TransactionType> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalTransactionType ConvertToDALEntity(TransactionType businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

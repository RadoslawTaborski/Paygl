using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TransactionTypeMapper : IMapper<TransactionType, DalTransactionType>
    {
        private Language _language;
        public TransactionTypeMapper(Language language)
        {
            _language = language;
        }

        public void Update(Language language)
        {
            _language = language;
        }

        public IEnumerable<TransactionType> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalTransactionType> dataEntities)
        {
            var result = new List<TransactionType>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public TransactionType ConvertToBusinessLogicEntity(DalTransactionType dataEntity)
        {
            var result = new TransactionType(dataEntity.Id, dataEntity.Text, _language);
            return result;
        }

        public IEnumerable<DalTransactionType> ConvertToDALEntitiesCollection(IEnumerable<TransactionType> dataEntities)
        {
            var result = new List<DalTransactionType>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalTransactionType ConvertToDALEntity(TransactionType businessEntity)
        {
            var result = new DalTransactionType(businessEntity.Id, businessEntity.Text, businessEntity.Id);
            return result;
        }
    }
}

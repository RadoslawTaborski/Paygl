using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TransferTypeMapper : IMapper<TransferType, DalTransferType>
    {
        private Language _language;
        public TransferTypeMapper(Language language)
        {
            _language = language;
        }

        public void Update(Language language)
        {
            _language = language;
        }

        public IEnumerable<TransferType> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalTransferType> dataEntities)
        {
            var result = new List<TransferType>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public TransferType ConvertToBusinessLogicEntity(DalTransferType dataEntity)
        {
            var result = new TransferType(dataEntity.Id, dataEntity.Text, _language);
            return result;
        }

        public IEnumerable<DalTransferType> ConvertToDALEntitiesCollection(IEnumerable<TransferType> dataEntities)
        {
            var result = new List<DalTransferType>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalTransferType ConvertToDALEntity(TransferType businessEntity)
        {
            var result = new DalTransferType(businessEntity.Id, businessEntity.Text, businessEntity.Id);
            return result;
        }
    }
}

using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TransferTypeMapper
    {
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
            var result = new TransferType(dataEntity.Id, dataEntity.Text);
            result.IsDirty = false;
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

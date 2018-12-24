using System.Collections.Generic;
using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationDetailsMapper
    {
        public IEnumerable<OperationDetails> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperationDetails> dataEntities)
        {
            var result = new List<OperationDetails>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public OperationDetails ConvertToBusinessLogicEntity(DalOperationDetails dataEntity)
        {
            var result = new OperationDetails(dataEntity.Id, dataEntity.Name, dataEntity.Quantity,dataEntity.Amount);
            result.IsDirty = false;
            return result;
        }

        public IEnumerable<DalOperationDetails> ConvertToDALEntitiesCollection(IEnumerable<OperationDetails> dataEntities, int parentId)
        {
            var result = new List<DalOperationDetails>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item,parentId));
            }

            return result;
        }

        public DalOperationDetails ConvertToDALEntity(OperationDetails businessEntity, int parentId)
        {
            var result = new DalOperationDetails(businessEntity.Id, parentId, businessEntity.Name, businessEntity.Quantity, businessEntity.Amount);
            return result;
        }
    }
}

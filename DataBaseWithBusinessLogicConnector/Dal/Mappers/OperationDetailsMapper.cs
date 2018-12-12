using System.Collections.Generic;
using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationDetailsMapper : IMapper<OperationDetails, DalOperationDetails>
    {
        public IEnumerable<OperationDetails> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperationDetails> dataEntities)
        {
            throw new System.NotImplementedException();
        }

        public OperationDetails ConvertToBusinessLogicEntity(DalOperationDetails dataEntity)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<DalOperationDetails> ConvertToDALEntitiesCollection(IEnumerable<OperationDetails> dataEntities)
        {
            throw new System.NotImplementedException();
        }

        public DalOperationDetails ConvertToDALEntity(OperationDetails businessEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}

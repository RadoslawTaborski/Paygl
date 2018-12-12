using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationMapper : IMapper<Operation, DalOperation>
    {
        public IEnumerable<Operation> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperation> dataEntities)
        {
            throw new NotImplementedException();
        }

        public Operation ConvertToBusinessLogicEntity(DalOperation dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalOperation> ConvertToDALEntitiesCollection(IEnumerable<Operation> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalOperation ConvertToDALEntity(Operation businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

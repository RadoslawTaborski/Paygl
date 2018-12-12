using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TransferTypeMapper : IMapper<TransferType, DalTransferType>
    {
        public IEnumerable<TransferType> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalTransferType> dataEntities)
        {
            throw new NotImplementedException();
        }

        public TransferType ConvertToBusinessLogicEntity(DalTransferType dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalTransferType> ConvertToDALEntitiesCollection(IEnumerable<TransferType> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalTransferType ConvertToDALEntity(TransferType businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

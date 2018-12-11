using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal.DalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationDetailMapper : IMapper<OperationDetail, DalOperationDetail>
    {
        public IEnumerable<OperationDetail> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperationDetail> dataEntities)
        {
            throw new NotImplementedException();
        }

        public OperationDetail ConvertToBusinessLogicEntity(DalOperationDetail dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalOperationDetail> ConvertToDALEntitiesCollection(IEnumerable<OperationDetail> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalOperationDetail ConvertToDALEntity(OperationDetail businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

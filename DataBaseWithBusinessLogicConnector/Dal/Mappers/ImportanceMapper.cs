using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class ImportanceMapper : IMapper<Importance, DalImportance>
    {
        public IEnumerable<Importance> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalImportance> dataEntities)
        {
            throw new NotImplementedException();
        }

        public Importance ConvertToBusinessLogicEntity(DalImportance dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalImportance> ConvertToDALEntitiesCollection(IEnumerable<Importance> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalImportance ConvertToDALEntity(Importance businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

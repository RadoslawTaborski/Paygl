using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TagMapper : IMapper<Tag, DalTag>
    {
        public IEnumerable<Tag> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalTag> dataEntities)
        {
            throw new NotImplementedException();
        }

        public Tag ConvertToBusinessLogicEntity(DalTag dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalTag> ConvertToDALEntitiesCollection(IEnumerable<Tag> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalTag ConvertToDALEntity(Tag businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

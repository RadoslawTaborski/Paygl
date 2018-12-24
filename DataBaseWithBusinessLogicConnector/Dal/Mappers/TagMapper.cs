using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class TagMapper
    {
        public IEnumerable<Tag> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalTag> dataEntities)
        {
            var result = new List<Tag>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public Tag ConvertToBusinessLogicEntity(DalTag dataEntity)
        {
            var result = new Tag(dataEntity.Id, dataEntity.Text);
            result.IsDirty = false;
            return result;
        }

        public IEnumerable<DalTag> ConvertToDALEntitiesCollection(IEnumerable<Tag> dataEntities)
        {
            var result = new List<DalTag>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalTag ConvertToDALEntity(Tag businessEntity)
        {
            var result = new DalTag(businessEntity.Id, businessEntity.Text, businessEntity.Id);
            return result;
        }
    }
}

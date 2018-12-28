using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationsGroupRelationMapper
    {
        public List<Tag> _tags;

        public OperationsGroupRelationMapper() { }

        public void Update(List<Tag> tags)
        {
            _tags = tags;
        }

        public IEnumerable<RelTag> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperationsGroupTag> dataEntities)
        {
            var result1 = new List<RelTag>();
            foreach (var item in dataEntities)
            {
                result1.Add(ConvertToBusinessLogicEntity(item));
            }

            return result1;
        }

        public RelTag ConvertToBusinessLogicEntity(DalOperationsGroupTag dataEntity)
        {
            var result1 = new RelTag(dataEntity.Id, _tags.Where(t => t.Id == dataEntity.TagId).First(), dataEntity.OperationsGroupId);
            result1.IsDirty = false;
            return result1;
        }

        public IEnumerable<DalOperationsGroupTag> ConvertToDALEntitiesCollection(IEnumerable<RelTag> dataEntities1, OperationsGroup dataEntity2)
        {
            var result = new List<DalOperationsGroupTag>();
            for (var i = 0; i < dataEntities1.Count(); ++i)
            {
                result.Add(ConvertToDALEntity(dataEntities1.ElementAt(i), dataEntity2));
            }

            return result;
        }

        public DalOperationsGroupTag ConvertToDALEntity(RelTag businessEntity1, OperationsGroup businessEntity2)
        {
            var result = new DalOperationsGroupTag(businessEntity1.Id, businessEntity2.Id, businessEntity1.Tag.Id);
            return result;
        }
    }
}

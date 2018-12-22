using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class RelationMapper
    {
        public List<Operation> _operations;
        public List<Tag> _tags;

        public RelationMapper(List<Operation> operations, List<Tag> tags)
        {
            _operations = operations;
            _tags = tags;
        }

        public void Update(List<Operation> operations, List<Tag> tags)
        {
            _operations = operations;
            _tags = tags;
        }

        public (IEnumerable<RelTag>, IEnumerable<RelOperation>) ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperationTags> dataEntities)
        {
            var result1 = new List<RelTag>();
            var result2 = new List<RelOperation>();
            foreach (var item in dataEntities)
            {
                result1.Add(ConvertToBusinessLogicEntity(item).Item1);
                result2.Add(ConvertToBusinessLogicEntity(item).Item2);
            }

            return (result1,result2);
        }

        public (RelTag, RelOperation) ConvertToBusinessLogicEntity(DalOperationTags dataEntity)
        {
            var result1 = new RelTag(dataEntity.Id, _tags.Where(t=>t.Id==dataEntity.TagId).First(), dataEntity.OperationId);
            var result2 = new RelOperation(dataEntity.Id, _operations.Where(o => o.Id == dataEntity.OperationId).First(), dataEntity.TagId);
            return (result1,result2);
        }

        public IEnumerable<DalOperationTags> ConvertToDALEntitiesCollection(IEnumerable<RelTag> dataEntities1, IEnumerable<RelOperation> dataEntities2)
        {
            var result = new List<DalOperationTags>();
            for (var i = 0; i<dataEntities1.Count();++i)
            {
                result.Add(ConvertToDALEntity(dataEntities1.ElementAt(i), dataEntities2.ElementAt(i)));
            }

            return result;
        }

        public DalOperationTags ConvertToDALEntity(RelTag businessEntity1, RelOperation businessEntity2)
        {
            var result = new DalOperationTags(businessEntity1.Id, businessEntity1.Tag.Id, businessEntity2.Operation.Id);
            return result;
        }
    }
}

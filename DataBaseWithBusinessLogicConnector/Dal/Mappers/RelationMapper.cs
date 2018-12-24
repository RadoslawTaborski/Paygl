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

        public RelationMapper(){}

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
            result1.IsDirty = false;
            result2.IsDirty = false;
            return (result1,result2);
        }

        public IEnumerable<DalOperationTags> ConvertToDALEntitiesCollection(IEnumerable<RelTag> dataEntities1, Operation dataEntity2)
        {
            var result = new List<DalOperationTags>();
            for (var i = 0; i<dataEntities1.Count();++i)
            {
                result.Add(ConvertToDALEntity(dataEntities1.ElementAt(i), dataEntity2));
            }

            return result;
        }

        public IEnumerable<DalOperationTags> ConvertToDALEntitiesCollection(IEnumerable<RelOperation> dataEntities1, Tag dataEntity2)
        {
            var result = new List<DalOperationTags>();
            for (var i = 0; i < dataEntities1.Count(); ++i)
            {
                result.Add(ConvertToDALEntity(dataEntities1.ElementAt(i), dataEntity2));
            }

            return result;
        }

        public DalOperationTags ConvertToDALEntity(RelTag businessEntity1, Operation businessEntity2)
        {
            var result = new DalOperationTags(businessEntity1.Id, businessEntity2.Id, businessEntity1.Tag.Id);
            return result;
        }

        public DalOperationTags ConvertToDALEntity(RelOperation businessEntity1, Tag businessEntity2)
        {
            var result = new DalOperationTags(businessEntity1.Id, businessEntity1.Operation.Id, businessEntity2.Id);
            return result;
        }
    }
}

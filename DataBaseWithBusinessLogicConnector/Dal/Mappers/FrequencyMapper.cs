using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class FrequencyMapper
    {
        public IEnumerable<Frequency> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalFrequency> dataEntities)
        {
            var result = new List<Frequency>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public Frequency ConvertToBusinessLogicEntity(DalFrequency dataEntity)
        {
            var result = new Frequency(dataEntity.Id, dataEntity.Text) {IsDirty = false};
            return result;
        }

        public IEnumerable<DalFrequency> ConvertToDALEntitiesCollection(IEnumerable<Frequency> dataEntities)
        {
            var result = new List<DalFrequency>();
            foreach(var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalFrequency ConvertToDALEntity(Frequency businessEntity)
        {
            var result = new DalFrequency(businessEntity.Id, businessEntity.Text, businessEntity.Id);
            return result;
        }
    }
}

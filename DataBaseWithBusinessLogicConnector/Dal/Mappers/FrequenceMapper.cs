using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class FrequenceMapper
    {
        public IEnumerable<Frequence> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalFrequence> dataEntities)
        {
            var result = new List<Frequence>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public Frequence ConvertToBusinessLogicEntity(DalFrequence dataEntity)
        {
            var result = new Frequence(dataEntity.Id,dataEntity.Text);
            result.IsDirty = false;
            return result;
        }

        public IEnumerable<DalFrequence> ConvertToDALEntitiesCollection(IEnumerable<Frequence> dataEntities)
        {
            var result = new List<DalFrequence>();
            foreach(var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalFrequence ConvertToDALEntity(Frequence businessEntity)
        {
            var result = new DalFrequence(businessEntity.Id, businessEntity.Text, businessEntity.Id);
            return result;
        }
    }
}

using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class FrequenceMapper : IMapper<Frequence, DalFrequence>
    {
        public IEnumerable<Frequence> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalFrequence> dataEntities)
        {
            throw new NotImplementedException();
        }

        public Frequence ConvertToBusinessLogicEntity(DalFrequence dataEntity)
        {
            var result = new Frequence(dataEntity.Id,dataEntity.Text);
            return result;
        }

        public IEnumerable<DalFrequence> ConvertToDALEntitiesCollection(IEnumerable<Frequence> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalFrequence ConvertToDALEntity(Frequence businessEntity)
        {
            var result = new DalFrequence(businessEntity.Id, businessEntity.Text);
            return result;
        }
    }
}

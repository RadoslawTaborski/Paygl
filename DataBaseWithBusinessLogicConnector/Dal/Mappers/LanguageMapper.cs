using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class LanguageMapper : IMapper<LanguageMapper, DalLanguage>
    {
        public IEnumerable<LanguageMapper> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalLanguage> dataEntities)
        {
            throw new NotImplementedException();
        }

        public LanguageMapper ConvertToBusinessLogicEntity(DalLanguage dataEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalLanguage> ConvertToDALEntitiesCollection(IEnumerable<LanguageMapper> dataEntities)
        {
            throw new NotImplementedException();
        }

        public DalLanguage ConvertToDALEntity(LanguageMapper businessEntity)
        {
            throw new NotImplementedException();
        }
    }
}

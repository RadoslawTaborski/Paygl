using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class LanguageMapper
    {
        public IEnumerable<Language> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalLanguage> dataEntities)
        {
            var result = new List<Language>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public Language ConvertToBusinessLogicEntity(DalLanguage dataEntity)
        {
            var result = new Language(dataEntity.Id, dataEntity.ShortName, dataEntity.FullName);
            return result;
        }

        public IEnumerable<DalLanguage> ConvertToDALEntitiesCollection(IEnumerable<Language> dataEntities)
        {
            var result = new List<DalLanguage>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalLanguage ConvertToDALEntity(Language businessEntity)
        {
            var result = new DalLanguage(businessEntity.Id, businessEntity.ShortName, businessEntity.FullName);
            return result;
        }
    }
}

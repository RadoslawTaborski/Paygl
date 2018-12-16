﻿using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class ImportanceMapper : IMapper<Importance, DalImportance>
    {
        private Language _language;
        public ImportanceMapper(Language language)
        {
            _language = language;
        }

        public void Update(Language language)
        {
            _language = language;
        }

        public IEnumerable<Importance> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalImportance> dataEntities)
        {
            var result = new List<Importance>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public Importance ConvertToBusinessLogicEntity(DalImportance dataEntity)
        {
            var result = new Importance(dataEntity.Id, dataEntity.Text, _language);
            return result;
        }

        public IEnumerable<DalImportance> ConvertToDALEntitiesCollection(IEnumerable<Importance> dataEntities)
        {
            var result = new List<DalImportance>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalImportance ConvertToDALEntity(Importance businessEntity)
        {
            var result = new DalImportance(businessEntity.Id, businessEntity.Text, businessEntity.Id);
            return result;
        }
    }
}

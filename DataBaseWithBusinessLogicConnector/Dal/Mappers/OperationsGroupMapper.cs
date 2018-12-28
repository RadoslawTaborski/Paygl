using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationsGroupMapper : IMapper<OperationsGroup, DalOperationsGroup>
    {
        public List<Importance> _importances;
        public List<Frequence> _frequencies;
        public User _user;

        public void Update(User user, List<Importance> importances, List<Frequence> frequencies)
        {
            _user = user;
            _importances = importances;
            _frequencies = frequencies;
        }

        public IEnumerable<OperationsGroup> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperationsGroup> dataEntities)
        {
            var result = new List<OperationsGroup>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public OperationsGroup ConvertToBusinessLogicEntity(DalOperationsGroup dataEntity)
        {
            var importance = _importances.Where(i => i.Id == dataEntity.ImportanceId).First();
            var frequence = _frequencies.Where(f => f.Id == dataEntity.FrequenceId).First();
            CultureInfo culture = new CultureInfo("pl-PL");
            DateTime tempDate = Convert.ToDateTime(dataEntity.Date, culture);
            var result = new OperationsGroup(dataEntity.Id, _user, dataEntity.Description, frequence, importance, tempDate)
            {
                IsDirty = false
            };
            return result;
        }

        public IEnumerable<DalOperationsGroup> ConvertToDALEntitiesCollection(IEnumerable<OperationsGroup> dataEntities)
        {
            var result = new List<DalOperationsGroup>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalOperationsGroup ConvertToDALEntity(OperationsGroup businessEntity)
        {
            if (businessEntity == null || businessEntity.User == null ||  businessEntity.Frequence == null || businessEntity.Importance == null)
            {
                throw new ArgumentException("wrong parameters");
            }
            var result = new DalOperationsGroup(businessEntity.Id, businessEntity.User.Id, businessEntity.Description, businessEntity.Frequence.Id, businessEntity.Importance.Id, businessEntity.Date.ToString("yyyy-MM-dd"));
            return result;
        }
    }
}

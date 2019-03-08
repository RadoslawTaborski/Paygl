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
        public List<Frequency> _frequencies;
        public User _user;

        public void Update(User user, List<Importance> importances, List<Frequency> frequencies)
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
            var importance = _importances.First(i => i.Id == dataEntity.ImportanceId);
            var frequency = _frequencies.First(f => f.Id == dataEntity.FrequencyId);
            var tempDate = DateTime.ParseExact(dataEntity.Date, Properties.strings.dateFullFormat, CultureInfo.CurrentCulture);
            var result = new OperationsGroup(dataEntity.Id, _user, dataEntity.Description, frequency, importance, tempDate)
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
            if (businessEntity?.User == null || businessEntity.Frequency == null || businessEntity.Importance == null)
            {
                throw new ArgumentException(Properties.strings.ExWrongParameters);
            }
            var result = new DalOperationsGroup(businessEntity.Id, businessEntity.User.Id, businessEntity.Description, businessEntity.Frequency.Id, businessEntity.Importance.Id, businessEntity.Date.ToString("yyyy-MM-dd"));
            return result;
        }
    }
}

using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Mappers
{
    public class OperationMapper : IMapper<Operation, DalOperation>
    {
        public List<OperationsGroup> _groups;
        public List<Importance> _importances;
        public List<Frequence> _frequencies;
        public List<TransactionType> _transactionTypes;
        public List<TransferType> _transferTypes;
        public User _user;

        public void Update(User user, List<OperationsGroup> groups, List<Importance> importances, List<Frequence> frequencies, List<TransactionType> transactionTypes, List<TransferType> transferTypes)
        {
            _user = user;
            _groups = groups;
            _importances = importances;
            _frequencies = frequencies;
            _transactionTypes = transactionTypes;
            _transferTypes = transferTypes;
        }

        public IEnumerable<Operation> ConvertToBusinessLogicEntitiesCollection(IEnumerable<DalOperation> dataEntities)
        {
            var result = new List<Operation>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToBusinessLogicEntity(item));
            }

            return result;
        }

        public Operation ConvertToBusinessLogicEntity(DalOperation dataEntity)
        {
            var group = dataEntity.ParentId != null?_groups.Where(o => o.Id == dataEntity.ParentId).First():null;
            var transaction = _transactionTypes.Where(t => t.Id == dataEntity.TransactionTypeId).First();
            var transfer = _transferTypes.Where(t => t.Id == dataEntity.TransferTypeId).First();
            var importance = _importances.Where(i => i.Id == dataEntity.ImportanceId).First();
            var frequence = _frequencies.Where(f => f.Id == dataEntity.FrequenceId).First();
            CultureInfo culture = new CultureInfo("pl-PL");
            DateTime tempDate = Convert.ToDateTime(dataEntity.Date, culture);
            var result = new Operation(dataEntity.Id, group, _user, dataEntity.Description, dataEntity.Amount, transaction,transfer,frequence,importance,tempDate,dataEntity.ReceiptPath);
            result.IsDirty = false;
            return result;
        }

        public IEnumerable<DalOperation> ConvertToDALEntitiesCollection(IEnumerable<Operation> dataEntities)
        {
            var result = new List<DalOperation>();
            foreach (var item in dataEntities)
            {
                result.Add(ConvertToDALEntity(item));
            }

            return result;
        }

        public DalOperation ConvertToDALEntity(Operation businessEntity)
        {
            if (businessEntity == null || businessEntity.User==null || businessEntity.TransactionType == null || businessEntity.TransferType == null || businessEntity.Frequence==null || businessEntity.Importance == null)
            {
                throw new ArgumentException("wrong parameters");
            }
            var result = new DalOperation(businessEntity.Id,businessEntity.Parent?.Id, businessEntity.User.Id, businessEntity.Description, businessEntity.Amount, businessEntity.TransactionType.Id,businessEntity.TransferType.Id,businessEntity.Frequence.Id,businessEntity.Importance.Id,businessEntity.Date.ToString("yyyy-MM-dd"),businessEntity.ReceiptPath);
            return result;
        }
    }
}

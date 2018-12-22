using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationAdapter : IAdapter<DalOperation>
    {
        private readonly string TABLE = "operations";
        private readonly string[] COLUMNS = { "parent_id", "user_id", "amount", "transfer_type_id", "transaction_type_id", "frequent_id", "importance_id", "date", "receipt_path" };

        private AdapterHelper _adapterHelper;

        public OperationAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
        }

        public void Delete(DalOperation entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalOperation> GetAll(string filter = "")
        {
            var result = new List<DalOperation>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalOperation(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()), decimal.Parse(dataRow[3].ToString()), int.Parse(dataRow[4].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[7].ToString()), dataRow[8].ToString(), dataRow[9].ToString()));
            }

            return result;
        }

        public DalOperation GetById(int id)
        {
            DalOperation result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalOperation(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()), decimal.Parse(dataRow[3].ToString()), int.Parse(dataRow[4].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[7].ToString()), dataRow[8].ToString(), dataRow[9].ToString());
            }

            return result;
        }

        public void Insert(DalOperation entity)
        {
            _adapterHelper.Insert(entity.ParentId.ToString(), entity.UserId.ToString(), entity.Amount.ToString(), entity.TransferTypeId.ToString(), entity.TransactionTypeId.ToString(), entity.FrequenceId.ToString(), entity.ImportanceId.ToString(), entity.Date, entity.ReceiptPath);
        }

        public void Update(DalOperation entity)
        {
            _adapterHelper.Update(entity.Id, entity.ParentId.ToString(), entity.UserId.ToString(), entity.Amount.ToString(), entity.TransferTypeId.ToString(), entity.TransactionTypeId.ToString(), entity.FrequenceId.ToString(), entity.ImportanceId.ToString(), entity.Date, entity.ReceiptPath);
        }
    }
}

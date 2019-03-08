using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationAdapter : IAdapter<DalOperation>
    {
        private const string Table = "operations";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["parent_id"] = DataType.IntegerNullable,
            ["user_id"] = DataType.IntegerNullable,
            ["description"] = DataType.String,
            ["amount"] = DataType.Decimal,
            ["transfer_type_id"] = DataType.IntegerNullable,
            ["transaction_type_id"] = DataType.IntegerNullable,
            ["frequent_id"] = DataType.IntegerNullable,
            ["importance_id"] = DataType.IntegerNullable,
            ["date"] = DataType.String,
            ["receipt_path"] = DataType.String,
        };

        private readonly AdapterHelper _adapterHelper;

        public OperationAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
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
                var parentId = int.TryParse(dataRow[1].ToString(), out var tempVal) ? tempVal : (int?)null;

                result.Add(new DalOperation(int.Parse(dataRow[0].ToString()), parentId, int.Parse(dataRow[2].ToString()), dataRow[3].ToString(), decimal.Parse(dataRow[4].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[7].ToString()), int.Parse(dataRow[8].ToString()), dataRow[9].ToString(), dataRow[10].ToString()));
            }

            return result;
        }

        public DalOperation GetById(int? id)
        {
            DalOperation result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                var parentId = int.TryParse(dataRow[1].ToString(), out var tempVal) ? tempVal : (int?)null;

                result = new DalOperation(int.Parse(dataRow[0].ToString()), parentId, int.Parse(dataRow[2].ToString()), dataRow[3].ToString(), decimal.Parse(dataRow[4].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[7].ToString()), int.Parse(dataRow[8].ToString()), dataRow[9].ToString(), dataRow[10].ToString());
            }

            return result;
        }

        public int Insert(DalOperation entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var parentId = _adapterHelper.ToStr(entity.ParentId, _columns["parent_id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, _columns["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, _columns["description"]);
            var amount = _adapterHelper.ToStr(entity.Amount, _columns["amount"]);
            var transferTypeId = _adapterHelper.ToStr(entity.TransferTypeId, _columns["transfer_type_id"]);
            var transactionTypeId = _adapterHelper.ToStr(entity.TransactionTypeId, _columns["transaction_type_id"]);
            var frequencyId = _adapterHelper.ToStr(entity.FrequencyId, _columns["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, _columns["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, _columns["date"]);
            var receiptPath = _adapterHelper.ToStr(entity.ReceiptPath, _columns["receipt_path"]);
            return _adapterHelper.Insert(id, parentId,userId,description,amount,transferTypeId,transactionTypeId,frequencyId,importanceId,date,receiptPath);
        }

        public void Update(DalOperation entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var parentId = _adapterHelper.ToStr(entity.ParentId, _columns["parent_id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, _columns["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, _columns["description"]);
            var amount = _adapterHelper.ToStr(entity.Amount, _columns["amount"]);
            var transferTypeId = _adapterHelper.ToStr(entity.TransferTypeId, _columns["transfer_type_id"]);
            var transactionTypeId = _adapterHelper.ToStr(entity.TransactionTypeId, _columns["transaction_type_id"]);
            var frequenceyId = _adapterHelper.ToStr(entity.FrequencyId, _columns["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, _columns["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, _columns["date"]);
            var receiptPath = _adapterHelper.ToStr(entity.ReceiptPath, _columns["receipt_path"]);
            _adapterHelper.Update(id, id, parentId, userId, description, amount, transferTypeId, transactionTypeId, frequenceyId, importanceId, date, receiptPath);
        }
    }
}

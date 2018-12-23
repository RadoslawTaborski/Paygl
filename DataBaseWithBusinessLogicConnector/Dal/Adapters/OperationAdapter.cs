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
        private readonly Dictionary<string, DataType> COLUMNS = new Dictionary<string, DataType>
        {
            ["id"] = DataType.INTEGER_NULLABLE,
            ["parent_id"] = DataType.INTEGER_NULLABLE,
            ["user_id"] = DataType.INTEGER_NULLABLE,
            ["description"] = DataType.STRING,
            ["amount"] = DataType.DECIMAL,
            ["transfer_type_id"] = DataType.INTEGER_NULLABLE,
            ["transaction_type_id"] = DataType.INTEGER_NULLABLE,
            ["frequent_id"] = DataType.INTEGER_NULLABLE,
            ["importance_id"] = DataType.INTEGER_NULLABLE,
            ["date"] = DataType.STRING,
            ["receipt_path"] = DataType.STRING,
        };

        private AdapterHelper _adapterHelper;

        public OperationAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.Keys.ToList());
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
                result.Add(new DalOperation(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()), dataRow[3].ToString(), decimal.Parse(dataRow[4].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[7].ToString()), int.Parse(dataRow[8].ToString()), dataRow[9].ToString(), dataRow[10].ToString()));
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
                result = new DalOperation(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()), dataRow[3].ToString(), decimal.Parse(dataRow[4].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[7].ToString()), int.Parse(dataRow[8].ToString()), dataRow[9].ToString(), dataRow[10].ToString());
            }

            return result;
        }

        public int Insert(DalOperation entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var parentId = _adapterHelper.ToStr(entity.ParentId, COLUMNS["parent_id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, COLUMNS["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, COLUMNS["description"]);
            var amount = _adapterHelper.ToStr(entity.Amount, COLUMNS["amount"]);
            var transferTypeId = _adapterHelper.ToStr(entity.TransferTypeId, COLUMNS["transfer_type_id"]);
            var transactionTypeId = _adapterHelper.ToStr(entity.TransactionTypeId, COLUMNS["transaction_type_id"]);
            var frequenceId = _adapterHelper.ToStr(entity.FrequenceId, COLUMNS["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, COLUMNS["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, COLUMNS["date"]);
            var receiptPath = _adapterHelper.ToStr(entity.ReceiptPath, COLUMNS["receipt_path"]);
            return _adapterHelper.Insert(id, parentId,userId,description,amount,transferTypeId,transactionTypeId,frequenceId,importanceId,date,receiptPath);
        }

        public void Update(DalOperation entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var parentId = _adapterHelper.ToStr(entity.ParentId, COLUMNS["parent_id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, COLUMNS["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, COLUMNS["description"]);
            var amount = _adapterHelper.ToStr(entity.Amount, COLUMNS["amount"]);
            var transferTypeId = _adapterHelper.ToStr(entity.TransferTypeId, COLUMNS["transfer_type_id"]);
            var transactionTypeId = _adapterHelper.ToStr(entity.TransactionTypeId, COLUMNS["transaction_type_id"]);
            var frequenceId = _adapterHelper.ToStr(entity.FrequenceId, COLUMNS["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, COLUMNS["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, COLUMNS["date"]);
            var receiptPath = _adapterHelper.ToStr(entity.ReceiptPath, COLUMNS["receipt_path"]);
            _adapterHelper.Update(id, parentId, userId, description, amount, transferTypeId, transactionTypeId, frequenceId, importanceId, date, receiptPath);
        }
    }
}

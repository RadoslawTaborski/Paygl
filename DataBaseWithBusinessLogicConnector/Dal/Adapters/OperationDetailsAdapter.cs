using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationDetailsAdapter : IAdapter<DalOperationDetails>
    {
        private readonly string TABLE = "operation_details";
        private readonly Dictionary<string, DataType> COLUMNS = new Dictionary<string, DataType>
        {
            ["id"] = DataType.INTEGER_NULLABLE,
            ["operation_id"] = DataType.INTEGER_NULLABLE,
            ["name"] = DataType.STRING,
            ["quantity"] = DataType.DOUBLE,
            ["amount"] = DataType.DECIMAL,
        };

        private AdapterHelper _adapterHelper;

        public OperationDetailsAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.Keys.ToList());
        }

        public void Delete(DalOperationDetails entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalOperationDetails> GetAll(string filter = "")
        {
            var result = new List<DalOperationDetails>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalOperationDetails(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), dataRow[2].ToString(), double.Parse(dataRow[3].ToString()), decimal.Parse(dataRow[4].ToString())));
            }

            return result;
        }

        public DalOperationDetails GetById(int? id)
        {
            DalOperationDetails result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalOperationDetails(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), dataRow[2].ToString(), double.Parse(dataRow[3].ToString()), decimal.Parse(dataRow[4].ToString()));
            }

            return result;
        }

        public int Insert(DalOperationDetails entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationId, COLUMNS["operation_id"]);
            var name = _adapterHelper.ToStr(entity.Name, COLUMNS["name"]);
            var quantity = _adapterHelper.ToStr(entity.Quantity, COLUMNS["quantity"]);
            var amount = _adapterHelper.ToStr(entity.Amount, COLUMNS["amount"]);
            return _adapterHelper.Insert(id, operationId, name, quantity, amount);
        }

        public void Update(DalOperationDetails entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationId, COLUMNS["operation_id"]);
            var name = _adapterHelper.ToStr(entity.Name, COLUMNS["name"]);
            var quantity = _adapterHelper.ToStr(entity.Quantity, COLUMNS["quantity"]);
            var amount = _adapterHelper.ToStr(entity.Amount, COLUMNS["amount"]);
            _adapterHelper.Update(id, operationId, name, quantity, amount);
        }
    }
}

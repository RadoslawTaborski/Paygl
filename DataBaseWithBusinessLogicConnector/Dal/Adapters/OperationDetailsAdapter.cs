using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationDetailsAdapter : IAdapter<DalOperationDetails>
    {
        private const string Table = "operation_details";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["operation_id"] = DataType.IntegerNullable,
            ["name"] = DataType.String,
            ["quantity"] = DataType.Double,
            ["amount"] = DataType.Decimal,
        };

        private readonly AdapterHelper _adapterHelper;

        public OperationDetailsAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
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
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationId, _columns["operation_id"]);
            var name = _adapterHelper.ToStr(entity.Name, _columns["name"]);
            var quantity = _adapterHelper.ToStr(entity.Quantity, _columns["quantity"]);
            var amount = _adapterHelper.ToStr(entity.Amount, _columns["amount"]);
            return _adapterHelper.Insert(id, operationId, name, quantity, amount);
        }

        public void Update(DalOperationDetails entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationId, _columns["operation_id"]);
            var name = _adapterHelper.ToStr(entity.Name, _columns["name"]);
            var quantity = _adapterHelper.ToStr(entity.Quantity, _columns["quantity"]);
            var amount = _adapterHelper.ToStr(entity.Amount, _columns["amount"]);
            _adapterHelper.Update(id, operationId, name, quantity, amount);
        }
    }
}

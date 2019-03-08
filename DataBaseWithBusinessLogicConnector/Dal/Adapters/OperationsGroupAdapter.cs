using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationsGroupAdapter : IAdapter<DalOperationsGroup>
    {
        private const string Table = "operations_groups";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["user_id"] = DataType.IntegerNullable,
            ["description"] = DataType.String,
            ["frequent_id"] = DataType.IntegerNullable,
            ["importance_id"] = DataType.IntegerNullable,
            ["date"] = DataType.String,
        };

        private readonly AdapterHelper _adapterHelper;

        public OperationsGroupAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
        }

        public void Delete(DalOperationsGroup entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalOperationsGroup> GetAll(string filter = "")
        {
            var result = new List<DalOperationsGroup>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;

                result.Add(new DalOperationsGroup(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), dataRow[2].ToString(), int.Parse(dataRow[3].ToString()), int.Parse(dataRow[4].ToString()), dataRow[5].ToString()));
            }

            return result;
        }

        public DalOperationsGroup GetById(int? id)
        {
            DalOperationsGroup result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;

                result = new DalOperationsGroup(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), dataRow[2].ToString(), int.Parse(dataRow[3].ToString()), int.Parse(dataRow[4].ToString()), dataRow[5].ToString());
            }

            return result;
        }

        public int Insert(DalOperationsGroup entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, _columns["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, _columns["description"]);
            var frequencyId = _adapterHelper.ToStr(entity.FrequencyId, _columns["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, _columns["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, _columns["date"]);
            return _adapterHelper.Insert(id, userId, description, frequencyId, importanceId, date);
        }

        public void Update(DalOperationsGroup entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, _columns["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, _columns["description"]);
            var frequencyId = _adapterHelper.ToStr(entity.FrequencyId, _columns["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, _columns["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, _columns["date"]);
            _adapterHelper.Update(id, id, userId, description, frequencyId, importanceId, date);
        }
    }
}

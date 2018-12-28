using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationsGroupAdapter : IAdapter<DalOperationsGroup>
    {
        private readonly string TABLE = "operations_groups";
        private readonly Dictionary<string, DataType> COLUMNS = new Dictionary<string, DataType>
        {
            ["id"] = DataType.INTEGER_NULLABLE,
            ["user_id"] = DataType.INTEGER_NULLABLE,
            ["description"] = DataType.STRING,
            ["frequent_id"] = DataType.INTEGER_NULLABLE,
            ["importance_id"] = DataType.INTEGER_NULLABLE,
            ["date"] = DataType.STRING,
        };

        private AdapterHelper _adapterHelper;

        public OperationsGroupAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.Keys.ToList());
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
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, COLUMNS["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, COLUMNS["description"]);
            var frequenceId = _adapterHelper.ToStr(entity.FrequenceId, COLUMNS["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, COLUMNS["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, COLUMNS["date"]);
            return _adapterHelper.Insert(id, userId, description, frequenceId, importanceId, date);
        }

        public void Update(DalOperationsGroup entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var userId = _adapterHelper.ToStr(entity.UserId, COLUMNS["user_id"]);
            var description = _adapterHelper.ToStr(entity.Description, COLUMNS["description"]);
            var frequenceId = _adapterHelper.ToStr(entity.FrequenceId, COLUMNS["frequent_id"]);
            var importanceId = _adapterHelper.ToStr(entity.ImportanceId, COLUMNS["importance_id"]);
            var date = _adapterHelper.ToStr(entity.Date, COLUMNS["date"]);
            _adapterHelper.Update(id, userId, description, frequenceId, importanceId, date);
        }
    }
}

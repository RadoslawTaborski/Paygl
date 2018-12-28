using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationsGroupTagAdapter
    {
        private readonly string TABLE = "operations_group_tags";
        private readonly Dictionary<string, DataType> COLUMNS = new Dictionary<string, DataType>
        {
            ["id"] = DataType.INTEGER_NULLABLE,
            ["operation_group_id"] = DataType.INTEGER_NULLABLE,
            ["tag_id"] = DataType.INTEGER_NULLABLE,
        };

        private AdapterHelper _adapterHelper;

        public OperationsGroupTagAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.Keys.ToList());
        }

        public void Delete(DalOperationsGroupTag entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalOperationsGroupTag> GetAll(string filter = "")
        {
            var result = new List<DalOperationsGroupTag>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalOperationsGroupTag(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString())));
            }

            return result;
        }

        public DalOperationsGroupTag GetById(int? id)
        {
            DalOperationsGroupTag result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalOperationsGroupTag(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()));
            }

            return result;
        }

        public int Insert(DalOperationsGroupTag entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationsGroupId, COLUMNS["operation_group_id"]);
            var tagId = _adapterHelper.ToStr(entity.TagId, COLUMNS["tag_id"]);
            return _adapterHelper.Insert(id, operationId, tagId);
        }

        public void Update(DalOperationsGroupTag entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, COLUMNS["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationsGroupId, COLUMNS["operation_group_id"]);
            var tagId = _adapterHelper.ToStr(entity.TagId, COLUMNS["tag_id"]);
            _adapterHelper.Update(id, operationId, tagId);
        }
    }
}

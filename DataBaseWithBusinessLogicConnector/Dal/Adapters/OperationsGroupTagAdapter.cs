using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationsGroupTagAdapter
    {
        private const string Table = "operations_group_tags";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["operation_group_id"] = DataType.IntegerNullable,
            ["tag_id"] = DataType.IntegerNullable,
        };

        private readonly AdapterHelper _adapterHelper;

        public OperationsGroupTagAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
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
            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count <= 0) return null;

            var dataRow = data.Tables[0].Rows[0].ItemArray;
            var result = new DalOperationsGroupTag(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()));

            return result;
        }

        public int Insert(DalOperationsGroupTag entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationsGroupId, _columns["operation_group_id"]);
            var tagId = _adapterHelper.ToStr(entity.TagId, _columns["tag_id"]);
            return _adapterHelper.Insert(id, operationId, tagId);
        }

        public void Update(DalOperationsGroupTag entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var operationId = _adapterHelper.ToStr(entity.OperationsGroupId, _columns["operation_group_id"]);
            var tagId = _adapterHelper.ToStr(entity.TagId, _columns["tag_id"]);
            _adapterHelper.Update(id, operationId, tagId);
        }
    }
}

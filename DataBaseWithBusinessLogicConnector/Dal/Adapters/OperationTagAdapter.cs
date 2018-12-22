using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class OperationTagAdapter : IAdapter<DalOperationTags>
    {
        private readonly string TABLE = "operation_tags";
        private readonly string[] COLUMNS = { "operation_id", "tag_id" };

        private AdapterHelper _adapterHelper;

        public OperationTagAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
        }

        public void Delete(DalOperationTags entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalOperationTags> GetAll(string filter = "")
        {
            var result = new List<DalOperationTags>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalOperationTags(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString())));
            }

            return result;
        }

        public DalOperationTags GetById(int id)
        {
            DalOperationTags result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalOperationTags(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()));
            }

            return result;
        }

        public void Insert(DalOperationTags entity)
        {
            _adapterHelper.Insert(entity.OperationId.ToString(), entity.TagId.ToString());
        }

        public void Update(DalOperationTags entity)
        {
            _adapterHelper.Update(entity.Id, entity.OperationId.ToString(), entity.TagId.ToString());
        }
    }
}

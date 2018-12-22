using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class TagAdapter : IAdapter<DalTag>
    {
        private readonly string TABLE = "tags";
        private readonly string[] COLUMNS = { "text", "language_id" };

        private AdapterHelper _adapterHelper;

        public TagAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
        }

        public void Delete(DalTag entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalTag> GetAll(string filter = "")
        {
            var result = new List<DalTag>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalTag(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString())));
            }

            return result;
        }

        public DalTag GetById(int id)
        {
            DalTag result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalTag(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString()));
            }

            return result;
        }

        public void Insert(DalTag entity)
        {
            _adapterHelper.Insert(entity.Text, entity.LanguageId.ToString());
        }

        public void Update(DalTag entity)
        {
            _adapterHelper.Update(entity.Id, entity.Text, entity.LanguageId.ToString());
        }
    }
}

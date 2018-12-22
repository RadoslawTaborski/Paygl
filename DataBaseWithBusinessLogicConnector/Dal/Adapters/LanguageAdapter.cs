using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class LanguageAdapter : IAdapter<DalLanguage>
    {
        private readonly string TABLE = "languages";
        private readonly string[] COLUMNS = { "short", "full" };

        private AdapterHelper _adapterHelper;

        public LanguageAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
        }

        public void Delete(DalLanguage entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalLanguage> GetAll(string filter = "")
        {
            var result = new List<DalLanguage>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalLanguage(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString()));
            }

            return result;
        }

        public DalLanguage GetById(int id)
        {
            DalLanguage result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalLanguage(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString());
            }

            return result;
        }

        public void Insert(DalLanguage entity)
        {
            _adapterHelper.Insert(entity.ShortName, entity.FullName);
        }

        public void Update(DalLanguage entity)
        {
            _adapterHelper.Update(entity.Id, entity.ShortName, entity.FullName);
        }
    }
}

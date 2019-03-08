using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class LanguageAdapter : IAdapter<DalLanguage>
    {
        private const string Table = "languages";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["short"] = DataType.String,
            ["full"] = DataType.String,
        };

        private readonly AdapterHelper _adapterHelper;

        public LanguageAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
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

        public DalLanguage GetById(int? id)
        {
            DalLanguage result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count <= 0) return result;

            var dataRow = data.Tables[0].Rows[0].ItemArray;
            result = new DalLanguage(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString());

            return result;
        }

        public int Insert(DalLanguage entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var shortName = _adapterHelper.ToStr(entity.ShortName, _columns["short"]);
            var fullName = _adapterHelper.ToStr(entity.FullName, _columns["full"]);
            return _adapterHelper.Insert(id, shortName, fullName);
        }

        public void Update(DalLanguage entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var shortName = _adapterHelper.ToStr(entity.ShortName, _columns["short"]);
            var fullName = _adapterHelper.ToStr(entity.FullName, _columns["full"]);
            _adapterHelper.Update(id, shortName, fullName);
        }
    }
}

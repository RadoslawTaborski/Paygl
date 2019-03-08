using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class UserDetailsAdapter : IAdapter<DalUserDetails>
    {
        private const string Table = "user_details";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["last_name"] = DataType.String,
            ["first_name"] = DataType.String,
        };

        private readonly AdapterHelper _adapterHelper;

        public UserDetailsAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
        }

        public void Delete(DalUserDetails entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalUserDetails> GetAll(string filter = "")
        {
            var result = new List<DalUserDetails>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalUserDetails(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString()));
            }

            return result;
        }

        public DalUserDetails GetById(int? id)
        {
            DalUserDetails result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalUserDetails(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString());
            }

            return result;
        }

        public int Insert(DalUserDetails entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var lastName = _adapterHelper.ToStr(entity.LastName, _columns["last_name"]);
            var firstName = _adapterHelper.ToStr(entity.FirstName, _columns["first_name"]);
            return _adapterHelper.Insert(id, lastName, firstName);
        }

        public void Update(DalUserDetails entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var lastName = _adapterHelper.ToStr(entity.LastName, _columns["last_name"]);
            var firstName = _adapterHelper.ToStr(entity.FirstName, _columns["first_name"]);
            _adapterHelper.Update(id, lastName, firstName);
        }
    }
}

using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class UserAdapter : IAdapter<DalUser>
    {
        private const string Table = "users";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["login"] = DataType.String,
            ["password"] = DataType.String,
            ["details_id"] = DataType.IntegerNullable,
        };

        private readonly AdapterHelper _adapterHelper;

        public UserAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
        }

        public void Delete(DalUser entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalUser> GetAll(string filter = "")
        {
            var result = new List<DalUser>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalUser(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString(), int.Parse(dataRow[3].ToString())));
            }

            return result;
        }

        public DalUser GetById(int? id)
        {
            DalUser result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalUser(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString(), int.Parse(dataRow[3].ToString()));
            }

            return result;
        }

        public int Insert(DalUser entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var login = _adapterHelper.ToStr(entity.Login, _columns["login"]);
            var password = _adapterHelper.ToStr(entity.Password, _columns["password"]);
            var detailsId = _adapterHelper.ToStr(entity.DetailsId, _columns["details_id"]);
            return _adapterHelper.Insert(id, login, password, detailsId);
        }

        public void Update(DalUser entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var login = _adapterHelper.ToStr(entity.Login, _columns["login"]);
            var password = _adapterHelper.ToStr(entity.Password, _columns["password"]);
            var detailsId = _adapterHelper.ToStr(entity.DetailsId, _columns["details_id"]);
            _adapterHelper.Update(id, login, password,detailsId);
        }
    }
}

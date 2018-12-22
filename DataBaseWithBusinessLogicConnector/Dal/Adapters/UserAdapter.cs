using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class UserAdapter : IAdapter<DalUser>
    {
        private readonly string TABLE = "users";
        private readonly string[] COLUMNS = { "login", "password", "details_id" };

        private AdapterHelper _adapterHelper;

        public UserAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
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

        public DalUser GetById(int id)
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

        public void Insert(DalUser entity)
        {
            _adapterHelper.Insert(entity.Login, entity.Password, entity.DetailsId.ToString());
        }

        public void Update(DalUser entity)
        {
            _adapterHelper.Update(entity.Id, entity.Login, entity.Password, entity.DetailsId.ToString());
        }
    }
}

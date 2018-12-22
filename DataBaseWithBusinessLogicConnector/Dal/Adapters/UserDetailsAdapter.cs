using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class UserDetailsAdapter : IAdapter<DalUserDetails>
    {
        private readonly string TABLE = "user_details";
        private readonly string[] COLUMNS = { "last_name", "first_name" };

        private AdapterHelper _adapterHelper;

        public UserDetailsAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
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

        public DalUserDetails GetById(int id)
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

        public void Insert(DalUserDetails entity)
        {
            _adapterHelper.Insert(entity.LastName, entity.LastName);
        }

        public void Update(DalUserDetails entity)
        {
            _adapterHelper.Update(entity.Id, entity.LastName, entity.LastName);
        }
    }
}

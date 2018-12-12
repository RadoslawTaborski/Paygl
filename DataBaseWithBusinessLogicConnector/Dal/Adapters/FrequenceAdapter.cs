using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class FrequenceAdapter : IAdapter<DalFrequence>
    {
        private DbConnector _connection;

        public FrequenceAdapter(DbConnector connection)
        {
            _connection = connection;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalFrequence> GetAll(string filter="")
        {
            var result = new List<DalFrequence>();
            var query = "SELECT * FROM `frequencies`";
            if (filter != "")
            {
                query += " WHERE " + filter;
            }
            _connection.DataAccess.ConnectToDb();
            var data = _connection.DataAccess.ExecuteSqlCommand(query);
            _connection.DataAccess.Disconnect();

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalFrequence(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString())));
            }

            return result;
        }

        public DalFrequence GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(DalFrequence entity)
        {
            _connection.DataAccess.ConnectToDb();
            _connection.DataAccess.ExecuteNonQueryDb($"INSERT INTO `frequencies` (`id`, `text`) VALUES(NULL, '{entity.Text}');");
            _connection.DataAccess.Disconnect();    
        }

        public void Update(int id, DalFrequence entity)
        {
            throw new NotImplementedException();
        }

        private static class Queries
        {
            const string select = "";
            const string insert = "";
            const string delete = "";
            const string update = "";
        }
    }
}

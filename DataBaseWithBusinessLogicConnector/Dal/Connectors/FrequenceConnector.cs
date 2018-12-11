using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Connectors
{
    public class FrequenceConnector : IEntityConnector<DalFrequence>
    {
        private DbConnector _connection;

        public FrequenceConnector(DbConnector connection)
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
                result.Add(new DalFrequence(int.Parse(dataRow[0].ToString()), dataRow[1].ToString()));
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
    }
}

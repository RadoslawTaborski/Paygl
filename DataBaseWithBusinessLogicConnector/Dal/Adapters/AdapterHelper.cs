using System;
using System.Collections.Generic;
using System.Data;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class AdapterHelper
    {
        private readonly DbConnector _connection;
        private readonly Queries _queries;

        public AdapterHelper(DbConnector connection, string tableName, List<string> columns)
        {
            _connection = connection;
            _queries = new Queries(tableName, columns);
        }

        public void Delete(int? id)
        {
            if (id.HasValue)
            {
                string query = _queries.Delete;
                query += string.Format(_queries.Where, $"id={id}");
                _connection.DataAccess.ConnectToDb();
                _connection.DataAccess.ExecuteNonQueryDb(query);
                _connection.DataAccess.Disconnect();
            }
        }

        public DataSet GetAll(string filter)
        {
            string query = _queries.Select;
            if (filter != "")
            {
                query += string.Format(_queries.Where, filter);
            }
            _connection.DataAccess.ConnectToDb();
            var data = _connection.DataAccess.ExecuteSqlCommand(query);
            _connection.DataAccess.Disconnect();

            return data;
        }

        public DataSet GetById(int? id)
        {
            if (id.HasValue)
            {
                string query = _queries.Select;
                query += string.Format(_queries.Where, $"id={id}");
                _connection.DataAccess.ConnectToDb();
                var data = _connection.DataAccess.ExecuteSqlCommand(query);
                _connection.DataAccess.Disconnect();
                return data;
            }
            return null;
        }

        public int Insert(params string[] args)
        {
            var query = string.Format(_queries.Insert, args);
            _connection.DataAccess.ConnectToDb();
            _connection.DataAccess.ExecuteNonQueryDb(query);
            var queryResults = _connection.DataAccess.ExecuteSqlCommand(_queries.LastInsert);
            var id = int.Parse(queryResults.Tables[0].Rows[0].ItemArray[0].ToString());
            _connection.DataAccess.Disconnect();

            return id;
        }

        public void Update(string id, params string[] args)
        {
            var query = string.Format(_queries.Update, args);
            query += string.Format(_queries.Where, $"id={id}");
            _connection.DataAccess.ConnectToDb();
            _connection.DataAccess.ExecuteNonQueryDb(query);
            _connection.DataAccess.Disconnect();
        }

        public string ToStr(object obj, DataType dt)
        {
            switch (dt)
            {
                case DataType.Integer:
                    return $"'{obj}'";
                case DataType.IntegerNullable:
                    var nullable = obj as int?;
                    if (nullable.HasValue)
                    {
                        return $"'{obj}'";
                    }
                    return "NULL";
                case DataType.Decimal:
                    var dec = obj as decimal?;
                    return $"'{dec.Value}'".Replace(",",".");
                case DataType.Double:
                    return $"'{obj}'".Replace(",", ".");
                case DataType.String:
                    return $"'{obj}'";
                default:
                    throw new NotImplementedException();
            }
        }

        private class Queries
        {
            private readonly string _tableName;
            private readonly List<string> _columns;

            public string Select { get; private set; }
            public string Insert { get; private set; }
            public string LastInsert { get; private set; }
            public string Delete { get; private set; }
            public string Update { get; private set; }
            public string Where { get; private set; }

            public Queries(string tableName, List<string> columns)
            {
                _tableName = tableName;
                _columns = columns;
                SetQueries();
            }

            private void SetQueries()
            {
                var parametersList = new List<string>();
                _columns.ForEach(item => parametersList.Add($"`{item}`"));

                var valuesList = new List<string>();
                var index = 0;
                _columns.ForEach(item => valuesList.Add($"{{{index++}}}"));

                Select = $"SELECT * FROM `{_tableName}`";

                Insert = "INSERT INTO `{0}` ({1}) VALUES ({2})";
                var parameters = string.Join(", ", parametersList.ToArray());
                var values = string.Join(", ", valuesList.ToArray());
                Insert = string.Format(Insert, _tableName, parameters, values);

                Delete = $"DELETE FROM `{_tableName}`";

                Update = "UPDATE `{0}` SET {1}";
                var setList = new List<string>();
                index = 0;
                _columns.ForEach(item => setList.Add($"{parametersList[index]} = {valuesList[index++]}"));
                var set = string.Join(", ", setList.ToArray());
                Update = string.Format(Update, _tableName, set);

                Where = " WHERE {0}";

                LastInsert = $"SELECT LAST_INSERT_ID(`id`) as ID FROM `{_tableName}` ORDER BY ID DESC";
            }
        }
    }
}

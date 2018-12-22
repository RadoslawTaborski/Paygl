﻿using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class AdapterHelper
    {
        private DbConnector _connection;
        private Queries queries;

        public AdapterHelper(DbConnector connection, string tableName, List<string> columns)
        {
            _connection = connection;
            queries = new Queries(tableName, columns);
        }

        public void Delete(int id)
        {
            string query = queries.Update;
            query += string.Format(queries.Where, $"id={id}");
            _connection.DataAccess.ConnectToDb();
            _connection.DataAccess.ExecuteNonQueryDb(query);
            _connection.DataAccess.Disconnect();
        }

        public DataSet GetAll(string filter)
        {
            string query = queries.Select;
            if (filter != "")
            {
                query += string.Format(queries.Where, filter);
            }
            _connection.DataAccess.ConnectToDb();
            var data = _connection.DataAccess.ExecuteSqlCommand(query);
            _connection.DataAccess.Disconnect();

            return data;
        }

        public DataSet GetById(int id)
        {
            string query = queries.Select;
            query += string.Format(queries.Where, $"id={id}");
            _connection.DataAccess.ConnectToDb();
            var data = _connection.DataAccess.ExecuteSqlCommand(query);
            _connection.DataAccess.Disconnect();

            return data;
        }

        public void Insert(params string[] args)
        {
            var query = string.Format(queries.Insert, args);
            _connection.DataAccess.ConnectToDb();
            _connection.DataAccess.ExecuteNonQueryDb(query);
            _connection.DataAccess.Disconnect();
        }

        public void Update(int id, params string[] args)
        {
            string query = string.Format(queries.Update, args);
            query += string.Format(queries.Where, $"id={id}");
            _connection.DataAccess.ConnectToDb();
            _connection.DataAccess.ExecuteNonQueryDb(query);
            _connection.DataAccess.Disconnect();
        }

        private class Queries
        {
            private string _tableName;
            private List<string> _columns;

            public string Select { get; private set; }
            public string Insert { get; private set; }
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
            }
        }
    }
}

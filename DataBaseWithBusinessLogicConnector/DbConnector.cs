using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;

namespace DataBaseWithBusinessLogicConnector
{
    public class DbConnector
    {
        private IDataAccess _dataAccess;
        internal IDataAccess DataAccess
        {
            get
            {
                if(_dataAccess==null)
                {
                    throw new NullReferenceException("Connection to database is not set");
                }
                return _dataAccess;
            }
        }

        public DbConnector(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
    }
}

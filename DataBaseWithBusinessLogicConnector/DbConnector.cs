using System;
using DataAccess.Interfaces;

namespace DataBaseWithBusinessLogicConnector
{
    public class DbConnector
    {
        private readonly IDataAccess _dataAccess;
        internal IDataAccess DataAccess
        {
            get
            {
                if(_dataAccess==null)
                {
                    throw new NullReferenceException(Properties.strings.ExConnectionDBIsNotSet);
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

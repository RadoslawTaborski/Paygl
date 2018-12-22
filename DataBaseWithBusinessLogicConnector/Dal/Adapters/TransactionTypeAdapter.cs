using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class TransactionTypeAdapter : IAdapter<DalTransactionType>
    {
        private readonly string TABLE = "transaction_types";
        private readonly string[] COLUMNS = { "text", "language_id" };

        private AdapterHelper _adapterHelper;

        public TransactionTypeAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
        }

        public void Delete(DalTransactionType entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalTransactionType> GetAll(string filter = "")
        {
            var result = new List<DalTransactionType>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalTransactionType(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString())));
            }

            return result;
        }

        public DalTransactionType GetById(int id)
        {
            DalTransactionType result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalTransactionType(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString()));
            }

            return result;
        }

        public void Insert(DalTransactionType entity)
        {
            _adapterHelper.Insert(entity.Text, entity.LanguageId.ToString());
        }

        public void Update(DalTransactionType entity)
        {
            _adapterHelper.Update(entity.Id, entity.Text, entity.LanguageId.ToString());
        }
    }
}

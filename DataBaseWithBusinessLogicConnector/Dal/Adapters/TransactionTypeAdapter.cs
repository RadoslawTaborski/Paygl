using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class TransactionTypeAdapter : IAdapter<DalTransactionType>
    {
        private const string Table = "transaction_types";

        private readonly Dictionary<string, DataType> _columns = new Dictionary<string, DataType>
        {
            ["id"] = DataType.IntegerNullable,
            ["text"] = DataType.String,
            ["language_id"] = DataType.IntegerNullable,
        };

        private readonly AdapterHelper _adapterHelper;

        public TransactionTypeAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, Table, _columns.Keys.ToList());
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

        public DalTransactionType GetById(int? id)
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

        public int Insert(DalTransactionType entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var text = _adapterHelper.ToStr(entity.Text, _columns["text"]);
            var language = _adapterHelper.ToStr(entity.LanguageId, _columns["language_id"]);
            return _adapterHelper.Insert(id, text, language);
        }

        public void Update(DalTransactionType entity)
        {
            var id = _adapterHelper.ToStr(entity.Id, _columns["id"]);
            var text = _adapterHelper.ToStr(entity.Text, _columns["text"]);
            var language = _adapterHelper.ToStr(entity.LanguageId, _columns["language_id"]);
            _adapterHelper.Update(id, text, language);
        }
    }
}

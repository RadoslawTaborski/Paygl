using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseWithBusinessLogicConnector.Dal.Adapters
{
    public class FrequenceAdapter : IAdapter<DalFrequence>
    {
        private readonly string TABLE = "frequencies";
        private readonly string[] COLUMNS = { "text", "language_id" };

        private AdapterHelper _adapterHelper;

        public FrequenceAdapter(DbConnector connector)
        {
            _adapterHelper = new AdapterHelper(connector, TABLE, COLUMNS.ToList());
        }

        public void Delete(DalFrequence entity)
        {
            _adapterHelper.Delete(entity.Id);
        }

        public IEnumerable<DalFrequence> GetAll(string filter = "")
        {
            var result = new List<DalFrequence>();

            var data = _adapterHelper.GetAll(filter);

            for (var i = 0; i < data.Tables[0].Rows.Count; ++i)
            {
                var dataRow = data.Tables[0].Rows[i].ItemArray;
                result.Add(new DalFrequence(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString())));
            }

            return result;
        }

        public DalFrequence GetById(int id)
        {
            DalFrequence result = null;

            var data = _adapterHelper.GetById(id);

            if (data.Tables.Count > 0)
            {
                var dataRow = data.Tables[0].Rows[0].ItemArray;
                result = new DalFrequence(int.Parse(dataRow[0].ToString()), dataRow[1].ToString(), int.Parse(dataRow[2].ToString()));
            }

            return result;
        }

        public void Insert(DalFrequence entity)
        {
            _adapterHelper.Insert(entity.Text, entity.LanguageId.ToString());
        }

        public void Update(DalFrequence entity)
        {
            _adapterHelper.Update(entity.Id, entity.Text, entity.LanguageId.ToString());
        }
    }
}

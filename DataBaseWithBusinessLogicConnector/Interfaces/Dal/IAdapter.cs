using System.Collections.Generic;

namespace DataBaseWithBusinessLogicConnector.Interfaces.Dal
{
    public interface IAdapter <TDalEntity>
    {
        IEnumerable<TDalEntity> GetAll(string filter);
        TDalEntity GetById(int? id);
        int Insert(TDalEntity entity);
        void Update(TDalEntity entity);
        void Delete(TDalEntity entity);
    }
}
